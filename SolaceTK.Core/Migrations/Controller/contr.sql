CREATE TABLE [BehaviorAnimationData] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NULL,
    [Description] nvarchar(max) NULL,
    [Tags] nvarchar(max) NULL,
    [Enabled] bit NOT NULL,
    [Loop] bit NOT NULL,
    [Invert] bit NOT NULL,
    [Mirror] bit NOT NULL,
    [Speed] real NOT NULL,
    [RunCount] real NOT NULL,
    CONSTRAINT [PK_BehaviorAnimationData] PRIMARY KEY ([Id])
);
GO


CREATE TABLE [BehaviorSystem] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NULL,
    [Description] nvarchar(max) NULL,
    [BehaviorType] int NOT NULL,
    [Tags] nvarchar(max) NULL,
    CONSTRAINT [PK_BehaviorSystem] PRIMARY KEY ([Id])
);
GO


CREATE TABLE [ImmovableObjects] (
    [Id] int NOT NULL IDENTITY,
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
    [Tags] nvarchar(max) NULL,
    [Discriminator] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_ImmovableObjects] PRIMARY KEY ([Id])
);
GO


CREATE TABLE [SoundSet] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NULL,
    [Description] nvarchar(max) NULL,
    [Tags] nvarchar(max) NULL,
    CONSTRAINT [PK_SoundSet] PRIMARY KEY ([Id])
);
GO


CREATE TABLE [BehaviorAnimation] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NULL,
    [Description] nvarchar(max) NULL,
    [Tags] nvarchar(max) NULL,
    [StartFrameDataId] int NULL,
    [ActFrameDataId] int NULL,
    [EndFrameDataId] int NULL,
    CONSTRAINT [PK_BehaviorAnimation] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_BehaviorAnimation_BehaviorAnimationData_ActFrameDataId] FOREIGN KEY ([ActFrameDataId]) REFERENCES [BehaviorAnimationData] ([Id]),
    CONSTRAINT [FK_BehaviorAnimation_BehaviorAnimationData_EndFrameDataId] FOREIGN KEY ([EndFrameDataId]) REFERENCES [BehaviorAnimationData] ([Id]),
    CONSTRAINT [FK_BehaviorAnimation_BehaviorAnimationData_StartFrameDataId] FOREIGN KEY ([StartFrameDataId]) REFERENCES [BehaviorAnimationData] ([Id])
);
GO


CREATE TABLE [BehaviorAnimationFrame] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NULL,
    [Description] nvarchar(max) NULL,
    [Tags] nvarchar(max) NULL,
    [Order] int NOT NULL,
    [Speed] real NOT NULL,
    [Duration] real NOT NULL,
    [FrameData] nvarchar(max) NULL,
    [BehaviorAnimationDataId] int NULL,
    CONSTRAINT [PK_BehaviorAnimationFrame] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_BehaviorAnimationFrame_BehaviorAnimationData_BehaviorAnimationDataId] FOREIGN KEY ([BehaviorAnimationDataId]) REFERENCES [BehaviorAnimationData] ([Id])
);
GO


CREATE TABLE [MovableObjects] (
    [Id] int NOT NULL IDENTITY,
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
    [BehaviorSystemId] int NULL,
    [SoundSetId] int NULL,
    [Type] int NOT NULL,
    [Tags] nvarchar(max) NULL,
    [Discriminator] nvarchar(max) NOT NULL,
    [DirectionX] real NULL,
    [DirectionY] real NULL,
    CONSTRAINT [PK_MovableObjects] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_MovableObjects_BehaviorSystem_BehaviorSystemId] FOREIGN KEY ([BehaviorSystemId]) REFERENCES [BehaviorSystem] ([Id]),
    CONSTRAINT [FK_MovableObjects_SoundSet_SoundSetId] FOREIGN KEY ([SoundSetId]) REFERENCES [SoundSet] ([Id])
);
GO


CREATE TABLE [SoundSource] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NULL,
    [Description] nvarchar(max) NULL,
    [Tags] nvarchar(max) NULL,
    [IsLoop] bit NOT NULL,
    [Volume] real NOT NULL,
    [Pitch] real NOT NULL,
    [PlayOnLoad] bit NOT NULL,
    [ClipName] nvarchar(max) NULL,
    [LoopStartTime] datetime2 NOT NULL,
    [LoopEndTime] datetime2 NOT NULL,
    [SoundSetId] int NULL,
    CONSTRAINT [PK_SoundSource] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_SoundSource_SoundSet_SoundSetId] FOREIGN KEY ([SoundSetId]) REFERENCES [SoundSet] ([Id])
);
GO


CREATE TABLE [BehaviorState] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NULL,
    [Description] nvarchar(max) NULL,
    [Tags] nvarchar(max) NULL,
    [StateType] nvarchar(max) NULL,
    [RunCount] int NOT NULL,
    [NoOp] bit NOT NULL,
    [Enabled] bit NOT NULL,
    [AnimationId] int NULL,
    [StartDelay] nvarchar(max) NULL,
    [EndDelay] nvarchar(max) NULL,
    [Interruptable] bit NOT NULL,
    [BehaviorStateId] int NULL,
    [BehaviorSystemId] int NULL,
    CONSTRAINT [PK_BehaviorState] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_BehaviorState_BehaviorAnimation_AnimationId] FOREIGN KEY ([AnimationId]) REFERENCES [BehaviorAnimation] ([Id]),
    CONSTRAINT [FK_BehaviorState_BehaviorState_BehaviorStateId] FOREIGN KEY ([BehaviorStateId]) REFERENCES [BehaviorState] ([Id]),
    CONSTRAINT [FK_BehaviorState_BehaviorSystem_BehaviorSystemId] FOREIGN KEY ([BehaviorSystemId]) REFERENCES [BehaviorSystem] ([Id])
);
GO


CREATE TABLE [ControllerComponent] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NULL,
    [Description] nvarchar(max) NULL,
    [Tags] nvarchar(max) NULL,
    [PositionX] real NOT NULL,
    [PositionY] real NOT NULL,
    [PositionZ] real NOT NULL,
    [RotationX] real NOT NULL,
    [RotationY] real NOT NULL,
    [RotationZ] real NOT NULL,
    [ScaleX] real NOT NULL,
    [ScaleY] real NOT NULL,
    [ScaleZ] real NOT NULL,
    [MovableControllerId] int NULL,
    CONSTRAINT [PK_ControllerComponent] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ControllerComponent_MovableObjects_MovableControllerId] FOREIGN KEY ([MovableControllerId]) REFERENCES [MovableObjects] ([Id])
);
GO


CREATE TABLE [BehaviorEvent] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NULL,
    [Description] nvarchar(max) NULL,
    [Tags] nvarchar(max) NULL,
    [BehaviorStateId] int NULL,
    [BehaviorSystemId] int NULL,
    CONSTRAINT [PK_BehaviorEvent] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_BehaviorEvent_BehaviorState_BehaviorStateId] FOREIGN KEY ([BehaviorStateId]) REFERENCES [BehaviorState] ([Id]),
    CONSTRAINT [FK_BehaviorEvent_BehaviorSystem_BehaviorSystemId] FOREIGN KEY ([BehaviorSystemId]) REFERENCES [BehaviorSystem] ([Id])
);
GO


CREATE TABLE [BehaviorMessage] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NULL,
    [Description] nvarchar(max) NULL,
    [Tags] nvarchar(max) NULL,
    [TargetName] nvarchar(max) NULL,
    [TargetType] int NOT NULL,
    [BehaviorEventId] int NULL,
    CONSTRAINT [PK_BehaviorMessage] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_BehaviorMessage_BehaviorEvent_BehaviorEventId] FOREIGN KEY ([BehaviorEventId]) REFERENCES [BehaviorEvent] ([Id])
);
GO


CREATE TABLE [SolTkCondition] (
    [Id] int NOT NULL IDENTITY,
    [Key] nvarchar(max) NULL,
    [Data] nvarchar(max) NULL,
    [Operator] int NOT NULL,
    [BehaviorEventId] int NULL,
    [BehaviorStateId] int NULL,
    CONSTRAINT [PK_SolTkCondition] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_SolTkCondition_BehaviorEvent_BehaviorEventId] FOREIGN KEY ([BehaviorEventId]) REFERENCES [BehaviorEvent] ([Id]),
    CONSTRAINT [FK_SolTkCondition_BehaviorState_BehaviorStateId] FOREIGN KEY ([BehaviorStateId]) REFERENCES [BehaviorState] ([Id])
);
GO


CREATE TABLE [ControllerData] (
    [Id] int NOT NULL IDENTITY,
    [Key] nvarchar(max) NULL,
    [Data] nvarchar(max) NULL,
    [Operator] int NOT NULL,
    [BehaviorAnimationFrameId] int NULL,
    [BehaviorEventId] int NULL,
    [BehaviorMessageId] int NULL,
    [BehaviorStateId] int NULL,
    [BehaviorStateId1] int NULL,
    [BehaviorStateId2] int NULL,
    [BehaviorSystemId] int NULL,
    [ControllerComponentId] int NULL,
    [MovableControllerId] int NULL,
    [SoundSourceId] int NULL,
    CONSTRAINT [PK_ControllerData] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ControllerData_BehaviorAnimationFrame_BehaviorAnimationFrameId] FOREIGN KEY ([BehaviorAnimationFrameId]) REFERENCES [BehaviorAnimationFrame] ([Id]),
    CONSTRAINT [FK_ControllerData_BehaviorEvent_BehaviorEventId] FOREIGN KEY ([BehaviorEventId]) REFERENCES [BehaviorEvent] ([Id]),
    CONSTRAINT [FK_ControllerData_BehaviorMessage_BehaviorMessageId] FOREIGN KEY ([BehaviorMessageId]) REFERENCES [BehaviorMessage] ([Id]),
    CONSTRAINT [FK_ControllerData_BehaviorState_BehaviorStateId] FOREIGN KEY ([BehaviorStateId]) REFERENCES [BehaviorState] ([Id]),
    CONSTRAINT [FK_ControllerData_BehaviorState_BehaviorStateId1] FOREIGN KEY ([BehaviorStateId1]) REFERENCES [BehaviorState] ([Id]),
    CONSTRAINT [FK_ControllerData_BehaviorState_BehaviorStateId2] FOREIGN KEY ([BehaviorStateId2]) REFERENCES [BehaviorState] ([Id]),
    CONSTRAINT [FK_ControllerData_BehaviorSystem_BehaviorSystemId] FOREIGN KEY ([BehaviorSystemId]) REFERENCES [BehaviorSystem] ([Id]),
    CONSTRAINT [FK_ControllerData_ControllerComponent_ControllerComponentId] FOREIGN KEY ([ControllerComponentId]) REFERENCES [ControllerComponent] ([Id]),
    CONSTRAINT [FK_ControllerData_MovableObjects_MovableControllerId] FOREIGN KEY ([MovableControllerId]) REFERENCES [MovableObjects] ([Id]),
    CONSTRAINT [FK_ControllerData_SoundSource_SoundSourceId] FOREIGN KEY ([SoundSourceId]) REFERENCES [SoundSource] ([Id])
);
GO


CREATE INDEX [IX_BehaviorAnimation_ActFrameDataId] ON [BehaviorAnimation] ([ActFrameDataId]);
GO


CREATE INDEX [IX_BehaviorAnimation_EndFrameDataId] ON [BehaviorAnimation] ([EndFrameDataId]);
GO


CREATE INDEX [IX_BehaviorAnimation_StartFrameDataId] ON [BehaviorAnimation] ([StartFrameDataId]);
GO


CREATE INDEX [IX_BehaviorAnimationFrame_BehaviorAnimationDataId] ON [BehaviorAnimationFrame] ([BehaviorAnimationDataId]);
GO


CREATE INDEX [IX_BehaviorEvent_BehaviorStateId] ON [BehaviorEvent] ([BehaviorStateId]);
GO


CREATE INDEX [IX_BehaviorEvent_BehaviorSystemId] ON [BehaviorEvent] ([BehaviorSystemId]);
GO


CREATE INDEX [IX_BehaviorMessage_BehaviorEventId] ON [BehaviorMessage] ([BehaviorEventId]);
GO


CREATE INDEX [IX_BehaviorState_AnimationId] ON [BehaviorState] ([AnimationId]);
GO


CREATE INDEX [IX_BehaviorState_BehaviorStateId] ON [BehaviorState] ([BehaviorStateId]);
GO


CREATE INDEX [IX_BehaviorState_BehaviorSystemId] ON [BehaviorState] ([BehaviorSystemId]);
GO


CREATE INDEX [IX_ControllerComponent_MovableControllerId] ON [ControllerComponent] ([MovableControllerId]);
GO


CREATE INDEX [IX_ControllerData_BehaviorAnimationFrameId] ON [ControllerData] ([BehaviorAnimationFrameId]);
GO


CREATE INDEX [IX_ControllerData_BehaviorEventId] ON [ControllerData] ([BehaviorEventId]);
GO


CREATE INDEX [IX_ControllerData_BehaviorMessageId] ON [ControllerData] ([BehaviorMessageId]);
GO


CREATE INDEX [IX_ControllerData_BehaviorStateId] ON [ControllerData] ([BehaviorStateId]);
GO


CREATE INDEX [IX_ControllerData_BehaviorStateId1] ON [ControllerData] ([BehaviorStateId1]);
GO


CREATE INDEX [IX_ControllerData_BehaviorStateId2] ON [ControllerData] ([BehaviorStateId2]);
GO


CREATE INDEX [IX_ControllerData_BehaviorSystemId] ON [ControllerData] ([BehaviorSystemId]);
GO


CREATE INDEX [IX_ControllerData_ControllerComponentId] ON [ControllerData] ([ControllerComponentId]);
GO


CREATE INDEX [IX_ControllerData_MovableControllerId] ON [ControllerData] ([MovableControllerId]);
GO


CREATE INDEX [IX_ControllerData_SoundSourceId] ON [ControllerData] ([SoundSourceId]);
GO


CREATE INDEX [IX_MovableObjects_BehaviorSystemId] ON [MovableObjects] ([BehaviorSystemId]);
GO


CREATE INDEX [IX_MovableObjects_SoundSetId] ON [MovableObjects] ([SoundSetId]);
GO


CREATE INDEX [IX_SolTkCondition_BehaviorEventId] ON [SolTkCondition] ([BehaviorEventId]);
GO


CREATE INDEX [IX_SolTkCondition_BehaviorStateId] ON [SolTkCondition] ([BehaviorStateId]);
GO


CREATE INDEX [IX_SoundSource_SoundSetId] ON [SoundSource] ([SoundSetId]);
GO


