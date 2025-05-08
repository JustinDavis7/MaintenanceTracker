using ProjectMaintenance.Data;
using ProjectMaintenance.Models;
using ProjectMaintenance.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectMaintenance.Utilities
{
    public static class SeedUsers
    {
        /// <summary>
        /// Initialize seed data for users.  Creates users for login using Identity and also application users.  One password
        /// is used for all accounts.
        /// </summary>
        /// <param name="serviceProvider">The fully configured service provider for this app that can provide a UserManager and the applications dbcontext</param>
        /// <param name="seedData">Array of seed data holding all the attributes needed to create the user objects</param>
        /// <param name="testUserPw">Password for all seed accounts</param>
        /// <returns></returns>
        public static async Task Initialize(IServiceProvider serviceProvider, UserInfoData[] seedData, string testUserPw)
        {
            try
            {
                // Get our application db context
                //   For later reference -- this uses the "Service Locator anti-pattern", not usually a good pattern
                //   but unavoidable here
                using (var context = new ProjectMaintenanceDbContext(serviceProvider.GetRequiredService<DbContextOptions<ProjectMaintenanceDbContext>>()))
                {
                    // Get the Identity user manager
                    var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

                    foreach(var u in seedData)
                    {
                        // Ensure this user exists or is newly created (Email is used for username since that is the default in Register and Login -- change those and then use username here if you want it different than email
                        var identityID = await EnsureUser(userManager, testUserPw, u.Name, u.Id);
                        // Create a new User if this one doesn't already exist
                        User fu = new User { AspNetUserId = identityID, Name = u.Name};
                        if(!context.Users.Any(x => x.AspNetUserId == fu.AspNetUserId))
                        {
                            // Doesn't already exist, so add a new user
                            context.Add(fu);
                            await context.SaveChangesAsync();
                        }
                    }
                }
            }
            catch(InvalidOperationException ex)
            {
                // Thrown if there is no service of the type requested from the service provider
                // Catch it (and don't throw the exception below) if you don't want it to fail (5xx status code)
                throw new Exception("Failed to initialize user seed data, service provider did not have the correct service");
            }
        }
        public static async Task InitializeAdmin(IServiceProvider serviceProvider, string userName, string adminPw, string id)
        {
            try
            {
                using (var context = new ProjectMaintenanceDbContext(serviceProvider.GetRequiredService<DbContextOptions<ProjectMaintenanceDbContext>>()))
                {
                    // Get the Identity user manager
                    var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
                    if(userManager.Users.Count() == 0)
                    {
                        // Ensure the admin user exists
                        var identityID = await EnsureUser(userManager, adminPw, userName, id);
                        // Create a new User if this one doesn't already exist
                        User fu = new User { AspNetUserId = identityID, Name = userName, Role = "Admin" };
                        if (!context.Users.Any(x => x.AspNetUserId == fu.AspNetUserId && x.Name == fu.Name && x.Role == fu.Role))
                        {
                            // Doesn't already exist, so add a new user
                            context.Add(fu);
                            await context.SaveChangesAsync();
                        }
                        // Now make sure admin role exists and give it to this user
                        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                        await EnsureRoleForUser(roleManager, userManager, identityID, "admin");
                    }
                }
            }
            catch (InvalidOperationException ex)
            {
                // Thrown if there is no service of the type requested from the service provider
                // Catch it (and don't throw the exception below) if you don't want it to fail (5xx status code)
                throw new Exception("Failed to initialize admin user or role, service provider did not have the correct service:" + ex.Message);
            }
        }

        /// <summary>
        /// Helper method to ensure that the Identity user exists or has been newly created.  Modified from
        /// <a href="https://docs.microsoft.com/en-us/aspnet/core/security/authorization/secure-data?view=aspnetcore-5.0#create-the-test-accounts-and-update-the-contacts">create the test accounts and update the contacts (in Contoso University example)</a>
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="password"></param>
        /// <param name="username"></param>
        /// <param name="email"></param>
        /// <param name="emailConfirmed"></param>
        /// <returns>The Identity ID of the user</returns>
        private static async Task<string> EnsureUser(UserManager<IdentityUser> userManager, string password, string name, string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                user = new IdentityUser
                {
                    UserName = name,
                    Id = id
                };
                var result = await userManager.CreateAsync(user, password);
                if (!result.Succeeded)
                {
                    throw new Exception("Failed to create identity user");
                }
            }
            else if (user != null)
            {
                throw new Exception($"User with name {name} already exists!");
            }
            return user.Id;
        }


        /// <summary>
        /// Ensure the given role exists and that the AspNetUser with the given id has been awarded that role.
        /// </summary>
        /// <param name="roleManager">The Identity role manager</param>
        /// <param name="userManager">The Identity user manager</param>
        /// <param name="uid">The AspNetUser id</param>
        /// <param name="role">The role to ensure and give to the user</param>
        /// <returns></returns>
        private static async Task<IdentityResult> EnsureRoleForUser(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager, string uid, string role)
        {
            IdentityResult iR = null;

            if (!await roleManager.RoleExistsAsync(role))
            {
                iR = await roleManager.CreateAsync(new IdentityRole(role));
            }

            var user = await userManager.FindByIdAsync(uid);
            if (user == null)
            {
                // remember to not throw exceptions in production code without also catching them
                throw new Exception("An AspNetUser does not exist with the given id so we cannot give them the requested role");
            }

            if (!await userManager.IsInRoleAsync(user, role))
            {
                iR = await userManager.AddToRoleAsync(user, role);
            }
            
            return iR;
        }
    }
}