using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProjectMaintenance.Models;

public partial class Equipment
{
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    public string Name { get; set; } = null!;

    [StringLength(512)]
    public string? Description { get; set; }

    [StringLength(50)]
    public string? LeadOperator { get; set; }

    [StringLength(50)]
    public string? Vendor { get; set; }

    [StringLength(255)]
    public string? Model { get; set; }

    [StringLength(255)]
    public string? SerialNumber { get; set; }

    public DateOnly? AcquiredDate { get; set; }

    public DateOnly? WarrantyExpiration { get; set; }

    [InverseProperty("Equipment")]
    public virtual ICollection<MaintenanceTicket> MaintenanceTickets { get; set; } = new List<MaintenanceTicket>();

    [InverseProperty("Equipment")]
    public virtual ICollection<PMTicket> PMTickets { get; set; } = new List<PMTicket>();
}
