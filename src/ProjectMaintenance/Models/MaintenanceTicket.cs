using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProjectMaintenance.Models;

[Table("MaintenanceTicket")]
public partial class MaintenanceTicket
{
    [Key]
    public int Id { get; set; }

    public int EquipmentId { get; set; }

    public int TicketCreatorId { get; set; }

    [StringLength(50)]
    public string Title { get; set; }

    [StringLength(512)]
    public string Description { get; set; }

    [StringLength(20)]
    public string MaintenanceType { get; set; }

    public int PriorityLevel { get; set; }

    public DateOnly RequestCreationDate { get; set; }

    public bool Satisfied { get; set; }

    public bool Closed { get; set; }

    public bool Archived { get; set; }

    public DateOnly PlannedCompletion { get; set; }

    public string PartsList { get; set; }

    public bool PriorityBump { get; set; }

    public string AssignedWorker { get; set; }

    [ForeignKey("EquipmentId")]
    [InverseProperty("MaintenanceTickets")]
    public virtual Equipment? Equipment { get; set; }

    [ForeignKey("TicketCreatorId")]
    [InverseProperty("MaintenanceTickets")]
    public virtual User? TicketCreatorNavigation { get; set; }
}
