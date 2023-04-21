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

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220903013442_LatestObjects')
BEGIN
    CREATE TABLE [SoundSets] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(max) NULL,
        [Description] nvarchar(max) NULL,
        [AssemblyType] nvarchar(max) NULL,
        [Tags] nvarchar(max) NULL,
        CONSTRAINT [PK_SoundSets] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220903013442_LatestObjects')
BEGIN
    CREATE TABLE [SoundSources] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(max) NULL,
        [Description] nvarchar(max) NULL,
        [IsLoop] bit NOT NULL,
        [Volume] real NOT NULL,
        [Pitch] real NOT NULL,
        [PlayOnLoad] bit NOT NULL,
        [ClipName] nvarchar(max) NULL,
        [LoopStartTime] datetime2 NOT NULL,
        [LoopEndTime] datetime2 NOT NULL,
        [AssemblyType] nvarchar(max) NULL,
        [Tags] nvarchar(max) NULL,
        [SoundSetId] uniqueidentifier NULL,
        CONSTRAINT [PK_SoundSources] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_SoundSources_SoundSets_SoundSetId] FOREIGN KEY ([SoundSetId]) REFERENCES [SoundSets] ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220903013442_LatestObjects')
BEGIN
    CREATE INDEX [IX_SoundSources_SoundSetId] ON [SoundSources] ([SoundSetId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220903013442_LatestObjects')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20220903013442_LatestObjects', N'6.0.8');
END;
GO

COMMIT;
GO

