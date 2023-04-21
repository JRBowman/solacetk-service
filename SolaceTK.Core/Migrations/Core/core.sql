CREATE TABLE [Attachments] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NULL,
    [Description] nvarchar(max) NULL,
    [Tags] nvarchar(max) NULL,
    [FileName] nvarchar(max) NULL,
    [FileUrl] nvarchar(max) NULL,
    [FileLocation] nvarchar(max) NULL,
    CONSTRAINT [PK_Attachments] PRIMARY KEY ([Id])
);
GO


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


CREATE TABLE [Collections] (
    [Id] int NOT NULL IDENTITY,
    [Tags] nvarchar(max) NULL,
    [Name] nvarchar(max) NULL,
    [Description] nvarchar(max) NULL,
    CONSTRAINT [PK_Collections] PRIMARY KEY ([Id])
);
GO


CREATE TABLE [Timelines] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NULL,
    [Description] nvarchar(max) NULL,
    [Tags] nvarchar(max) NULL,
    [Length] bigint NOT NULL,
    CONSTRAINT [PK_Timelines] PRIMARY KEY ([Id])
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


CREATE TABLE [BehaviorSystem] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NULL,
    [Description] nvarchar(max) NULL,
    [BehaviorType] int NOT NULL,
    [Tags] nvarchar(max) NULL,
    [Discriminator] nvarchar(max) NOT NULL,
    [ResourceCollectionId] int NULL,
    CONSTRAINT [PK_BehaviorSystem] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_BehaviorSystem_Collections_ResourceCollectionId] FOREIGN KEY ([ResourceCollectionId]) REFERENCES [Collections] ([Id])
);
GO


CREATE TABLE [Hud] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NULL,
    [Description] nvarchar(max) NULL,
    [Tags] nvarchar(max) NULL,
    [ResourceCollectionId] int NULL,
    CONSTRAINT [PK_Hud] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Hud_Collections_ResourceCollectionId] FOREIGN KEY ([ResourceCollectionId]) REFERENCES [Collections] ([Id])
);
GO


CREATE TABLE [HudDialog] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NULL,
    [Description] nvarchar(max) NULL,
    [Tags] nvarchar(max) NULL,
    [ResourceCollectionId] int NULL,
    CONSTRAINT [PK_HudDialog] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_HudDialog_Collections_ResourceCollectionId] FOREIGN KEY ([ResourceCollectionId]) REFERENCES [Collections] ([Id])
);
GO


CREATE TABLE [HudMenu] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NULL,
    [Description] nvarchar(max) NULL,
    [Tags] nvarchar(max) NULL,
    [ResourceCollectionId] int NULL,
    CONSTRAINT [PK_HudMenu] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_HudMenu_Collections_ResourceCollectionId] FOREIGN KEY ([ResourceCollectionId]) REFERENCES [Collections] ([Id])
);
GO


CREATE TABLE [ImmovableController] (
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
    [ResourceCollectionId] int NULL,
    [ResourceCollectionId1] int NULL,
    [StaticObjectController_ResourceCollectionId1] int NULL,
    CONSTRAINT [PK_ImmovableController] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ImmovableController_Collections_ResourceCollectionId] FOREIGN KEY ([ResourceCollectionId]) REFERENCES [Collections] ([Id]),
    CONSTRAINT [FK_ImmovableController_Collections_ResourceCollectionId1] FOREIGN KEY ([ResourceCollectionId1]) REFERENCES [Collections] ([Id]),
    CONSTRAINT [FK_ImmovableController_Collections_StaticObjectController_ResourceCollectionId1] FOREIGN KEY ([StaticObjectController_ResourceCollectionId1]) REFERENCES [Collections] ([Id])
);
GO


CREATE TABLE [SoundSet] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NULL,
    [Description] nvarchar(max) NULL,
    [Tags] nvarchar(max) NULL,
    [ResourceCollectionId] int NULL,
    CONSTRAINT [PK_SoundSet] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_SoundSet_Collections_ResourceCollectionId] FOREIGN KEY ([ResourceCollectionId]) REFERENCES [Collections] ([Id])
);
GO


CREATE TABLE [TileSet] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NULL,
    [Description] nvarchar(max) NULL,
    [Tags] nvarchar(max) NULL,
    [ResourceCollectionId] int NULL,
    CONSTRAINT [PK_TileSet] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_TileSet_Collections_ResourceCollectionId] FOREIGN KEY ([ResourceCollectionId]) REFERENCES [Collections] ([Id])
);
GO


CREATE TABLE [StoryCards] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NULL,
    [Description] nvarchar(max) NULL,
    [Tags] nvarchar(max) NULL,
    [Enabled] bit NOT NULL,
    [Completed] bit NOT NULL,
    [Title] nvarchar(max) NULL,
    [Order] int NOT NULL,
    [StartTime] bigint NOT NULL,
    [Duration] bigint NOT NULL,
    [EndTime] bigint NOT NULL,
    [TimelineId] int NULL,
    CONSTRAINT [PK_StoryCards] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_StoryCards_Timelines_TimelineId] FOREIGN KEY ([TimelineId]) REFERENCES [Timelines] ([Id])
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


CREATE TABLE [MovableController] (
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
    [ResourceCollectionId] int NULL,
    [CharacterController_ResourceCollectionId1] int NULL,
    [ResourceCollectionId1] int NULL,
    [DirectionX] real NULL,
    [DirectionY] real NULL,
    [TransportController_ResourceCollectionId1] int NULL,
    CONSTRAINT [PK_MovableController] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_MovableController_BehaviorSystem_BehaviorSystemId] FOREIGN KEY ([BehaviorSystemId]) REFERENCES [BehaviorSystem] ([Id]),
    CONSTRAINT [FK_MovableController_Collections_CharacterController_ResourceCollectionId1] FOREIGN KEY ([CharacterController_ResourceCollectionId1]) REFERENCES [Collections] ([Id]),
    CONSTRAINT [FK_MovableController_Collections_ResourceCollectionId] FOREIGN KEY ([ResourceCollectionId]) REFERENCES [Collections] ([Id]),
    CONSTRAINT [FK_MovableController_Collections_ResourceCollectionId1] FOREIGN KEY ([ResourceCollectionId1]) REFERENCES [Collections] ([Id]),
    CONSTRAINT [FK_MovableController_Collections_TransportController_ResourceCollectionId1] FOREIGN KEY ([TransportController_ResourceCollectionId1]) REFERENCES [Collections] ([Id]),
    CONSTRAINT [FK_MovableController_SoundSet_SoundSetId] FOREIGN KEY ([SoundSetId]) REFERENCES [SoundSet] ([Id])
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
    [ResourceCollectionId] int NULL,
    [SoundSetId] int NULL,
    CONSTRAINT [PK_SoundSource] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_SoundSource_Collections_ResourceCollectionId] FOREIGN KEY ([ResourceCollectionId]) REFERENCES [Collections] ([Id]),
    CONSTRAINT [FK_SoundSource_SoundSet_SoundSetId] FOREIGN KEY ([SoundSetId]) REFERENCES [SoundSet] ([Id])
);
GO


CREATE TABLE [Tile] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NULL,
    [Description] nvarchar(max) NULL,
    [Tags] nvarchar(max) NULL,
    [ColorKey] nvarchar(max) NULL,
    [Type] int NOT NULL,
    [Mode] int NOT NULL,
    [LX] int NOT NULL,
    [LY] int NOT NULL,
    [Width] int NOT NULL,
    [Height] int NOT NULL,
    [ObjectKey] nvarchar(max) NULL,
    [TileSetId] int NULL,
    CONSTRAINT [PK_Tile] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Tile_TileSet_TileSetId] FOREIGN KEY ([TileSetId]) REFERENCES [TileSet] ([Id])
);
GO


CREATE TABLE [BehaviorEvent] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NULL,
    [Description] nvarchar(max) NULL,
    [Tags] nvarchar(max) NULL,
    [BehaviorStateId] int NULL,
    [BehaviorSystemId] int NULL,
    [StoryCardId] int NULL,
    CONSTRAINT [PK_BehaviorEvent] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_BehaviorEvent_BehaviorState_BehaviorStateId] FOREIGN KEY ([BehaviorStateId]) REFERENCES [BehaviorState] ([Id]),
    CONSTRAINT [FK_BehaviorEvent_BehaviorSystem_BehaviorSystemId] FOREIGN KEY ([BehaviorSystemId]) REFERENCES [BehaviorSystem] ([Id]),
    CONSTRAINT [FK_BehaviorEvent_StoryCards_StoryCardId] FOREIGN KEY ([StoryCardId]) REFERENCES [StoryCards] ([Id])
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
    CONSTRAINT [FK_ControllerComponent_MovableController_MovableControllerId] FOREIGN KEY ([MovableControllerId]) REFERENCES [MovableController] ([Id])
);
GO


CREATE TABLE [TileRule] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NULL,
    [Description] nvarchar(max) NULL,
    [Tags] nvarchar(max) NULL,
    [Priority] int NOT NULL,
    [ColorKey] nvarchar(max) NULL,
    [DataKey] nvarchar(max) NULL,
    [VX] int NOT NULL,
    [VY] int NOT NULL,
    [VM] int NOT NULL,
    [CheckType] int NOT NULL,
    [TileId] int NULL,
    CONSTRAINT [PK_TileRule] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_TileRule_Tile_TileId] FOREIGN KEY ([TileId]) REFERENCES [Tile] ([Id])
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
    [StoryCardId] int NULL,
    CONSTRAINT [PK_SolTkCondition] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_SolTkCondition_BehaviorEvent_BehaviorEventId] FOREIGN KEY ([BehaviorEventId]) REFERENCES [BehaviorEvent] ([Id]),
    CONSTRAINT [FK_SolTkCondition_BehaviorState_BehaviorStateId] FOREIGN KEY ([BehaviorStateId]) REFERENCES [BehaviorState] ([Id]),
    CONSTRAINT [FK_SolTkCondition_StoryCards_StoryCardId] FOREIGN KEY ([StoryCardId]) REFERENCES [StoryCards] ([Id])
);
GO


CREATE TABLE [SolTkData] (
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
    [StoryCardId] int NULL,
    [TileId] int NULL,
    CONSTRAINT [PK_SolTkData] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_SolTkData_BehaviorAnimationFrame_BehaviorAnimationFrameId] FOREIGN KEY ([BehaviorAnimationFrameId]) REFERENCES [BehaviorAnimationFrame] ([Id]),
    CONSTRAINT [FK_SolTkData_BehaviorEvent_BehaviorEventId] FOREIGN KEY ([BehaviorEventId]) REFERENCES [BehaviorEvent] ([Id]),
    CONSTRAINT [FK_SolTkData_BehaviorMessage_BehaviorMessageId] FOREIGN KEY ([BehaviorMessageId]) REFERENCES [BehaviorMessage] ([Id]),
    CONSTRAINT [FK_SolTkData_BehaviorState_BehaviorStateId] FOREIGN KEY ([BehaviorStateId]) REFERENCES [BehaviorState] ([Id]),
    CONSTRAINT [FK_SolTkData_BehaviorState_BehaviorStateId1] FOREIGN KEY ([BehaviorStateId1]) REFERENCES [BehaviorState] ([Id]),
    CONSTRAINT [FK_SolTkData_BehaviorState_BehaviorStateId2] FOREIGN KEY ([BehaviorStateId2]) REFERENCES [BehaviorState] ([Id]),
    CONSTRAINT [FK_SolTkData_BehaviorSystem_BehaviorSystemId] FOREIGN KEY ([BehaviorSystemId]) REFERENCES [BehaviorSystem] ([Id]),
    CONSTRAINT [FK_SolTkData_ControllerComponent_ControllerComponentId] FOREIGN KEY ([ControllerComponentId]) REFERENCES [ControllerComponent] ([Id]),
    CONSTRAINT [FK_SolTkData_MovableController_MovableControllerId] FOREIGN KEY ([MovableControllerId]) REFERENCES [MovableController] ([Id]),
    CONSTRAINT [FK_SolTkData_SoundSource_SoundSourceId] FOREIGN KEY ([SoundSourceId]) REFERENCES [SoundSource] ([Id]),
    CONSTRAINT [FK_SolTkData_StoryCards_StoryCardId] FOREIGN KEY ([StoryCardId]) REFERENCES [StoryCards] ([Id]),
    CONSTRAINT [FK_SolTkData_Tile_TileId] FOREIGN KEY ([TileId]) REFERENCES [Tile] ([Id])
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


CREATE INDEX [IX_BehaviorEvent_StoryCardId] ON [BehaviorEvent] ([StoryCardId]);
GO


CREATE INDEX [IX_BehaviorMessage_BehaviorEventId] ON [BehaviorMessage] ([BehaviorEventId]);
GO


CREATE INDEX [IX_BehaviorState_AnimationId] ON [BehaviorState] ([AnimationId]);
GO


CREATE INDEX [IX_BehaviorState_BehaviorStateId] ON [BehaviorState] ([BehaviorStateId]);
GO


CREATE INDEX [IX_BehaviorState_BehaviorSystemId] ON [BehaviorState] ([BehaviorSystemId]);
GO


CREATE INDEX [IX_BehaviorSystem_ResourceCollectionId] ON [BehaviorSystem] ([ResourceCollectionId]);
GO


CREATE INDEX [IX_ControllerComponent_MovableControllerId] ON [ControllerComponent] ([MovableControllerId]);
GO


CREATE INDEX [IX_Hud_ResourceCollectionId] ON [Hud] ([ResourceCollectionId]);
GO


CREATE INDEX [IX_HudDialog_ResourceCollectionId] ON [HudDialog] ([ResourceCollectionId]);
GO


CREATE INDEX [IX_HudMenu_ResourceCollectionId] ON [HudMenu] ([ResourceCollectionId]);
GO


CREATE INDEX [IX_ImmovableController_ResourceCollectionId] ON [ImmovableController] ([ResourceCollectionId]);
GO


CREATE INDEX [IX_ImmovableController_ResourceCollectionId1] ON [ImmovableController] ([ResourceCollectionId1]);
GO


CREATE INDEX [IX_ImmovableController_StaticObjectController_ResourceCollectionId1] ON [ImmovableController] ([StaticObjectController_ResourceCollectionId1]);
GO


CREATE INDEX [IX_MovableController_BehaviorSystemId] ON [MovableController] ([BehaviorSystemId]);
GO


CREATE INDEX [IX_MovableController_CharacterController_ResourceCollectionId1] ON [MovableController] ([CharacterController_ResourceCollectionId1]);
GO


CREATE INDEX [IX_MovableController_ResourceCollectionId] ON [MovableController] ([ResourceCollectionId]);
GO


CREATE INDEX [IX_MovableController_ResourceCollectionId1] ON [MovableController] ([ResourceCollectionId1]);
GO


CREATE INDEX [IX_MovableController_SoundSetId] ON [MovableController] ([SoundSetId]);
GO


CREATE INDEX [IX_MovableController_TransportController_ResourceCollectionId1] ON [MovableController] ([TransportController_ResourceCollectionId1]);
GO


CREATE INDEX [IX_SolTkCondition_BehaviorEventId] ON [SolTkCondition] ([BehaviorEventId]);
GO


CREATE INDEX [IX_SolTkCondition_BehaviorStateId] ON [SolTkCondition] ([BehaviorStateId]);
GO


CREATE INDEX [IX_SolTkCondition_StoryCardId] ON [SolTkCondition] ([StoryCardId]);
GO


CREATE INDEX [IX_SolTkData_BehaviorAnimationFrameId] ON [SolTkData] ([BehaviorAnimationFrameId]);
GO


CREATE INDEX [IX_SolTkData_BehaviorEventId] ON [SolTkData] ([BehaviorEventId]);
GO


CREATE INDEX [IX_SolTkData_BehaviorMessageId] ON [SolTkData] ([BehaviorMessageId]);
GO


CREATE INDEX [IX_SolTkData_BehaviorStateId] ON [SolTkData] ([BehaviorStateId]);
GO


CREATE INDEX [IX_SolTkData_BehaviorStateId1] ON [SolTkData] ([BehaviorStateId1]);
GO


CREATE INDEX [IX_SolTkData_BehaviorStateId2] ON [SolTkData] ([BehaviorStateId2]);
GO


CREATE INDEX [IX_SolTkData_BehaviorSystemId] ON [SolTkData] ([BehaviorSystemId]);
GO


CREATE INDEX [IX_SolTkData_ControllerComponentId] ON [SolTkData] ([ControllerComponentId]);
GO


CREATE INDEX [IX_SolTkData_MovableControllerId] ON [SolTkData] ([MovableControllerId]);
GO


CREATE INDEX [IX_SolTkData_SoundSourceId] ON [SolTkData] ([SoundSourceId]);
GO


CREATE INDEX [IX_SolTkData_StoryCardId] ON [SolTkData] ([StoryCardId]);
GO


CREATE INDEX [IX_SolTkData_TileId] ON [SolTkData] ([TileId]);
GO


CREATE INDEX [IX_SoundSet_ResourceCollectionId] ON [SoundSet] ([ResourceCollectionId]);
GO


CREATE INDEX [IX_SoundSource_ResourceCollectionId] ON [SoundSource] ([ResourceCollectionId]);
GO


CREATE INDEX [IX_SoundSource_SoundSetId] ON [SoundSource] ([SoundSetId]);
GO


CREATE INDEX [IX_StoryCards_TimelineId] ON [StoryCards] ([TimelineId]);
GO


CREATE INDEX [IX_Tile_TileSetId] ON [Tile] ([TileSetId]);
GO


CREATE INDEX [IX_TileRule_TileId] ON [TileRule] ([TileId]);
GO


CREATE INDEX [IX_TileSet_ResourceCollectionId] ON [TileSet] ([ResourceCollectionId]);
GO


