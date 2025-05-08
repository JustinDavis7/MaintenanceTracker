using Microsoft.EntityFrameworkCore.Storage;
using ProjectMaintenance.Models;

namespace ProjectMaintenance.DAL.Abstract
{
    public interface IUserRepository : IRepository<User>
    {
        /// <summary>
        /// Get information about the current user.
        /// </summary>
        /// <param name="userId">The identity ID of the current user.</param>
        /// <returns>A new User with all of the info in the User table about them</returns>
        Task <User> GetUserById(string userId);

        Task <User> GetUserById(int id);

        /// <summary>
        /// Get a list of all users on the site -- used solely for admin access right now.
        /// <summary>
        /// <returns>A list of all the users on the site.
        Task<IEnumerable<User>> GetAllUsers();
        Task AddOrUpdate(User user);
        Task DeleteUser(User user);
        Task<IDbContextTransaction> BeginTransactionAsync();
        Task<User> GetUserByName(string? name);
    }
}