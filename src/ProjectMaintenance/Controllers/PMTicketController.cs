using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectMaintenance.DAL.Abstract;
using ProjectMaintenance.Models;

namespace ProjectMaintenance.Controllers
{
    public class PMTicketController : Controller
    {
        private readonly ProjectMaintenanceDbContext _context;
        private readonly IPMTicketRepository _pMTicketRepository;
        private readonly IEquipmentRepository _equipmentRepository;

        public PMTicketController(ProjectMaintenanceDbContext context, IPMTicketRepository pMTicketRepository, IEquipmentRepository equipmentRepository)
        {
            _context = context;
            _pMTicketRepository = pMTicketRepository;
            _equipmentRepository = equipmentRepository;
        }

        // GET: PMTicket
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var pMTickets = await _pMTicketRepository.GetAllPMTickets();
            return View(pMTickets);
        }

        // GET: PMTicket/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int id)
        {
            var pMTicket = await _pMTicketRepository.GetPMTicket(id);
            if (pMTicket == null)
            {
                return NotFound();
            }
            var equipment = await _equipmentRepository.GetEquipmentById(pMTicket.EquipmentId);
            if (equipment == null)
            {
                return NotFound();
            }

            var pMTicketDetailVM = new PMTicketDetailViewModel {
                pMTicket = pMTicket,
                equipment = equipment
            };

            return View(pMTicketDetailVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin,maintenanceLead")]
        public async Task<IActionResult> Create([Bind("EquipmentId,DatePerformed,Title")] PMTicket pMTicket)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _pMTicketRepository.CreatePMTicket(pMTicket.EquipmentId, pMTicket.DatePerformed, pMTicket.Title);
                    return Json(new { success = true, title = pMTicket.Title, datePerformed = pMTicket.DatePerformed });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = ex.Message });
                }
            }
            return Json(new { success = false, message = "Model state is invalid." });
        }

        // GET: PMTicket/Delete/5
        [Authorize(Roles = "admin,maintenanceLead")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pMTicket = await _context.PMTickets
                .Include(p => p.Equipment)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pMTicket == null)
            {
                return NotFound();
            }

            return View(pMTicket);
        }

        // POST: PMTicket/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin,maintenanceLead")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pMTicket = await _pMTicketRepository.GetPMTicket(id);
            if (pMTicket != null)
            {
                _pMTicketRepository.DeletePMTicket(id);
            }

            return RedirectToAction("Index", "Calendar");
        }

        [Authorize]
        public async Task<IActionResult> GetPmTickets()
        {
            var events = await _pMTicketRepository.GetAll()
                .Select(t => new PMTicketEventViewModel
                {
                    Title = t.Title,
                    DatePerformed = t.DatePerformed.ToDateTime(TimeOnly.MinValue), // Convert DateOnly to DateTime
                    EquipmentId = t.EquipmentId,
                    EquipmentName = t.Equipment.Name,
                    TicketId = t.Id
                })
                .ToListAsync();

            return Json(events);
        }

        [Authorize(Roles = "admin,maintenanceLead")]
        public async Task<IActionResult> CompleteTicket(int id)
        {
            var pMTicket = await _pMTicketRepository.GetPMTicket(id);
            _pMTicketRepository.CompletePMTicket(pMTicket);
            return RedirectToAction("Index", "Calendar");
        }

        [HttpGet]
        [Authorize(Roles = "admin,maintenanceLead")]
        public async Task<IActionResult> Edit(int id)
        {
            var pMTicket = await _pMTicketRepository.GetPMTicket(id);
            if (pMTicket == null)
            {
                return NotFound();
            }
            return View(pMTicket);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin,maintenanceLead")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,DatePerformed")] PMTicket pMTicket)
        {
            if (id != pMTicket.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Retrieve the existing ticket to preserve the EquipmentId
                    var existingTicket = await _pMTicketRepository.GetPMTicket(id);
                    if (existingTicket == null)
                    {
                        return NotFound();
                    }

                    // Update only the Title and DatePerformed fields
                    existingTicket.Title = pMTicket.Title;
                    existingTicket.DatePerformed = pMTicket.DatePerformed;

                    await _pMTicketRepository.Update(existingTicket);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PMTicketExists(pMTicket.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            return View(pMTicket);
        }

        [Authorize(Roles = "admin,maintenanceLead")]
        private bool PMTicketExists(int id)
        {
            return _pMTicketRepository.Exists(id);
        }

    }
}
