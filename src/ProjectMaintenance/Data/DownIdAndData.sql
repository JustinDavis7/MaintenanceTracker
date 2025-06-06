USE ProjectMaintenance;

ALTER TABLE [MaintenanceTicket] DROP CONSTRAINT [FK_MaintenanceTicket_User];
ALTER TABLE [MaintenanceTicket] DROP CONSTRAINT [FK_MaintenanceTicket_Equpiment];
ALTER TABLE [PMTicket]          DROP CONSTRAINT [FK_PMTicket_Equpiment];

DROP TABLE [MaintenanceTicket];
DROP TABLE [Equipment];
DROP TABLE [User];
DROP TABLE [PMTicket];

--Transition to the second DB we need to delete

USE [ProjectMaintenanceAuth];

ALTER TABLE [AspNetRoleClaims] DROP CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId];
ALTER TABLE [AspNetUserClaims] DROP CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId];
ALTER TABLE [AspNetUserLogins] DROP CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId];
ALTER TABLE [AspNetUserRoles]  DROP CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId];
ALTER TABLE [AspNetUserRoles]  DROP CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId];
ALTER TABLE [AspNetUserTokens] DROP CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId];

DROP TABLE [__EFMigrationsHistory];
DROP TABLE [AspNetRoles];
DROP TABLE [AspNetUsers];
DROP TABLE [AspNetRoleClaims];
DROP TABLE [AspNetUserClaims];
DROP TABLE [AspNetUserLogins];
DROP TABLE [AspNetUserRoles];
DROP TABLE [AspNetUserTokens];