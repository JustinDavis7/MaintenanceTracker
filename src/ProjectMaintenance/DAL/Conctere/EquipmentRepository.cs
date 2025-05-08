using ProjectMaintenance.Models;
using ProjectMaintenance.DAL.Abstract;
using Microsoft.EntityFrameworkCore;

namespace ProjectMaintenance.DAL.Concrete;

public class EquipmentRepository : Repository<Equipment>, IEquipmentRepository
{
    private readonly ProjectMaintenanceDbContext _ctx;
    private readonly IMaintenanceTicketRepository _maintenanceTicketRepository;

    public EquipmentRepository(ProjectMaintenanceDbContext ctx, IMaintenanceTicketRepository maintenanceTicketRepository) : base(ctx)
    {
        _maintenanceTicketRepository = maintenanceTicketRepository;
        _ctx = ctx;
    }
    public async Task<Equipment> GetEquipmentById(int equipementId)
    {
        var equipment = await _ctx.Equipment.FirstOrDefaultAsync<Equipment>(x => x.Id == equipementId);
        return equipment;
    }
    public async Task CreateEquipment(string name, string description, string? leadOperator, 
                                string? vendor, string? model, string? serialNumber, 
                                DateOnly? acquiredDate, DateOnly? warrantyExpiration)
    {
        Equipment equipment = new Equipment();
        equipment.Name = name;
        equipment.Description = description;
        equipment.LeadOperator = leadOperator;
        equipment.Vendor = vendor;
        equipment.Model = model;
        equipment.SerialNumber = serialNumber;
        equipment.AcquiredDate = acquiredDate;
        equipment.WarrantyExpiration = warrantyExpiration;
        _ctx.Equipment.Add(equipment);
        await _ctx.SaveChangesAsync();
    }

    public async Task DeleteEquipmentById(int equipmentId)
    {
        var equipmentToRemove = await _ctx.Equipment.FindAsync(equipmentId);
        var ticketsForEquipment = await _maintenanceTicketRepository.GetAllMaintenanceTicketsForEquipment(equipmentId);

        if (ticketsForEquipment.Count > 0)
        {
            foreach (var ticket in ticketsForEquipment)
            {
                await _maintenanceTicketRepository.DeleteTicket(ticket.Id);
            }
        }
        if (equipmentToRemove != null)
        {
            _ctx.Equipment.Remove(equipmentToRemove);
            await _ctx.SaveChangesAsync();
        }
    }

    public async Task<List<Equipment>> GetAllEquipment()
    {
        return await _ctx.Equipment.ToListAsync();
    } 
}