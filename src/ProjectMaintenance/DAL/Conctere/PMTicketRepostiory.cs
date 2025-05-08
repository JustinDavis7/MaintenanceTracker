using ProjectMaintenance.Models;
using ProjectMaintenance.DAL.Abstract;
using Microsoft.EntityFrameworkCore;

namespace ProjectMaintenance.DAL.Concrete;

public class PMTicketRepository : Repository<PMTicket>, IPMTicketRepository
{
    private readonly ProjectMaintenanceDbContext _ctx;

    public PMTicketRepository(ProjectMaintenanceDbContext ctx) : base(ctx)
    {
        _ctx = ctx;
    }

    public async Task CompletePMTicket(PMTicket pMTicket)
    {
        if (pMTicket != null)
        {
            // Update the entity's properties
            pMTicket.DatePerformed = DateOnly.FromDateTime(DateTime.Now).AddDays(30);

            // Mark the entity as modified
            _ctx.PMTickets.Update(pMTicket);

            // Save changes to the database
            await _ctx.SaveChangesAsync();
        }
        else
        {
            // Handle the case where the ticket was not found
            throw new Exception($"PMTicket not found.");
        }
    }

    public async Task CreatePMTicket(int equipmentId, DateOnly datePerformed, string title)
    {
        PMTicket pMTicket = new PMTicket();
        pMTicket.EquipmentId = equipmentId;
        pMTicket.DatePerformed = datePerformed;
        pMTicket.Title = title;

        _ctx.PMTickets.Add(pMTicket);
        await _ctx.SaveChangesAsync();
    }

    public async Task DeletePMTicket(int ticketId)
    {
        var ticketToDelete = await _ctx.PMTickets.FindAsync(ticketId);
        _ctx.PMTickets.Remove(ticketToDelete);
        await _ctx.SaveChangesAsync();
    }
    
    public async Task<PMTicket> GetPMTicket(int ticketId)
    {
        return await _ctx.PMTickets
                        .Include(p => p.Equipment)
                        .FirstOrDefaultAsync(x => x.Id == ticketId);
    }

    public async Task<List<PMTicket>> GetAllPMTickets()
    {
        return await _ctx.PMTickets
                        .Include(p => p.Equipment)
                        .ToListAsync();
    }

    public async Task Update(PMTicket pMTicket)
    {
        if (pMTicket == null)
        {
            throw new ArgumentNullException(nameof(pMTicket));
        }

        _ctx.PMTickets.Update(pMTicket);
        await _ctx.SaveChangesAsync();
    }
}
