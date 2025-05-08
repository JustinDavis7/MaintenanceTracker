using ProjectMaintenance.Models;

namespace ProjectMaintenance.Utilities
{
    public class EquipmentSeedData
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string LeadOperator { get; set; }
        public string Vendor { get; set; }
        public string Model { get; set; }
        public string SerialNumber { get; set; }
        public DateOnly AcquiredDate { get; set; }
        public DateOnly WarrantyExpiration { get; set; }
    }

    public class TicketSeedData
    {
        public int EquipmentId { get; set; }
        public int TicketCreatorId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string MaintenanceType { get; set; }
        public int PriorityLevel { get; set; }
        public DateOnly RequestCreationDate { get; set; }
        public bool Satisfied { get; set; }
        public bool Closed { get; set; }
        public DateOnly PlannedCompletion { get; set; }
        public string PartsList { get; set; }
        public bool PriorityBump { get; set; }
    }

    public static class SeedETData
    {
        public static void Seed(ProjectMaintenanceDbContext dbContext)
        {
            // Equipment seed data
            var equipmentData = new List<Equipment>
            {
                new Equipment { Name = "Equipment1", Description = "Description1", LeadOperator = "LeadOperator1", Vendor = "Vendor1", 
                                Model = "Model1", SerialNumber = "SerialNumber1", AcquiredDate = DateOnly.FromDateTime(new DateTime(2024, 1, 1)), 
                                WarrantyExpiration = DateOnly.FromDateTime(new DateTime(2025, 1, 1)) },
                new Equipment { Name = "Equipment2", Description = "Description2", LeadOperator = "LeadOperator2", Vendor = "Vendor2", 
                                Model = "Model2", SerialNumber = "SerialNumber2", AcquiredDate = DateOnly.FromDateTime(new DateTime(2024, 2, 1)), 
                                WarrantyExpiration = DateOnly.FromDateTime(new DateTime(2025, 2, 1)) },
                new Equipment { Name = "Equipment3", Description = "Description3", LeadOperator = "LeadOperator3", Vendor = "Vendor3", 
                                Model = "Model3", SerialNumber = "SerialNumber3", AcquiredDate = DateOnly.FromDateTime(new DateTime(2024, 3, 1)), 
                                WarrantyExpiration = DateOnly.FromDateTime(new DateTime(2025, 3, 1)) }
            };

            foreach (var equipment in equipmentData)
            {
                if (!dbContext.Equipment.Any(e => e.Name == equipment.Name))
                {
                    dbContext.Equipment.Add(new Equipment
                    {
                        Name = equipment.Name,
                        Description = equipment.Description,
                        LeadOperator = equipment.LeadOperator,
                        Vendor = equipment.Vendor,
                        Model = equipment.Model,
                        SerialNumber = equipment.SerialNumber,
                        AcquiredDate = equipment.AcquiredDate,
                        WarrantyExpiration = equipment.WarrantyExpiration
                    });
                }
            }

            dbContext.SaveChanges();

            // MaintenanceTicket seed data
            var ticketData = new List<MaintenanceTicket>
            {
                new MaintenanceTicket { EquipmentId = 1, TicketCreatorId = 1, Title = "Ticket1", Description = "Description1", 
                                        MaintenanceType = "Prevention", PriorityLevel = 1, RequestCreationDate = new DateOnly(2024, 1, 1), 
                                        Satisfied = false, Closed = false, Archived = false, PlannedCompletion = new DateOnly(2024, 2, 1), 
                                        PartsList = "PartsList1", PriorityBump = false, AssignedWorker = "Tim" },
                                        
                new MaintenanceTicket { EquipmentId = 1, TicketCreatorId = 2, Title = "Ticket2", Description = "Description2", 
                                        MaintenanceType = "Prevention", PriorityLevel = 2, RequestCreationDate = new DateOnly(2024, 2, 1), 
                                        Satisfied = false, Closed = false, Archived = false, PlannedCompletion = new DateOnly(2024, 3, 1), 
                                        PartsList = "PartsList2", PriorityBump = false, AssignedWorker = "Jim" },
                                        
                new MaintenanceTicket { EquipmentId = 2, TicketCreatorId = 3, Title = "Ticket3", Description = "Description3", 
                                        MaintenanceType = "Corrective", PriorityLevel = 3, RequestCreationDate = new DateOnly(2024, 3, 1), 
                                        Satisfied = true, Closed = true, Archived = false, PlannedCompletion = new DateOnly(2024, 4, 1), 
                                        PartsList = "PartsList3", PriorityBump = false, AssignedWorker = "Bim" },
                                        
                new MaintenanceTicket { EquipmentId = 2, TicketCreatorId = 4, Title = "Ticket4", Description = "Description4", 
                                        MaintenanceType = "Prevention", PriorityLevel = 4, RequestCreationDate = new DateOnly(2024, 4, 1), 
                                        Satisfied = true, Closed = false, Archived = false, PlannedCompletion = new DateOnly(2024, 5, 1), 
                                        PartsList = "PartsList4", PriorityBump = false, AssignedWorker = "Holland" },
                                        
                new MaintenanceTicket { EquipmentId = 3, TicketCreatorId = 1, Title = "Ticket5", Description = "Description5", 
                                        MaintenanceType = "Corrective", PriorityLevel = 3, RequestCreationDate = new DateOnly(2024, 5, 1), 
                                        Satisfied = false, Closed = false, Archived = false, PlannedCompletion = new DateOnly(2024, 6, 1), 
                                        PartsList = "PartsList5", PriorityBump = false, AssignedWorker = "Samuel" },
                                        
                new MaintenanceTicket { EquipmentId = 3, TicketCreatorId = 2, Title = "Ticket6", Description = "Description6", 
                                        MaintenanceType = "Corrective", PriorityLevel = 5, RequestCreationDate = new DateOnly(2024, 6, 1), 
                                        Satisfied = true, Closed = true, Archived = true, PlannedCompletion = new DateOnly(2024, 7, 1), 
                                        PartsList = "PartsList6", PriorityBump = false, AssignedWorker = "Tabitha" },

                new MaintenanceTicket { EquipmentId = 2, TicketCreatorId = 3, Title = "Ticket7", Description = "Description7", 
                                        MaintenanceType = "Corrective", PriorityLevel = 3, RequestCreationDate = new DateOnly(2024, 6, 1), 
                                        Satisfied = true, Closed = false, Archived = false, PlannedCompletion = new DateOnly(2024, 7, 1), 
                                        PartsList = "PartsList7", PriorityBump = false, AssignedWorker = "Joaquin" }
            };

            foreach (var ticket in ticketData)
            {
                if (!dbContext.MaintenanceTickets.Any(t => t.Title == ticket.Title && t.EquipmentId == ticket.EquipmentId))
                {
                    dbContext.MaintenanceTickets.Add(new MaintenanceTicket
                    {
                        EquipmentId = ticket.EquipmentId,
                        TicketCreatorId = ticket.TicketCreatorId,
                        Title = ticket.Title,
                        Description = ticket.Description,
                        MaintenanceType = ticket.MaintenanceType,
                        PriorityLevel = ticket.PriorityLevel,
                        RequestCreationDate = ticket.RequestCreationDate,
                        Satisfied = ticket.Satisfied,
                        Closed = ticket.Closed,
                        Archived = ticket.Archived,
                        PlannedCompletion = ticket.PlannedCompletion,
                        PartsList = ticket.PartsList,
                        PriorityBump = ticket.PriorityBump,
                        AssignedWorker = ticket.AssignedWorker
                    });
                }
            }

            dbContext.SaveChanges();
        }
    }
}