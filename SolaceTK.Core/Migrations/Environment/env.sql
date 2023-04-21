CREATE TABLE [TileSets] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NULL,
    [Description] nvarchar(max) NULL,
    [Tags] nvarchar(max) NULL,
    CONSTRAINT [PK_TileSets] PRIMARY KEY ([Id])
);
GO


CREATE TABLE [Maps] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NULL,
    [Description] nvarchar(max) NULL,
    [Tags] nvarchar(max) NULL,
    [TileSetId] int NULL,
    CONSTRAINT [PK_Maps] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Maps_TileSets_TileSetId] FOREIGN KEY ([TileSetId]) REFERENCES [TileSets] ([Id])
);
GO


CREATE TABLE [Tiles] (
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
    CONSTRAINT [PK_Tiles] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Tiles_TileSets_TileSetId] FOREIGN KEY ([TileSetId]) REFERENCES [TileSets] ([Id])
);
GO


CREATE TABLE [Chunks] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NULL,
    [Description] nvarchar(max) NULL,
    [Tags] nvarchar(max) NULL,
    [ColorKey] nvarchar(max) NULL,
    [MapId] int NULL,
    CONSTRAINT [PK_Chunks] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Chunks_Maps_MapId] FOREIGN KEY ([MapId]) REFERENCES [Maps] ([Id])
);
GO


CREATE TABLE [Layers] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NULL,
    [Description] nvarchar(max) NULL,
    [Tags] nvarchar(max) NULL,
    [Enabled] bit NOT NULL,
    [LayerName] nvarchar(max) NULL,
    [SortingLayer] nvarchar(max) NULL,
    [LayerOrder] int NOT NULL,
    [IsCollidable] bit NOT NULL,
    [IsBreakable] bit NOT NULL,
    [MapId] int NULL,
    CONSTRAINT [PK_Layers] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Layers_Maps_MapId] FOREIGN KEY ([MapId]) REFERENCES [Maps] ([Id])
);
GO


CREATE TABLE [TileRules] (
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
    CONSTRAINT [PK_TileRules] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_TileRules_Tiles_TileId] FOREIGN KEY ([TileId]) REFERENCES [Tiles] ([Id])
);
GO


CREATE TABLE [Cells] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NULL,
    [Description] nvarchar(max) NULL,
    [Tags] nvarchar(max) NULL,
    [X] int NOT NULL,
    [Y] int NOT NULL,
    [ColorKey] nvarchar(max) NULL,
    [Enabled] bit NOT NULL,
    [MapChunkId] int NULL,
    [MapId] int NULL,
    CONSTRAINT [PK_Cells] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Cells_Chunks_MapChunkId] FOREIGN KEY ([MapChunkId]) REFERENCES [Chunks] ([Id]),
    CONSTRAINT [FK_Cells_Maps_MapId] FOREIGN KEY ([MapId]) REFERENCES [Maps] ([Id])
);
GO


CREATE TABLE [BehaviorEvent] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NULL,
    [Description] nvarchar(max) NULL,
    [Tags] nvarchar(max) NULL,
    [MapCellId] int NULL,
    [MapCellId1] int NULL,
    [MapCellId2] int NULL,
    CONSTRAINT [PK_BehaviorEvent] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_BehaviorEvent_Cells_MapCellId] FOREIGN KEY ([MapCellId]) REFERENCES [Cells] ([Id]),
    CONSTRAINT [FK_BehaviorEvent_Cells_MapCellId1] FOREIGN KEY ([MapCellId1]) REFERENCES [Cells] ([Id]),
    CONSTRAINT [FK_BehaviorEvent_Cells_MapCellId2] FOREIGN KEY ([MapCellId2]) REFERENCES [Cells] ([Id])
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
    CONSTRAINT [PK_SolTkCondition] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_SolTkCondition_BehaviorEvent_BehaviorEventId] FOREIGN KEY ([BehaviorEventId]) REFERENCES [BehaviorEvent] ([Id])
);
GO


CREATE TABLE [EnvironmentData] (
    [Id] int NOT NULL IDENTITY,
    [Key] nvarchar(max) NULL,
    [Data] nvarchar(max) NULL,
    [Operator] int NOT NULL,
    [BehaviorEventId] int NULL,
    [BehaviorMessageId] int NULL,
    [MapCellId] int NULL,
    [MapCellId1] int NULL,
    [MapCellId2] int NULL,
    [MapLayerId] int NULL,
    [TileId] int NULL,
    CONSTRAINT [PK_EnvironmentData] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_EnvironmentData_BehaviorEvent_BehaviorEventId] FOREIGN KEY ([BehaviorEventId]) REFERENCES [BehaviorEvent] ([Id]),
    CONSTRAINT [FK_EnvironmentData_BehaviorMessage_BehaviorMessageId] FOREIGN KEY ([BehaviorMessageId]) REFERENCES [BehaviorMessage] ([Id]),
    CONSTRAINT [FK_EnvironmentData_Cells_MapCellId] FOREIGN KEY ([MapCellId]) REFERENCES [Cells] ([Id]),
    CONSTRAINT [FK_EnvironmentData_Cells_MapCellId1] FOREIGN KEY ([MapCellId1]) REFERENCES [Cells] ([Id]),
    CONSTRAINT [FK_EnvironmentData_Cells_MapCellId2] FOREIGN KEY ([MapCellId2]) REFERENCES [Cells] ([Id]),
    CONSTRAINT [FK_EnvironmentData_Layers_MapLayerId] FOREIGN KEY ([MapLayerId]) REFERENCES [Layers] ([Id]),
    CONSTRAINT [FK_EnvironmentData_Tiles_TileId] FOREIGN KEY ([TileId]) REFERENCES [Tiles] ([Id])
);
GO


CREATE INDEX [IX_BehaviorEvent_MapCellId] ON [BehaviorEvent] ([MapCellId]);
GO


CREATE INDEX [IX_BehaviorEvent_MapCellId1] ON [BehaviorEvent] ([MapCellId1]);
GO


CREATE INDEX [IX_BehaviorEvent_MapCellId2] ON [BehaviorEvent] ([MapCellId2]);
GO


CREATE INDEX [IX_BehaviorMessage_BehaviorEventId] ON [BehaviorMessage] ([BehaviorEventId]);
GO


CREATE INDEX [IX_Cells_MapChunkId] ON [Cells] ([MapChunkId]);
GO


CREATE INDEX [IX_Cells_MapId] ON [Cells] ([MapId]);
GO


CREATE INDEX [IX_Chunks_MapId] ON [Chunks] ([MapId]);
GO


CREATE INDEX [IX_EnvironmentData_BehaviorEventId] ON [EnvironmentData] ([BehaviorEventId]);
GO


CREATE INDEX [IX_EnvironmentData_BehaviorMessageId] ON [EnvironmentData] ([BehaviorMessageId]);
GO


CREATE INDEX [IX_EnvironmentData_MapCellId] ON [EnvironmentData] ([MapCellId]);
GO


CREATE INDEX [IX_EnvironmentData_MapCellId1] ON [EnvironmentData] ([MapCellId1]);
GO


CREATE INDEX [IX_EnvironmentData_MapCellId2] ON [EnvironmentData] ([MapCellId2]);
GO


CREATE INDEX [IX_EnvironmentData_MapLayerId] ON [EnvironmentData] ([MapLayerId]);
GO


CREATE INDEX [IX_EnvironmentData_TileId] ON [EnvironmentData] ([TileId]);
GO


CREATE INDEX [IX_Layers_MapId] ON [Layers] ([MapId]);
GO


CREATE INDEX [IX_Maps_TileSetId] ON [Maps] ([TileSetId]);
GO


CREATE INDEX [IX_SolTkCondition_BehaviorEventId] ON [SolTkCondition] ([BehaviorEventId]);
GO


CREATE INDEX [IX_TileRules_TileId] ON [TileRules] ([TileId]);
GO


CREATE INDEX [IX_Tiles_TileSetId] ON [Tiles] ([TileSetId]);
GO


