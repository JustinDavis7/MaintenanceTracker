using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProjectMaintenance.Models;

[Table("PMTicket")]
public partial class PMTicket
{
    [Key]
    public int Id { get; set; }

    public int EquipmentId { get; set; }

    public DateOnly DatePerformed { get; set; }

    [StringLength(512)]
    public string? Title { get; set; }

    [ForeignKey("EquipmentId")]
    [InverseProperty("PMTickets")]
    public virtual Equipment? Equipment { get; set; }
}
