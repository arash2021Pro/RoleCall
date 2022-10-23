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

CREATE TABLE [Licenses] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NULL,
    [LastName] nvarchar(max) NULL,
    [PhoneNumber] nvarchar(11) NULL,
    [LegalCode] nvarchar(10) NULL,
    [ConstPhone] nvarchar(12) NULL,
    [CompanyName] nvarchar(max) NULL,
    [CompanyAddress] nvarchar(max) NULL,
    [SoftwareType] int NOT NULL,
    [IsSmsPanelActive] bit NOT NULL,
    [IsMobileVersionActive] bit NOT NULL,
    [ClientCount] int NOT NULL,
    [AppSerialCount] int NOT NULL,
    [LicenseCode] nvarchar(max) NULL,
    [CreationTime] nvarchar(max) NULL,
    [ModificationTime] nvarchar(max) NULL,
    [IsDeleted] bit NOT NULL,
    CONSTRAINT [PK_Licenses] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Users] (
    [Id] int NOT NULL IDENTITY,
    [PhoneNumber] nvarchar(11) NULL,
    [Password] nvarchar(max) NULL,
    [CreationTime] nvarchar(max) NULL,
    [ModificationTime] nvarchar(max) NULL,
    [IsDeleted] bit NOT NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Clients] (
    [Id] int NOT NULL IDENTITY,
    [AppSerial] nvarchar(8) NULL,
    [SystemSerial] nvarchar(max) NULL,
    [LicenseId] int NOT NULL,
    [CreationTime] nvarchar(max) NULL,
    [ModificationTime] nvarchar(max) NULL,
    [IsDeleted] bit NOT NULL,
    CONSTRAINT [PK_Clients] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Clients_Licenses_LicenseId] FOREIGN KEY ([LicenseId]) REFERENCES [Licenses] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [Otps] (
    [Id] int NOT NULL IDENTITY,
    [LicenseId] int NOT NULL,
    [Code] nvarchar(6) NULL,
    [IsUsed] bit NOT NULL,
    [ExpireTime] datetimeoffset NOT NULL,
    [CreationTime] nvarchar(max) NULL,
    [ModificationTime] nvarchar(max) NULL,
    [IsDeleted] bit NOT NULL,
    CONSTRAINT [PK_Otps] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Otps_Licenses_LicenseId] FOREIGN KEY ([LicenseId]) REFERENCES [Licenses] ([Id]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_Clients_LicenseId] ON [Clients] ([LicenseId]);
GO

CREATE INDEX [IX_Otps_LicenseId] ON [Otps] ([LicenseId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20221012154808_init', N'6.0.9');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [Licenses] ADD [IsActive] bit NOT NULL DEFAULT CAST(0 AS bit);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20221013132539_IsActive', N'6.0.9');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Users]') AND [c].[name] = N'PhoneNumber');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Users] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [Users] DROP COLUMN [PhoneNumber];
GO

ALTER TABLE [Users] ADD [UserName] nvarchar(max) NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20221013142258_UserName', N'6.0.9');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [Licenses] ADD [Expiration] datetime2 NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20221015092723_Expiration', N'6.0.9');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [Licenses] ADD [IsOtpConfirmed] bit NOT NULL DEFAULT CAST(0 AS bit);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20221016135815_IsOtpConfirmed', N'6.0.9');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Users]') AND [c].[name] = N'ModificationTime');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [Users] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [Users] ALTER COLUMN [ModificationTime] nvarchar(max) NULL;
GO

DECLARE @var2 sysname;
SELECT @var2 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Users]') AND [c].[name] = N'CreationTime');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [Users] DROP CONSTRAINT [' + @var2 + '];');
ALTER TABLE [Users] ALTER COLUMN [CreationTime] nvarchar(max) NULL;
GO

ALTER TABLE [Users] ADD [DateTimeCreation] datetime2 NOT NULL DEFAULT '0001-01-01T00:00:00.0000000';
GO

DECLARE @var3 sysname;
SELECT @var3 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Otps]') AND [c].[name] = N'ModificationTime');
IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [Otps] DROP CONSTRAINT [' + @var3 + '];');
ALTER TABLE [Otps] ALTER COLUMN [ModificationTime] nvarchar(max) NULL;
GO

DECLARE @var4 sysname;
SELECT @var4 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Otps]') AND [c].[name] = N'CreationTime');
IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [Otps] DROP CONSTRAINT [' + @var4 + '];');
ALTER TABLE [Otps] ALTER COLUMN [CreationTime] nvarchar(max) NULL;
GO

ALTER TABLE [Otps] ADD [DateTimeCreation] datetime2 NOT NULL DEFAULT '0001-01-01T00:00:00.0000000';
GO

DECLARE @var5 sysname;
SELECT @var5 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Licenses]') AND [c].[name] = N'ModificationTime');
IF @var5 IS NOT NULL EXEC(N'ALTER TABLE [Licenses] DROP CONSTRAINT [' + @var5 + '];');
ALTER TABLE [Licenses] ALTER COLUMN [ModificationTime] nvarchar(max) NULL;
GO

DECLARE @var6 sysname;
SELECT @var6 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Licenses]') AND [c].[name] = N'CreationTime');
IF @var6 IS NOT NULL EXEC(N'ALTER TABLE [Licenses] DROP CONSTRAINT [' + @var6 + '];');
ALTER TABLE [Licenses] ALTER COLUMN [CreationTime] nvarchar(max) NULL;
GO

ALTER TABLE [Licenses] ADD [DateTimeCreation] datetime2 NOT NULL DEFAULT '0001-01-01T00:00:00.0000000';
GO

DECLARE @var7 sysname;
SELECT @var7 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Clients]') AND [c].[name] = N'ModificationTime');
IF @var7 IS NOT NULL EXEC(N'ALTER TABLE [Clients] DROP CONSTRAINT [' + @var7 + '];');
ALTER TABLE [Clients] ALTER COLUMN [ModificationTime] nvarchar(max) NULL;
GO

DECLARE @var8 sysname;
SELECT @var8 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Clients]') AND [c].[name] = N'CreationTime');
IF @var8 IS NOT NULL EXEC(N'ALTER TABLE [Clients] DROP CONSTRAINT [' + @var8 + '];');
ALTER TABLE [Clients] ALTER COLUMN [CreationTime] nvarchar(max) NULL;
GO

ALTER TABLE [Clients] ADD [DateTimeCreation] datetime2 NOT NULL DEFAULT '0001-01-01T00:00:00.0000000';
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20221016163943_DateTimeCreation', N'6.0.9');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [Users] ADD [DateTimeModification] datetime2 NOT NULL DEFAULT '0001-01-01T00:00:00.0000000';
GO

ALTER TABLE [Otps] ADD [DateTimeModification] datetime2 NOT NULL DEFAULT '0001-01-01T00:00:00.0000000';
GO

ALTER TABLE [Licenses] ADD [DateTimeModification] datetime2 NOT NULL DEFAULT '0001-01-01T00:00:00.0000000';
GO

ALTER TABLE [Clients] ADD [DateTimeModification] datetime2 NOT NULL DEFAULT '0001-01-01T00:00:00.0000000';
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20221016164532_DateTimeModification', N'6.0.9');
GO

COMMIT;
GO

