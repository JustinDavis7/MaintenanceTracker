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

namespace ProjectMaintenance.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository, UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
            _userRepository = userRepository;
        }

        // GET: User
        public async Task<IActionResult> Index()
        {
            var users = await _userRepository.GetAllUsers();
            if (users == null)
            {
                return Problem("Entity set 'ProjectMaintenanceDbContex.Users' is null.");
            }
            return View(users);
        }

        // GET: User/Details/5
        public async Task<IActionResult> Details(string? userAspId)
        {
            if (userAspId == null)
            {
                return NotFound();
            }

            var user = await _userRepository.GetUserById(userAspId);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: User/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AspNetUserId,Name")] User user)
        {
            if (ModelState.IsValid)
            {
                await _userRepository.AddOrUpdate(user);
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: User/Delete/5
        public async Task<IActionResult> Delete(string userAspId)
        {
            var user = await _userRepository.GetUserById(userAspId);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string userAspId)
        {
            var user = await _userRepository.GetUserById(userAspId);
            if (user == null)
            {
                return NotFound();
            }

            await _userRepository.DeleteUser(user);
            
            return RedirectToAction(nameof(Index));
        }
    }
}
