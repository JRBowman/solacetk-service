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

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220903013356_LatestObjects')
BEGIN
    CREATE TABLE [BehaviorSystem] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(max) NULL,
        [Description] nvarchar(max) NULL,
        [BehaviorType] int NOT NULL,
        [AssemblyType] nvarchar(max) NULL,
        [Tags] nvarchar(max) NULL,
        CONSTRAINT [PK_BehaviorSystem] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220903013356_LatestObjects')
BEGIN
    CREATE TABLE [ImmovableObjects] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(max) NULL,
        [Description] nvarchar(max) NULL,
        [PixelKeyColor] nvarchar(max) NULL,
        [WorldPositionX] real NOT NULL,
        [WorldPositionY] real NOT NULL,
        [WorldPositionZ] real NOT NULL,
        [MapPositionX] int NOT NULL,
        [MapPositionY] int NOT NULL,
        [CollisionType] int NOT NULL,
        [IsHit] bit NOT NULL,
        [AssemblyType] nvarchar(max) NULL,
        [Tags] nvarchar(max) NULL,
        [Discriminator] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_ImmovableObjects] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220903013356_LatestObjects')
BEGIN
    CREATE TABLE [BehaviorBranch] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(max) NULL,
        [Description] nvarchar(max) NULL,
        [BranchType] int NOT NULL,
        [AssemblyType] nvarchar(max) NULL,
        [Tags] nvarchar(max) NULL,
        [BehaviorSystemId] uniqueidentifier NULL,
        CONSTRAINT [PK_BehaviorBranch] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_BehaviorBranch_BehaviorSystem_BehaviorSystemId] FOREIGN KEY ([BehaviorSystemId]) REFERENCES [BehaviorSystem] ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220903013356_LatestObjects')
BEGIN
    CREATE TABLE [MovableObjects] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(max) NULL,
        [Description] nvarchar(max) NULL,
        [PixelKeyColor] nvarchar(max) NULL,
        [WorldPositionX] real NOT NULL,
        [WorldPositionY] real NOT NULL,
        [WorldPositionZ] real NOT NULL,
        [MapPositionX] int NOT NULL,
        [MapPositionY] int NOT NULL,
        [CollisionType] int NOT NULL,
        [UseFriction] bit NOT NULL,
        [AffectedByGravity] bit NOT NULL,
        [CanMove] bit NOT NULL,
        [Mass] real NOT NULL,
        [Speed] real NOT NULL,
        [IsHit] bit NOT NULL,
        [BehaviorSystemId] uniqueidentifier NULL,
        [Type] int NOT NULL,
        [AssemblyType] nvarchar(max) NULL,
        [Tags] nvarchar(max) NULL,
        [Discriminator] nvarchar(max) NOT NULL,
        [DirectionX] real NULL,
        [DirectionY] real NULL,
        CONSTRAINT [PK_MovableObjects] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_MovableObjects_BehaviorSystem_BehaviorSystemId] FOREIGN KEY ([BehaviorSystemId]) REFERENCES [BehaviorSystem] ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220903013356_LatestObjects')
BEGIN
    CREATE TABLE [BehaviorState] (
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
        CONSTRAINT [PK_BehaviorState] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_BehaviorState_BehaviorBranch_BehaviorBranchId] FOREIGN KEY ([BehaviorBranchId]) REFERENCES [BehaviorBranch] ([Id]),
        CONSTRAINT [FK_BehaviorState_BehaviorState_NextId] FOREIGN KEY ([NextId]) REFERENCES [BehaviorState] ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220903013356_LatestObjects')
BEGIN
    CREATE TABLE [BehaviorAction] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(max) NULL,
        [Description] nvarchar(max) NULL,
        [StateId] uniqueidentifier NOT NULL,
        [StartAction] nvarchar(max) NULL,
        [MainAction] nvarchar(max) NULL,
        [EndAction] nvarchar(max) NULL,
        [AssemblyType] nvarchar(max) NULL,
        [Tags] nvarchar(max) NULL,
        CONSTRAINT [PK_BehaviorAction] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_BehaviorAction_BehaviorState_StateId] FOREIGN KEY ([StateId]) REFERENCES [BehaviorState] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220903013356_LatestObjects')
BEGIN
    CREATE TABLE [BehaviorAnimation] (
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
        CONSTRAINT [PK_BehaviorAnimation] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_BehaviorAnimation_BehaviorState_BehaviorStateId] FOREIGN KEY ([BehaviorStateId]) REFERENCES [BehaviorState] ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220903013356_LatestObjects')
BEGIN
    CREATE TABLE [BehaviorCondition] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(max) NULL,
        [Description] nvarchar(max) NULL,
        [AssemblyType] nvarchar(max) NULL,
        [Tags] nvarchar(max) NULL,
        [BehaviorStateId] uniqueidentifier NULL,
        CONSTRAINT [PK_BehaviorCondition] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_BehaviorCondition_BehaviorState_BehaviorStateId] FOREIGN KEY ([BehaviorStateId]) REFERENCES [BehaviorState] ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220903013356_LatestObjects')
BEGIN
    CREATE TABLE [ControllerData] (
        [Id] uniqueidentifier NOT NULL,
        [Key] nvarchar(max) NULL,
        [Data] nvarchar(max) NULL,
        [BehaviorConditionId] uniqueidentifier NULL,
        [MovableControllerId] uniqueidentifier NULL,
        CONSTRAINT [PK_ControllerData] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_ControllerData_BehaviorCondition_BehaviorConditionId] FOREIGN KEY ([BehaviorConditionId]) REFERENCES [BehaviorCondition] ([Id]),
        CONSTRAINT [FK_ControllerData_MovableObjects_MovableControllerId] FOREIGN KEY ([MovableControllerId]) REFERENCES [MovableObjects] ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220903013356_LatestObjects')
BEGIN
    CREATE UNIQUE INDEX [IX_BehaviorAction_StateId] ON [BehaviorAction] ([StateId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220903013356_LatestObjects')
BEGIN
    CREATE INDEX [IX_BehaviorAnimation_BehaviorStateId] ON [BehaviorAnimation] ([BehaviorStateId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220903013356_LatestObjects')
BEGIN
    CREATE INDEX [IX_BehaviorBranch_BehaviorSystemId] ON [BehaviorBranch] ([BehaviorSystemId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220903013356_LatestObjects')
BEGIN
    CREATE INDEX [IX_BehaviorCondition_BehaviorStateId] ON [BehaviorCondition] ([BehaviorStateId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220903013356_LatestObjects')
BEGIN
    CREATE INDEX [IX_BehaviorState_BehaviorBranchId] ON [BehaviorState] ([BehaviorBranchId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220903013356_LatestObjects')
BEGIN
    CREATE INDEX [IX_BehaviorState_NextId] ON [BehaviorState] ([NextId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220903013356_LatestObjects')
BEGIN
    CREATE INDEX [IX_ControllerData_BehaviorConditionId] ON [ControllerData] ([BehaviorConditionId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220903013356_LatestObjects')
BEGIN
    CREATE INDEX [IX_ControllerData_MovableControllerId] ON [ControllerData] ([MovableControllerId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220903013356_LatestObjects')
BEGIN
    CREATE INDEX [IX_MovableObjects_BehaviorSystemId] ON [MovableObjects] ([BehaviorSystemId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220903013356_LatestObjects')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20220903013356_LatestObjects', N'6.0.8');
END;
GO

COMMIT;
GO

