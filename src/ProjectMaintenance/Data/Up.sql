-- CREATE DATABASE ProjectMaintenance;

USE ProjectMaintenance;

CREATE TABLE [User]
(
	[Id]				    INT				NOT NULL IDENTITY(1,1) PRIMARY KEY
	,[AspNetUserId]		    NVARCHAR(450)	NOT NULL
    ,[Role]                 NVARCHAR(50)    NOT NULL
    ,[Name]                 NVARCHAR(50)    NOT NULL
);

CREATE TABLE [Equipment]
(
    [Id]                    INT             NOT NULL IDENTITY(1,1) PRIMARY KEY
    ,[Name]                 NVARCHAR(50)    NOT NULL
    ,[Description]          NVARCHAR(512)   
    ,[LeadOperator]         NVARCHAR(50)
    ,[Vendor]               NVARCHAR(50)
    ,[Model]                NVARCHAR(255)
    ,[SerialNumber]         NVARCHAR(255)
    ,[AcquiredDate]         DATE
    ,[WarrantyExpiration]   DATE
);

CREATE TABLE [MaintenanceTicket]
(
    [Id]                    INT             NOT NULL IDENTITY(1,1) PRIMARY KEY
    ,[EquipmentId]          INT
    ,[TicketCreatorId]        INT
    ,[Title]                NVARCHAR(50)
    ,[Description]          NVARCHAR(512)
    ,[MaintenanceType]      NVARCHAR(20) CHECK (MaintenanceType IN ('Corrective', 'Prevention'))
    ,[PriorityLevel]        INT CHECK   (PriorityLevel >= 1 AND PriorityLevel <=5)
    ,[RequestCreationDate]  DATE
    ,[Satisfied]            BIT
    ,[Closed]               BIT
    ,[Archived]             BIT
    ,[PlannedCompletion]    DATE
    ,[PartsList]            NVARCHAR(MAX)
    ,[PriorityBump]         BIT
    ,[AssignedWorker]       NVARCHAR(50)
);

CREATE TABLE [PMTicket]
(
    [Id]                    INT             NOT NULL IDENTITY(1,1) PRIMARY KEY
    ,[EquipmentId]          INT
    ,[DatePerformed]        DATE
    ,[Title]                NVARCHAR(512)
    ,[Repeat]               BIT
);

ALTER TABLE [MaintenanceTicket]     ADD CONSTRAINT [FK_MaintenanceTicket_Equpiment]     FOREIGN KEY ([EquipmentId])         REFERENCES [Equipment] ([Id])       ON DELETE NO ACTION ON UPDATE NO ACTION;
ALTER TABLE [MaintenanceTicket]     ADD CONSTRAINT [FK_MaintenanceTicket_User]          FOREIGN KEY ([TicketCreatorId])     REFERENCES [User] ([Id])            ON DELETE NO ACTION ON UPDATE NO ACTION;
ALTER TABLE [PMTicket]              ADD CONSTRAINT [FK_PMTicket_Equpiment]              FOREIGN KEY ([EquipmentId])         REFERENCES [Equipment] ([Id])       ON DELETE NO ACTION ON UPDATE NO ACTION;