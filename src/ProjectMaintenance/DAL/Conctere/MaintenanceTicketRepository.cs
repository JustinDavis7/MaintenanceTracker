using ProjectMaintenance.Models;
using ProjectMaintenance.DAL.Abstract;
using Microsoft.EntityFrameworkCore;

namespace ProjectMaintenance.DAL.Concrete;

public class MaintenanceTicketRepository : Repository<MaintenanceTicket>, IMaintenanceTicketRepository
{
    private readonly ProjectMaintenanceDbContext _ctx;
    public MaintenanceTicketRepository(ProjectMaintenanceDbContext ctx) : base(ctx)
    {
        _ctx = ctx;
    }
    public async Task<MaintenanceTicket> GetMaintenanceTicket(int ticketId)
    {
        var ticket = await _ctx.MaintenanceTickets.FirstOrDefaultAsync(x => x.Id == ticketId);
        return ticket;
    }

    public async Task<List<MaintenanceTicket>> GetAllMaintenanceTicketForUser(int userId)
    {
        var tickets = await _ctx.MaintenanceTickets.Where(x => x.TicketCreatorId == userId).ToListAsync();
        return tickets;
    }

    public async Task<List<MaintenanceTicket>> GetAllMaintenanceTicketsForEquipment(int equipmentId)
    {
        var tickets = await _ctx.MaintenanceTickets
                                .Where(x => x.EquipmentId == equipmentId)
                                .ToListAsync();
        return tickets;
    }

    public async Task ScrubDeletedUserTicket(int userId, int adminId)
    {
        var tickets = await GetAllMaintenanceTicketForUser(userId);
        foreach(var ticket in tickets)
        {
            ticket.TicketCreatorId = adminId;
        }
        await _ctx.SaveChangesAsync();
    }

    public async Task CreateMaintenanceTicket(int equipmentId, int ticketCreatorId, string title, string description, 
                                        string maintenanceType, int priorityLevel, DateOnly requestCreationDate, 
                                        DateOnly plannedCompletion, string partsList, bool priorityBump, string assignedWorker)
    {
        MaintenanceTicket maintenanceTicket = new MaintenanceTicket();
        maintenanceTicket.EquipmentId = equipmentId;
        maintenanceTicket.TicketCreatorId = ticketCreatorId;
        maintenanceTicket.Title = title;
        maintenanceTicket.Description = description;
        maintenanceTicket.PriorityLevel = priorityLevel;
        maintenanceTicket.MaintenanceType = maintenanceType;
        maintenanceTicket.RequestCreationDate = requestCreationDate;
        maintenanceTicket.PlannedCompletion = plannedCompletion;
        maintenanceTicket.PartsList = partsList;
        maintenanceTicket.PriorityBump = priorityBump;
        maintenanceTicket.Satisfied = false;
        maintenanceTicket.Closed = false;
        maintenanceTicket.Archived = false;
        maintenanceTicket.AssignedWorker = assignedWorker;
        
        _ctx.MaintenanceTickets.Add(maintenanceTicket);
        await _ctx.SaveChangesAsync();
    }

    public async Task ActivateTicket(int ticketId, int equipmentId)
    {
        var maintenanceTicket = _ctx.MaintenanceTickets.FirstOrDefault(x => x.Id == ticketId && x.EquipmentId == equipmentId);
        maintenanceTicket.Satisfied = false;
        maintenanceTicket.Closed = false;
        maintenanceTicket.Archived = false;
        await _ctx.SaveChangesAsync();
    }

    public async Task SatisfyTicket(int ticketId, int equipmentId)
    {
        var maintenanceTicket = _ctx.MaintenanceTickets.FirstOrDefault(x => x.Id == ticketId && x.EquipmentId == equipmentId);
        maintenanceTicket.Satisfied = true;
        maintenanceTicket.Closed = false;
        maintenanceTicket.Archived = false;
        await _ctx.SaveChangesAsync();
    }

    public async Task CloseTicket(int ticketId, int equipmentId)
    {
        var maintenanceTicket = _ctx.MaintenanceTickets.FirstOrDefault(x => x.Id == ticketId && x.EquipmentId == equipmentId);
        maintenanceTicket.Satisfied = true;
        maintenanceTicket.Closed = true;
        maintenanceTicket.Archived = false;
        await _ctx.SaveChangesAsync();
    }

    public async Task ArchiveTicket(int ticketId, int equipmentId)
    {
        var maintenanceTicket = _ctx.MaintenanceTickets.FirstOrDefault(x => x.Id == ticketId && x.EquipmentId == equipmentId);
        maintenanceTicket.Satisfied = true;
        maintenanceTicket.Closed = true;
        maintenanceTicket.Archived = true;
        await _ctx.SaveChangesAsync();
    }

    public async Task DeleteTicket(int ticketId)
    {
        var maintenanceTicket = await _ctx.MaintenanceTickets.FindAsync(ticketId);
        if (maintenanceTicket != null)
        {
            _ctx.MaintenanceTickets.Remove(maintenanceTicket);
            await _ctx.SaveChangesAsync();
        }
    }
}