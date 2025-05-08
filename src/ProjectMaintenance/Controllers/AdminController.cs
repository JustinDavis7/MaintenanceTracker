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

namespace ProjectMaintenance.Controllers;

[Authorize(Roles = "admin")]
public class AdminController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;    
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IUserRepository _userRepository;
    private readonly IEquipmentRepository _equipmentRepository;
    private readonly IMaintenanceTicketRepository _maintenanceTicketRepository;


    public AdminController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IUserRepository userRepository, IEquipmentRepository equipmentRepository, IMaintenanceTicketRepository maintenanceTicketRepository)
    {       
        _userManager = userManager;
        _roleManager = roleManager;
        _userRepository = userRepository;
        _equipmentRepository = equipmentRepository;
        _maintenanceTicketRepository = maintenanceTicketRepository;
    }

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> ShowAllUsers()
    {
        var currentUser = await _userManager.GetUserAsync(User); // Current user - The admin viewing all users.
        List<UsersVM> usersVM = new List<UsersVM>();
        
        var users = await _userRepository.GetAllUsers();

        foreach(var user in users)
        {
            var identityUser = await _userManager.FindByIdAsync(user.AspNetUserId);
            if(identityUser.Id != currentUser.Id)
            {
                if (identityUser != null)
                {
                    // Retrieve the roles for the user
                    var roles = await _userManager.GetRolesAsync(identityUser);

                    // If there's only one role, use it. Otherwise, you might want to handle multiple roles
                    var role = roles.FirstOrDefault() ?? "No role assigned";

                    // Create a new UsersVM object and add it to the list
                    usersVM.Add(new UsersVM
                    {
                        UserName = identityUser.UserName,
                        Id = currentUser.Id,
                        Role = role
                    });
                }
            }
        }
    return View(usersVM);
    }

    [HttpGet]
    public async Task<IActionResult> CreateUser()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser(string name, string role)
    {
        // Remove all invalid characters for Identity username (allowing only alphanumeric characters)
        string identityName = name != null ? Regex.Replace(name.Trim(), @"[^a-zA-Z0-9]", "") : name;

        if (string.IsNullOrEmpty(identityName) || string.IsNullOrEmpty(role))
        {
            return Json(new { success = false, message = "Name and role cannot be empty." });
        }

        if (ModelState.IsValid)
        {
            // Create the IdentityUser instance
            var user = new IdentityUser
            {
                UserName = identityName, // Use sanitized name for IdentityUser
            };

            // Use a transaction scope to ensure atomicity
            using (var transaction = await _userRepository.BeginTransactionAsync())
            {
                try
                {
                    // Create the user in the Identity system
                    var result = await _userManager.CreateAsync(user, "Temp_PW1");

                    if (result.Succeeded)
                    {
                        // Add the user to the specified role
                        if (!await _roleManager.RoleExistsAsync(role))
                        {
                            var roleResult = await _roleManager.CreateAsync(new IdentityRole(role));
                            if (!roleResult.Succeeded)
                            {
                                throw new Exception("Failed to create role");
                            }
                        }

                        await _userManager.AddToRoleAsync(user, role);

                        // Create the application-specific user (allow spaces in the Name)
                        var newUser = new User
                        {
                            AspNetUserId = user.Id,
                            Name = identityName, 
                            Role = role
                        };

                        // Use the repository to add or update the application-specific user
                        await _userRepository.AddOrUpdate(newUser);

                        // Commit the transaction if everything is successful
                        await transaction.CommitAsync();

                        return Json(new { success = true });
                    }
                    else
                    {
                        var errorMessage = string.Join("; ", result.Errors.Select(e => e.Description));
                        return Json(new { success = false, message = errorMessage });
                    }
                }
                catch (Exception ex)
                {
                    // Rollback the transaction on failure
                    await transaction.RollbackAsync();
                    return Json(new { success = false, message = ex.Message });
                }
            }
        }

        var modelStateErrors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
        return Json(new { success = false, message = string.Join("; ", modelStateErrors) });
    }

    public async Task<IActionResult> DeleteUser (string userName)
    {
        var user = await _userRepository.GetUserByName(userName);
        var currentAdminUser = _userRepository.GetUserByName(User.Identity.Name).Result;
        var id = currentAdminUser.Id;
        if (user == null)
        {
            return NotFound();
        }
        await _maintenanceTicketRepository.ScrubDeletedUserTicket(user.Id, id);
        await _userRepository.DeleteUser(user);

        return RedirectToAction(nameof(Index));
    }
}