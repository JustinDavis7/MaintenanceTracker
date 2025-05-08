USE ProjectMaintenance;

ALTER TABLE [MaintenanceTicket] DROP CONSTRAINT [FK_MaintenanceTicket_User];
ALTER TABLE [MaintenanceTicket] DROP CONSTRAINT [FK_MaintenanceTicket_Equpiment];
ALTER TABLE [PMTicket]          DROP CONSTRAINT [FK_PMTicket_Equpiment];

DROP TABLE [MaintenanceTicket];
DROP TABLE [Equipment];
DROP TABLE [User];
DROP TABLE [PMTicket];