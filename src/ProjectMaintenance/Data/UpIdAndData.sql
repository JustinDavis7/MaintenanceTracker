USE ProjectMaintenance;
GO

CREATE TABLE [User]
(
	[Id]				   INT			    NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[AspNetUserId]		   NVARCHAR(450)	NOT NULL,
    [Role]                 NVARCHAR(50)     NOT NULL,
    [Name]                 NVARCHAR(50)     NOT NULL
);
GO

CREATE TABLE [Equipment]
(
    [Id]                    INT             NOT NULL IDENTITY(1,1) PRIMARY KEY,
    [Name]                 NVARCHAR(50)     NOT NULL,
    [Description]          NVARCHAR(512),
    [LeadOperator]         NVARCHAR(50),
    [Vendor]               NVARCHAR(50),
    [Model]                NVARCHAR(255),
    [SerialNumber]         NVARCHAR(255),
    [AcquiredDate]         DATE,
    [WarrantyExpiration]   DATE
);
GO

CREATE TABLE [MaintenanceTicket]
(
    [Id]                   INT              NOT NULL IDENTITY(1,1) PRIMARY KEY,
    [EquipmentId]          INT,
    [TicketCreatorId]      INT,
    [Title]                NVARCHAR(50),
    [Description]          NVARCHAR(512),
    [MaintenanceType]      NVARCHAR(20) CHECK (MaintenanceType IN ('Corrective', 'Prevention')),
    [PriorityLevel]        INT CHECK   (PriorityLevel >= 1 AND PriorityLevel <=5),
    [RequestCreationDate]  DATE,
    [Satisfied]            BIT,
    [Closed]               BIT,
    [Archived]             BIT,
    [PlannedCompletion]    DATE,
    [PartsList]            NVARCHAR(MAX),
    [PriorityBump]         BIT,
    [AssignedWorker]       NVARCHAR(50)
);
GO

CREATE TABLE [PMTicket]
(
    [Id]                   INT              NOT NULL IDENTITY(1,1) PRIMARY KEY,
    [EquipmentId]          INT,
    [DatePerformed]        DATE,
    [Title]                NVARCHAR(512)
);
GO

ALTER TABLE [MaintenanceTicket] ADD CONSTRAINT [FK_MaintenanceTicket_Equpiment] FOREIGN KEY ([EquipmentId]) REFERENCES [Equipment] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

ALTER TABLE [MaintenanceTicket] ADD CONSTRAINT [FK_MaintenanceTicket_User] FOREIGN KEY ([TicketCreatorId]) REFERENCES [User] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

ALTER TABLE [PMTicket] ADD CONSTRAINT [FK_PMTicket_Equpiment] FOREIGN KEY ([EquipmentId]) REFERENCES [Equipment] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

--Transition to the second DB we need to create

USE ProjectMaintenanceAuth;
GO

IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [AspNetRoles] (
    [Id] nvarchar(450) NOT NULL,
    [Name] nvarchar(256) NULL,
    [NormalizedName] nvarchar(256) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetRoles] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [AspNetUsers] (
    [Id] nvarchar(450) NOT NULL,
    [UserName] nvarchar(256) NULL,
    [NormalizedUserName] nvarchar(256) NULL,
    [Email] nvarchar(256) NULL,
    [NormalizedEmail] nvarchar(256) NULL,
    [EmailConfirmed] bit NOT NULL,
    [PasswordHash] nvarchar(max) NULL,
    [SecurityStamp] nvarchar(max) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    [PhoneNumber] nvarchar(max) NULL,
    [PhoneNumberConfirmed] bit NOT NULL,
    [TwoFactorEnabled] bit NOT NULL,
    [LockoutEnd] datetimeoffset NULL,
    [LockoutEnabled] bit NOT NULL,
    [AccessFailedCount] int NOT NULL,
    CONSTRAINT [PK_AspNetUsers] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [AspNetRoleClaims] (
    [Id] int NOT NULL IDENTITY,
    [RoleId] nvarchar(450) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [AspNetUserClaims] (
    [Id] int NOT NULL IDENTITY,
    [UserId] nvarchar(450) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [AspNetUserLogins] (
    [LoginProvider] nvarchar(128) NOT NULL,
    [ProviderKey] nvarchar(128) NOT NULL,
    [ProviderDisplayName] nvarchar(max) NULL,
    [UserId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
    CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [AspNetUserRoles] (
    [UserId] nvarchar(450) NOT NULL,
    [RoleId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
    CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [AspNetUserTokens] (
    [UserId] nvarchar(450) NOT NULL,
    [LoginProvider] nvarchar(128) NOT NULL,
    [Name] nvarchar(128) NOT NULL,
    [Value] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
    CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_AspNetRoleClaims_RoleId] ON [AspNetRoleClaims] ([RoleId]);
GO

CREATE UNIQUE INDEX [RoleNameIndex] ON [AspNetRoles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL;
GO

CREATE INDEX [IX_AspNetUserClaims_UserId] ON [AspNetUserClaims] ([UserId]);
GO

CREATE INDEX [IX_AspNetUserLogins_UserId] ON [AspNetUserLogins] ([UserId]);
GO

CREATE INDEX [IX_AspNetUserRoles_RoleId] ON [AspNetUserRoles] ([RoleId]);
GO

CREATE INDEX [EmailIndex] ON [AspNetUsers] ([NormalizedEmail]);
GO

CREATE UNIQUE INDEX [UserNameIndex] ON [AspNetUsers] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'00000000000000_CreateIdentitySchema', N'8.0.6');
GO

COMMIT;
GO
