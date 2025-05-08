using ProjectMaintenance.Models;
using ProjectMaintenance.DAL.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;

namespace ProjectMaintenance.DAL.Concrete;

public class UserRepository : Repository<User>, IUserRepository
{
    private readonly ProjectMaintenanceDbContext _ctx;
    private readonly UserManager<IdentityUser> _userManager;

    public UserRepository(ProjectMaintenanceDbContext ctx, UserManager<IdentityUser> userManager) : base(ctx)
    {
        _ctx = ctx;
        _userManager = userManager;
    }

    public async Task<IEnumerable<User>> GetAllUsers()
    {
        return await _ctx.Users.ToListAsync();
    }

    public async Task<User> GetUserById(string userAspId)
    {
        var user = await _ctx.Users.FirstOrDefaultAsync(x => x.AspNetUserId == userAspId);
        return user;
    }

    public async Task<User> GetUserById(int userId)
    {
        var user = await _ctx.Users.FirstOrDefaultAsync(x => x.Id == userId);
        return user;
    }

    public async Task AddOrUpdate(User user)
    {
        if (user.Id == 0)
        {
            // This is a new user, so add it
            _ctx.Users.Add(user);
        }
        else
        {
            // This is an existing user, so update it
            _ctx.Users.Update(user);
        }

        await _ctx.SaveChangesAsync();
    }

    public async Task DeleteUser(User user)
    {
        _ctx.Users.Remove(user);
        await _ctx.SaveChangesAsync();

        var identityUser = await _userManager.FindByIdAsync(user.AspNetUserId);
        if(identityUser != null)
        {
            await _userManager.DeleteAsync(identityUser);
        }
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        return await _ctx.Database.BeginTransactionAsync();
    }

    public async Task<User> GetUserByName(string name)
    {
        var user = await _ctx.Users.FirstOrDefaultAsync(x => x.Name == name);
        return user;
    }
}