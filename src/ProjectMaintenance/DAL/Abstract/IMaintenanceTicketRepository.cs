using ProjectMaintenance.Models;

namespace ProjectMaintenance.DAL.Abstract
{
    public interface IMaintenanceTicketRepository : IRepository<MaintenanceTicket>
    {
        public Task CreateMaintenanceTicket(int equipmentId, int ticketCreatorId, string title, string description, string maintenanceType, 
                                            int priorityLevel, DateOnly requestCreationDate, DateOnly plannedCompletion, string partsList, 
                                            bool priorityBump, string assignedWorker);
        public Task ActivateTicket(int ticketId, int equipmentId);
        public Task SatisfyTicket(int ticketId, int equipmentId);
        public Task CloseTicket(int ticketId, int equipmentId);
        public Task ArchiveTicket(int ticketId, int equipmentId);
        Task<MaintenanceTicket> GetMaintenanceTicket(int ticketId);
        Task<List<MaintenanceTicket>> GetAllMaintenanceTicketForUser(int userId);
        Task<List<MaintenanceTicket>> GetAllMaintenanceTicketsForEquipment(int equipmentId); 
        public Task ScrubDeletedUserTicket(int userId, int adminId);
        Task DeleteTicket(int ticketId);
    }
}