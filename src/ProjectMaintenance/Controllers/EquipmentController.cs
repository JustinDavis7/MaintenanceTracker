using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ProjectMaintenance.DAL.Abstract;
using ProjectMaintenance.Models;

namespace ProjectMaintenance.Controllers
{
    public class EquipmentController : Controller
    {
        private readonly IEquipmentRepository _equipmentRepository;

        public EquipmentController(IEquipmentRepository equipmentRepository)
        {
            _equipmentRepository = equipmentRepository;
        }

        // GET: Equipment
        [Authorize(Roles = "maintenance,admin,maintenanceLead")]
        public async Task<IActionResult> Index()
        {
            return View(await _equipmentRepository.GetAllEquipment());
        }

        // GET: Equipment/Details/5
        [Authorize(Roles = "maintenance,admin,maintenanceLead")]
        public async Task<IActionResult> Details(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var equipment = await _equipmentRepository.GetEquipmentById(id);
            if (equipment == null)
            {
                return NotFound();
            }

            return View(equipment);
        }

        // GET: Equipment/Create
        [Authorize(Roles = "maintenance,admin,maintenanceLead")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Equipment/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,LeadOperator,Vendor,Model,SerialNumber,AcquiredDate,WarrantyExpiration")] Equipment equipment)
        {
            if (ModelState.IsValid)
            {
                await _equipmentRepository.CreateEquipment(equipment.Name, equipment.Description, equipment.LeadOperator, 
                    equipment.Vendor, equipment.Model, equipment.SerialNumber, 
                    equipment.AcquiredDate, equipment.WarrantyExpiration);
                return RedirectToAction(nameof(Index));
            }
            return View(equipment);
        }

        [HttpGet]
        [Authorize(Roles = "admin,maintenanceLead")]
        public async Task<IActionResult> Edit(int id)
        {
            var equipment = await _equipmentRepository.GetEquipmentById(id);
            if(equipment == null)
            {
                return NotFound();
            }
            return View(equipment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin,maintenanceLead")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,LeadOperator,Vendor,Model,SerialNumber,AcquiredDate,WarrantyExpiration,Description")] Equipment equipmentEdited)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var equipment = await _equipmentRepository.GetEquipmentById(id);

                    if (equipment == null)
                    {
                        return NotFound();
                    }
                    equipment.Name = equipmentEdited.Name;
                    equipment.LeadOperator = equipmentEdited.LeadOperator;
                    equipment.Vendor = equipmentEdited.Vendor;
                    equipment.Model = equipmentEdited.Model;
                    equipment.SerialNumber = equipmentEdited.SerialNumber;
                    equipment.AcquiredDate = equipmentEdited.AcquiredDate;
                    equipment.WarrantyExpiration = equipmentEdited.WarrantyExpiration;
                    equipment.Description = equipmentEdited.Description;

                    _equipmentRepository.Update(equipment);

                    return RedirectToAction(nameof(Details), new { id = id });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = ex.Message });
                }
            }


            return View(equipmentEdited);
        }

        // GET: Equipment/Delete/5
        [Authorize(Roles = "admin,maintenanceLead")]
        public async Task<IActionResult> Delete(int id)
        {
            var equipment = await _equipmentRepository.GetEquipmentById(id);
            if (equipment == null)
            {
                return NotFound();
            }

            return View(equipment);
        }

        // POST: Equipment/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "admin,maintenanceLead")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _equipmentRepository.DeleteEquipmentById(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
