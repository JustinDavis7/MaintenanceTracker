using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ProjectMaintenance.Models;

public partial class ProjectMaintenanceDbContext : DbContext
{
    public ProjectMaintenanceDbContext()
    {
    }

    public ProjectMaintenanceDbContext(DbContextOptions<ProjectMaintenanceDbContext> options)
        : base(options)
    {
    }
    public virtual DbSet<Equipment> Equipment { get; set; }

    public virtual DbSet<MaintenanceTicket> MaintenanceTickets { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<PMTicket> PMTickets { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ProjectMaintenanceConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Equipment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Equipmen__3214EC0707EB0615");
        });

        modelBuilder.Entity<MaintenanceTicket>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Maintena__3214EC07CCF9430B");

            entity.HasOne(d => d.Equipment).WithMany(p => p.MaintenanceTickets).HasConstraintName("FK_MaintenanceTicket_Equpiment");

            entity.HasOne(d => d.TicketCreatorNavigation).WithMany(p => p.MaintenanceTickets).HasConstraintName("FK_MaintenanceTicket_User");
        });

        modelBuilder.Entity<PMTicket>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PMTicket__3214EC07898DA921");

            entity.HasOne(d => d.Equipment).WithMany(p => p.PMTickets).HasConstraintName("FK_PMTicket_Equpiment");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__User__3214EC07D49ADB05");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
