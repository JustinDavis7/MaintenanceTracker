using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProjectMaintenance.DAL.Abstract;
using ProjectMaintenance.Models;

namespace ProjectMaintenance.Controllers;

[AllowAnonymous]
public class HomeController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IUserRepository _userRepository;
    private readonly IEquipmentRepository _equipmentRepository;
    private readonly IMaintenanceTicketRepository _maintenanceTicketRepository;

    public HomeController(UserManager<IdentityUser> userManager, IUserRepository userRepository, IEquipmentRepository equipmentRepository, IMaintenanceTicketRepository maintenanceTicketRepository)
    {       
        _userManager = userManager;
        _userRepository = userRepository;
        _equipmentRepository = equipmentRepository;
        _maintenanceTicketRepository = maintenanceTicketRepository;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
