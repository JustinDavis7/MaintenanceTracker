using ProjectMaintenance.Models;

namespace ProjectMaintenance.DAL.Abstract
{
    public interface IEquipmentRepository : IRepository<Equipment>
    {
        public Task CreateEquipment(string name, string description, string? leadOperator, 
                                    string? vendor, string? model, string? serialNumber, 
                                    DateOnly? acquiredDate, DateOnly? warrantyExpiration);
        Task <Equipment> GetEquipmentById(int id);

        Task<List<Equipment>> GetAllEquipment();

        Task DeleteEquipmentById(int equipmentId);
    }
}