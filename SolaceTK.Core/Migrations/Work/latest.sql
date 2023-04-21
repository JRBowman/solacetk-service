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

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220903013452_LatestObjects')
BEGIN
    CREATE TABLE [Artifacts] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(max) NULL,
        [Description] nvarchar(max) NULL,
        [AssemblyType] nvarchar(max) NULL,
        [Tags] nvarchar(max) NULL,
        [ArtifactUrl] nvarchar(max) NULL,
        [Created] datetimeoffset NOT NULL,
        [Updated] datetimeoffset NOT NULL,
        CONSTRAINT [PK_Artifacts] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220903013452_LatestObjects')
BEGIN
    CREATE TABLE [Projects] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(max) NULL,
        [Description] nvarchar(max) NULL,
        [AssemblyType] nvarchar(max) NULL,
        [Tags] nvarchar(max) NULL,
        [TotalEstimatedHours] real NOT NULL,
        [TotalActualHours] real NOT NULL,
        [PaidHours] real NOT NULL,
        [TotalPaid] real NOT NULL,
        [RemainingPayment] real NOT NULL,
        [Created] datetimeoffset NOT NULL,
        [Updated] datetimeoffset NOT NULL,
        CONSTRAINT [PK_Projects] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220903013452_LatestObjects')
BEGIN
    CREATE TABLE [Payments] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(max) NULL,
        [Description] nvarchar(max) NULL,
        [AssemblyType] nvarchar(max) NULL,
        [Tags] nvarchar(max) NULL,
        [ProjectId] uniqueidentifier NULL,
        [PaymentDate] datetimeoffset NOT NULL,
        [Amount] real NOT NULL,
        [Created] datetimeoffset NOT NULL,
        [Updated] datetimeoffset NOT NULL,
        CONSTRAINT [PK_Payments] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Payments_Projects_ProjectId] FOREIGN KEY ([ProjectId]) REFERENCES [Projects] ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220903013452_LatestObjects')
BEGIN
    CREATE TABLE [EnvironmentData] (
        [Id] uniqueidentifier NOT NULL,
        [Key] nvarchar(max) NULL,
        [Data] nvarchar(max) NULL,
        [WorkPaymentId] uniqueidentifier NULL,
        CONSTRAINT [PK_EnvironmentData] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_EnvironmentData_Payments_WorkPaymentId] FOREIGN KEY ([WorkPaymentId]) REFERENCES [Payments] ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220903013452_LatestObjects')
BEGIN
    CREATE TABLE [WorkItems] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(max) NULL,
        [Description] nvarchar(max) NULL,
        [AssemblyType] nvarchar(max) NULL,
        [Tags] nvarchar(max) NULL,
        [HoursEstimate] real NOT NULL,
        [HoursActual] real NOT NULL,
        [PaymentId] uniqueidentifier NULL,
        [IsPaid] bit NOT NULL,
        [WorkProjectId] uniqueidentifier NOT NULL,
        [ArtifactId] uniqueidentifier NULL,
        [Created] datetimeoffset NOT NULL,
        [Updated] datetimeoffset NOT NULL,
        CONSTRAINT [PK_WorkItems] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_WorkItems_Artifacts_ArtifactId] FOREIGN KEY ([ArtifactId]) REFERENCES [Artifacts] ([Id]),
        CONSTRAINT [FK_WorkItems_Payments_PaymentId] FOREIGN KEY ([PaymentId]) REFERENCES [Payments] ([Id]),
        CONSTRAINT [FK_WorkItems_Projects_WorkProjectId] FOREIGN KEY ([WorkProjectId]) REFERENCES [Projects] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220903013452_LatestObjects')
BEGIN
    CREATE TABLE [Comments] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(max) NULL,
        [Description] nvarchar(max) NULL,
        [AssemblyType] nvarchar(max) NULL,
        [Tags] nvarchar(max) NULL,
        [Comment] nvarchar(max) NULL,
        [Created] datetimeoffset NOT NULL,
        [Updated] datetimeoffset NOT NULL,
        [WorkItemId] uniqueidentifier NULL,
        [WorkProjectId] uniqueidentifier NULL,
        CONSTRAINT [PK_Comments] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Comments_Projects_WorkProjectId] FOREIGN KEY ([WorkProjectId]) REFERENCES [Projects] ([Id]),
        CONSTRAINT [FK_Comments_WorkItems_WorkItemId] FOREIGN KEY ([WorkItemId]) REFERENCES [WorkItems] ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220903013452_LatestObjects')
BEGIN
    CREATE INDEX [IX_Comments_WorkItemId] ON [Comments] ([WorkItemId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220903013452_LatestObjects')
BEGIN
    CREATE INDEX [IX_Comments_WorkProjectId] ON [Comments] ([WorkProjectId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220903013452_LatestObjects')
BEGIN
    CREATE INDEX [IX_EnvironmentData_WorkPaymentId] ON [EnvironmentData] ([WorkPaymentId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220903013452_LatestObjects')
BEGIN
    CREATE INDEX [IX_Payments_ProjectId] ON [Payments] ([ProjectId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220903013452_LatestObjects')
BEGIN
    CREATE INDEX [IX_WorkItems_ArtifactId] ON [WorkItems] ([ArtifactId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220903013452_LatestObjects')
BEGIN
    CREATE INDEX [IX_WorkItems_PaymentId] ON [WorkItems] ([PaymentId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220903013452_LatestObjects')
BEGIN
    CREATE INDEX [IX_WorkItems_WorkProjectId] ON [WorkItems] ([WorkProjectId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220903013452_LatestObjects')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20220903013452_LatestObjects', N'6.0.8');
END;
GO

COMMIT;
GO

