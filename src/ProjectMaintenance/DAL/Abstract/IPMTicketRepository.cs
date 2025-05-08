using ProjectMaintenance.Models;

namespace ProjectMaintenance.DAL.Abstract
{
    public interface IPMTicketRepository : IRepository<PMTicket>
    {
        public Task CreatePMTicket(int equipmentId, DateOnly datePerformed, string title);
        public Task CompletePMTicket(PMTicket pMTicket);
        Task<PMTicket> GetPMTicket(int ticketId);
        Task<List<PMTicket>> GetAllPMTickets();
        Task DeletePMTicket(int ticketId);
        Task Update(PMTicket pMTicket);
    }
}