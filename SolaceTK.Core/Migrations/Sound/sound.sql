CREATE TABLE [SoundConditions] (
    [Id] int NOT NULL IDENTITY,
    [Key] nvarchar(max) NULL,
    [Data] nvarchar(max) NULL,
    [Operator] int NOT NULL,
    CONSTRAINT [PK_SoundConditions] PRIMARY KEY ([Id])
);
GO


CREATE TABLE [SoundSets] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NULL,
    [Description] nvarchar(max) NULL,
    [Tags] nvarchar(max) NULL,
    CONSTRAINT [PK_SoundSets] PRIMARY KEY ([Id])
);
GO


CREATE TABLE [SoundSources] (
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
    CONSTRAINT [PK_SoundSources] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_SoundSources_SoundSets_SoundSetId] FOREIGN KEY ([SoundSetId]) REFERENCES [SoundSets] ([Id])
);
GO


CREATE TABLE [SoundData] (
    [Id] int NOT NULL IDENTITY,
    [Key] nvarchar(max) NULL,
    [Data] nvarchar(max) NULL,
    [Operator] int NOT NULL,
    [SoundSourceId] int NULL,
    CONSTRAINT [PK_SoundData] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_SoundData_SoundSources_SoundSourceId] FOREIGN KEY ([SoundSourceId]) REFERENCES [SoundSources] ([Id])
);
GO


CREATE INDEX [IX_SoundData_SoundSourceId] ON [SoundData] ([SoundSourceId]);
GO


CREATE INDEX [IX_SoundSources_SoundSetId] ON [SoundSources] ([SoundSetId]);
GO


