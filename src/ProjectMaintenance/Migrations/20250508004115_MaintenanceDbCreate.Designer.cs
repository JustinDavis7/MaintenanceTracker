﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ProjectMaintenance.Models;

#nullable disable

namespace ProjectMaintenance.Migrations
{
    [DbContext(typeof(ProjectMaintenanceDbContext))]
    [Migration("20250508004115_MaintenanceDbCreate")]
    partial class MaintenanceDbCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ProjectMaintenance.Models.Equipment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateOnly?>("AcquiredDate")
                        .HasColumnType("date");

                    b.Property<string>("Description")
                        .HasMaxLength(512)
                        .HasColumnType("nvarchar(512)");

                    b.Property<string>("LeadOperator")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Model")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("SerialNumber")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Vendor")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateOnly?>("WarrantyExpiration")
                        .HasColumnType("date");

                    b.HasKey("Id")
                        .HasName("PK__Equipmen__3214EC0707EB0615");

                    b.ToTable("Equipment");
                });

            modelBuilder.Entity("ProjectMaintenance.Models.MaintenanceTicket", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("Archived")
                        .HasColumnType("bit");

                    b.Property<string>("AssignedWorker")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Closed")
                        .HasColumnType("bit");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(512)
                        .HasColumnType("nvarchar(512)");

                    b.Property<int>("EquipmentId")
                        .HasColumnType("int");

                    b.Property<string>("MaintenanceType")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("PartsList")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateOnly>("PlannedCompletion")
                        .HasColumnType("date");

                    b.Property<bool>("PriorityBump")
                        .HasColumnType("bit");

                    b.Property<int>("PriorityLevel")
                        .HasColumnType("int");

                    b.Property<DateOnly>("RequestCreationDate")
                        .HasColumnType("date");

                    b.Property<bool>("Satisfied")
                        .HasColumnType("bit");

                    b.Property<int>("TicketCreatorId")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id")
                        .HasName("PK__Maintena__3214EC07CCF9430B");

                    b.HasIndex("EquipmentId");

                    b.HasIndex("TicketCreatorId");

                    b.ToTable("MaintenanceTicket");
                });

            modelBuilder.Entity("ProjectMaintenance.Models.PMTicket", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateOnly>("DatePerformed")
                        .HasColumnType("date");

                    b.Property<int>("EquipmentId")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .HasMaxLength(512)
                        .HasColumnType("nvarchar(512)");

                    b.HasKey("Id")
                        .HasName("PK__PMTicket__3214EC07898DA921");

                    b.HasIndex("EquipmentId");

                    b.ToTable("PMTicket");
                });

            modelBuilder.Entity("ProjectMaintenance.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AspNetUserId")
                        .IsRequired()
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id")
                        .HasName("PK__User__3214EC07D49ADB05");

                    b.ToTable("User");
                });

            modelBuilder.Entity("ProjectMaintenance.Models.MaintenanceTicket", b =>
                {
                    b.HasOne("ProjectMaintenance.Models.Equipment", "Equipment")
                        .WithMany("MaintenanceTickets")
                        .HasForeignKey("EquipmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_MaintenanceTicket_Equpiment");

                    b.HasOne("ProjectMaintenance.Models.User", "TicketCreatorNavigation")
                        .WithMany("MaintenanceTickets")
                        .HasForeignKey("TicketCreatorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_MaintenanceTicket_User");

                    b.Navigation("Equipment");

                    b.Navigation("TicketCreatorNavigation");
                });

            modelBuilder.Entity("ProjectMaintenance.Models.PMTicket", b =>
                {
                    b.HasOne("ProjectMaintenance.Models.Equipment", "Equipment")
                        .WithMany("PMTickets")
                        .HasForeignKey("EquipmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_PMTicket_Equpiment");

                    b.Navigation("Equipment");
                });

            modelBuilder.Entity("ProjectMaintenance.Models.Equipment", b =>
                {
                    b.Navigation("MaintenanceTickets");

                    b.Navigation("PMTickets");
                });

            modelBuilder.Entity("ProjectMaintenance.Models.User", b =>
                {
                    b.Navigation("MaintenanceTickets");
                });
#pragma warning restore 612, 618
        }
    }
}
