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

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220903013337_LatestObjects')
BEGIN
    CREATE TABLE [Systems] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(max) NULL,
        [Description] nvarchar(max) NULL,
        [BehaviorType] int NOT NULL,
        [AssemblyType] nvarchar(max) NULL,
        [Tags] nvarchar(max) NULL,
        CONSTRAINT [PK_Systems] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220903013337_LatestObjects')
BEGIN
    CREATE TABLE [Branches] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(max) NULL,
        [Description] nvarchar(max) NULL,
        [BranchType] int NOT NULL,
        [AssemblyType] nvarchar(max) NULL,
        [Tags] nvarchar(max) NULL,
        [BehaviorSystemId] uniqueidentifier NULL,
        CONSTRAINT [PK_Branches] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Branches_Systems_BehaviorSystemId] FOREIGN KEY ([BehaviorSystemId]) REFERENCES [Systems] ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220903013337_LatestObjects')
BEGIN
    CREATE TABLE [States] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(max) NULL,
        [Description] nvarchar(max) NULL,
        [StateType] nvarchar(max) NULL,
        [NextId] uniqueidentifier NULL,
        [StartDelay] nvarchar(max) NULL,
        [EndDelay] nvarchar(max) NULL,
        [Interruptable] bit NOT NULL,
        [AssemblyType] nvarchar(max) NULL,
        [Tags] nvarchar(max) NULL,
        [BehaviorBranchId] uniqueidentifier NULL,
        CONSTRAINT [PK_States] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_States_Branches_BehaviorBranchId] FOREIGN KEY ([BehaviorBranchId]) REFERENCES [Branches] ([Id]),
        CONSTRAINT [FK_States_States_NextId] FOREIGN KEY ([NextId]) REFERENCES [States] ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220903013337_LatestObjects')
BEGIN
    CREATE TABLE [Actions] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(max) NULL,
        [Description] nvarchar(max) NULL,
        [StateId] uniqueidentifier NOT NULL,
        [StartAction] nvarchar(max) NULL,
        [MainAction] nvarchar(max) NULL,
        [EndAction] nvarchar(max) NULL,
        [AssemblyType] nvarchar(max) NULL,
        [Tags] nvarchar(max) NULL,
        CONSTRAINT [PK_Actions] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Actions_States_StateId] FOREIGN KEY ([StateId]) REFERENCES [States] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220903013337_LatestObjects')
BEGIN
    CREATE TABLE [Animations] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(max) NULL,
        [Description] nvarchar(max) NULL,
        [StartFrameData_Id] uniqueidentifier NULL,
        [StartFrameData_Name] nvarchar(max) NULL,
        [StartFrameData_Description] nvarchar(max) NULL,
        [StartFrameData_AssemblyType] nvarchar(max) NULL,
        [StartFrameData_Tags] nvarchar(max) NULL,
        [StartFrameData_Loop] bit NULL,
        [StartFrameData_Invert] bit NULL,
        [StartFrameData_Mirror] bit NULL,
        [StartFrameData_Speed] real NULL,
        [StartFrameData_FramesJson] nvarchar(max) NULL,
        [StartFrameData_FramesSheet] nvarchar(max) NULL,
        [ActFrameData_Id] uniqueidentifier NULL,
        [ActFrameData_Name] nvarchar(max) NULL,
        [ActFrameData_Description] nvarchar(max) NULL,
        [ActFrameData_AssemblyType] nvarchar(max) NULL,
        [ActFrameData_Tags] nvarchar(max) NULL,
        [ActFrameData_Loop] bit NULL,
        [ActFrameData_Invert] bit NULL,
        [ActFrameData_Mirror] bit NULL,
        [ActFrameData_Speed] real NULL,
        [ActFrameData_FramesJson] nvarchar(max) NULL,
        [ActFrameData_FramesSheet] nvarchar(max) NULL,
        [EndFrameData_Id] uniqueidentifier NULL,
        [EndFrameData_Name] nvarchar(max) NULL,
        [EndFrameData_Description] nvarchar(max) NULL,
        [EndFrameData_AssemblyType] nvarchar(max) NULL,
        [EndFrameData_Tags] nvarchar(max) NULL,
        [EndFrameData_Loop] bit NULL,
        [EndFrameData_Invert] bit NULL,
        [EndFrameData_Mirror] bit NULL,
        [EndFrameData_Speed] real NULL,
        [EndFrameData_FramesJson] nvarchar(max) NULL,
        [EndFrameData_FramesSheet] nvarchar(max) NULL,
        [AssemblyType] nvarchar(max) NULL,
        [Tags] nvarchar(max) NULL,
        [BehaviorStateId] uniqueidentifier NULL,
        CONSTRAINT [PK_Animations] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Animations_States_BehaviorStateId] FOREIGN KEY ([BehaviorStateId]) REFERENCES [States] ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220903013337_LatestObjects')
BEGIN
    CREATE TABLE [Conditions] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(max) NULL,
        [Description] nvarchar(max) NULL,
        [AssemblyType] nvarchar(max) NULL,
        [Tags] nvarchar(max) NULL,
        [BehaviorStateId] uniqueidentifier NULL,
        CONSTRAINT [PK_Conditions] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Conditions_States_BehaviorStateId] FOREIGN KEY ([BehaviorStateId]) REFERENCES [States] ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220903013337_LatestObjects')
BEGIN
    CREATE TABLE [AttributeData] (
        [Id] uniqueidentifier NOT NULL,
        [Key] nvarchar(max) NULL,
        [Data] nvarchar(max) NULL,
        [BehaviorConditionId] uniqueidentifier NULL,
        CONSTRAINT [PK_AttributeData] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AttributeData_Conditions_BehaviorConditionId] FOREIGN KEY ([BehaviorConditionId]) REFERENCES [Conditions] ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220903013337_LatestObjects')
BEGIN
    CREATE UNIQUE INDEX [IX_Actions_StateId] ON [Actions] ([StateId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220903013337_LatestObjects')
BEGIN
    CREATE INDEX [IX_Animations_BehaviorStateId] ON [Animations] ([BehaviorStateId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220903013337_LatestObjects')
BEGIN
    CREATE INDEX [IX_AttributeData_BehaviorConditionId] ON [AttributeData] ([BehaviorConditionId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220903013337_LatestObjects')
BEGIN
    CREATE INDEX [IX_Branches_BehaviorSystemId] ON [Branches] ([BehaviorSystemId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220903013337_LatestObjects')
BEGIN
    CREATE INDEX [IX_Conditions_BehaviorStateId] ON [Conditions] ([BehaviorStateId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220903013337_LatestObjects')
BEGIN
    CREATE INDEX [IX_States_BehaviorBranchId] ON [States] ([BehaviorBranchId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220903013337_LatestObjects')
BEGIN
    CREATE INDEX [IX_States_NextId] ON [States] ([NextId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220903013337_LatestObjects')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20220903013337_LatestObjects', N'6.0.8');
END;
GO

COMMIT;
GO

