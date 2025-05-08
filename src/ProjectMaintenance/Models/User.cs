using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProjectMaintenance.Models;

[Table("User")]
public partial class User
{
    [Key]
    public int Id { get; set; }

    [StringLength(450)]
    public string AspNetUserId { get; set; } = null!;

    [StringLength(50)]
    public string Name { get; set; } = null!;

    [StringLength(50)]
    public string Role { get; set;} = "maintenance"; // Defaulted to maintenance, but can be other roles.

    [InverseProperty("TicketCreatorNavigation")]
    public virtual ICollection<MaintenanceTicket> MaintenanceTickets { get; set; } = new List<MaintenanceTicket>();
}
