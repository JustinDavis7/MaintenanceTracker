using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProjectMaintenance.DAL.Abstract;
using ProjectMaintenance.Models;
using ProjectMaintenance.Utilities;
using ProjectMaintenance.ViewModels;
using System.Text.RegularExpressions;
using System;
using System.Data;

namespace ProjectMaintenance.Controllers
{

    [Authorize]
    public class CalendarController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEquipmentRepository _equipmentRepository;

        public CalendarController(UserManager<IdentityUser> userManager, IEquipmentRepository equipmentRepository)
        {
            _userManager = userManager;
            _equipmentRepository = equipmentRepository;
        }
        public async Task<IActionResult> Index()
        {
            var equipment = await _equipmentRepository.GetAllEquipment();
            var user = await _userManager.GetUserAsync(User);
            var roles = await _userManager.GetRolesAsync(user);

            var viewModel = new CalendarIndexViewModel {
                Equipment = equipment,
                Role = roles.SingleOrDefault()
            };
            return View(viewModel);
        }
    }
}