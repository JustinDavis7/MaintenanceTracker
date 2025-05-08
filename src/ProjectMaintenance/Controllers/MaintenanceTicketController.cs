using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectMaintenance.DAL.Abstract;
using ProjectMaintenance.Models;
using Microsoft.Extensions.Logging;
using ProjectMaintenance.ViewModels;


namespace ProjectMaintenance.Controllers
{
    [Authorize(Roles = "maintenance,admin,maintenanceLead")]
    public class MaintenanceTicketController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<MaintenanceTicketController> _logger;
        private readonly IMaintenanceTicketRepository _maintenanceTicketRepository;
        private readonly IUserRepository _userRepository;
        private readonly IEquipmentRepository _equipmentRepository;
        private readonly IViewRenderService _viewRenderService;

        public MaintenanceTicketController( UserManager<IdentityUser> userManager, IMaintenanceTicketRepository maintenanceTicketRepository, 
                                            IEquipmentRepository equipmentRepository, IUserRepository userRepository,
                                            ILogger<MaintenanceTicketController> logger, IViewRenderService viewRenderService)
        {
            _userManager = userManager;
            _maintenanceTicketRepository = maintenanceTicketRepository;
            _equipmentRepository = equipmentRepository;
            _userRepository = userRepository;
            _logger = logger;
            _viewRenderService = viewRenderService;
        }

        // GET: MaintenanceTicket
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var viewModel = new MaintenanceIndexViewModel {
                Tickets = await _maintenanceTicketRepository.GetAll()
                    .Include(m => m.Equipment)
                    .Include(m => m.TicketCreatorNavigation)
                    .OrderBy(m => m.PriorityLevel)
                    .ToListAsync(),
                Equipment = await _equipmentRepository.GetAllEquipment()
            };
            return View(viewModel);
        }

        // GET: MaintenanceTicket/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var ticket = await _maintenanceTicketRepository.GetMaintenanceTicket(id);
            var equipment = await _equipmentRepository.GetEquipmentById(ticket.EquipmentId);
            var user = await _userRepository.GetUserById(ticket.TicketCreatorId);

            var viewModel = new MaintenanceDetailViewModel {
                Ticket = ticket,
                Equipment = equipment,
                User = user
            };

            return View(viewModel);
        }

        // POST: MaintenanceTicket/Create
        [HttpPost]
        public async Task<IActionResult> Create(string title, int equipmentId, string description, int priorityLevel, DateOnly plannedCompletion, string partsList, string maintenanceType, bool priorityBump, DateOnly requestCreationDate, string assignedWorker)
        {
            var user = _userRepository.GetUserByName(User.Identity.Name).Result;
            var id = user.Id;

             //if (ModelState.IsValid)
             //{
                await _maintenanceTicketRepository.CreateMaintenanceTicket(equipmentId, id, title, description, maintenanceType, priorityLevel, requestCreationDate, plannedCompletion, partsList, priorityBump, assignedWorker);
                return Ok();
            //}
            //return BadRequest();
        }

        [HttpGet]
        [Authorize(Roles = "admin,maintenanceLead")]
        public async Task<IActionResult> Edit(int id)
        {
            var maintenanceTicket = await _maintenanceTicketRepository.GetMaintenanceTicket(id);

            if (maintenanceTicket == null)
            {
                return NotFound();
            }

            var viewModel = new MaintenanceTicketViewModel
            {
                assignedWorker = maintenanceTicket.AssignedWorker,
                priorityLevel = maintenanceTicket.PriorityLevel,
                maintenanceType = maintenanceTicket.MaintenanceType,
                plannedCompletion = maintenanceTicket.PlannedCompletion,
                partsList = maintenanceTicket.PartsList,
                description = maintenanceTicket.Description
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin,maintenanceLead")]
        public async Task<IActionResult> Edit(int id, MaintenanceTicketViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var maintenanceTicket = await _maintenanceTicketRepository.GetMaintenanceTicket(id);

            if (maintenanceTicket == null)
            {
                return NotFound();
            }

            // Update the properties you want to change
            maintenanceTicket.AssignedWorker = viewModel.assignedWorker;
            maintenanceTicket.PriorityLevel = viewModel.priorityLevel;
            maintenanceTicket.MaintenanceType = viewModel.maintenanceType;
            maintenanceTicket.PlannedCompletion = viewModel.plannedCompletion;
            maintenanceTicket.PartsList = viewModel.partsList;
            maintenanceTicket.Description = viewModel.description;

            _maintenanceTicketRepository.Update(maintenanceTicket);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateTicketStatus(int ticketId, string newStatus)
        {
            var ticket = await _maintenanceTicketRepository.GetMaintenanceTicket(ticketId);
            if (ticket == null)
            {
                return NotFound($"Ticket with ID {ticketId} not found");
            }

            var user = await _userManager.GetUserAsync(User);
            var roles = await _userManager.GetRolesAsync(user);
            
            if ((newStatus.Equals("archived", StringComparison.OrdinalIgnoreCase) ||
                newStatus.Equals("active", StringComparison.OrdinalIgnoreCase)) && 
                !(roles.Contains("admin") || roles.Contains("maintenanceLead")))
            {
                return Forbid(); // Return 403 Forbidden if user is not an Admin or Maintenance Lead
            }

            if (newStatus.Equals("satisfied", StringComparison.OrdinalIgnoreCase) && 
                !roles.Contains("admin") && 
                !roles.Contains("maintenance") &&
                !roles.Contains("maintenanceLead"))
            {
                return Forbid(); // Return 403 Forbidden if user is neither Admin nor maintenance
            }

            try
            {
                var equipmentId = ticket.EquipmentId;
                switch (newStatus.ToLower())
                {
                    case "active":
                        await _maintenanceTicketRepository.ActivateTicket(ticketId, equipmentId);
                        break;
                    case "satisfied":
                        await _maintenanceTicketRepository.SatisfyTicket(ticketId, equipmentId);
                        break;
                    case "closed":
                        await _maintenanceTicketRepository.CloseTicket(ticketId, equipmentId);
                        break;
                    case "archived":
                        await _maintenanceTicketRepository.ArchiveTicket(ticketId, equipmentId);
                        break;
                    default:
                        // Handle unexpected status value
                        return BadRequest("Invalid status value");
                }
                    // Render the HTML for the ticket
                    var ticketHtml = await _viewRenderService.RenderViewToStringAsync("_TicketPartial", ticket);
                    return Json(new { ticketHtml = ticketHtml });
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, "Error updating ticket status");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
