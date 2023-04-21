CREATE TABLE [Artifacts] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NULL,
    [Description] nvarchar(max) NULL,
    [Tags] nvarchar(max) NULL,
    [ArtifactUrl] nvarchar(max) NULL,
    [Created] datetimeoffset NOT NULL,
    [Updated] datetimeoffset NOT NULL,
    CONSTRAINT [PK_Artifacts] PRIMARY KEY ([Id])
);
GO


CREATE TABLE [Projects] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NULL,
    [Description] nvarchar(max) NULL,
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
GO


CREATE TABLE [Payments] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NULL,
    [Description] nvarchar(max) NULL,
    [Tags] nvarchar(max) NULL,
    [ProjectId] int NULL,
    [PaymentDate] datetimeoffset NOT NULL,
    [Amount] real NOT NULL,
    [Created] datetimeoffset NOT NULL,
    [Updated] datetimeoffset NOT NULL,
    CONSTRAINT [PK_Payments] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Payments_Projects_ProjectId] FOREIGN KEY ([ProjectId]) REFERENCES [Projects] ([Id])
);
GO


CREATE TABLE [WorkItems] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NULL,
    [Description] nvarchar(max) NULL,
    [Tags] nvarchar(max) NULL,
    [HoursEstimate] real NOT NULL,
    [HoursActual] real NOT NULL,
    [PaymentId] int NULL,
    [IsPaid] bit NOT NULL,
    [WorkProjectId] uniqueidentifier NOT NULL,
    [ArtifactId] int NULL,
    [Created] datetimeoffset NOT NULL,
    [Updated] datetimeoffset NOT NULL,
    [WorkProjectId1] int NULL,
    CONSTRAINT [PK_WorkItems] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_WorkItems_Artifacts_ArtifactId] FOREIGN KEY ([ArtifactId]) REFERENCES [Artifacts] ([Id]),
    CONSTRAINT [FK_WorkItems_Payments_PaymentId] FOREIGN KEY ([PaymentId]) REFERENCES [Payments] ([Id]),
    CONSTRAINT [FK_WorkItems_Projects_WorkProjectId1] FOREIGN KEY ([WorkProjectId1]) REFERENCES [Projects] ([Id])
);
GO


CREATE TABLE [Comments] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NULL,
    [Description] nvarchar(max) NULL,
    [Tags] nvarchar(max) NULL,
    [Comment] nvarchar(max) NULL,
    [Created] datetimeoffset NOT NULL,
    [Updated] datetimeoffset NOT NULL,
    [WorkItemId] int NULL,
    [WorkProjectId] int NULL,
    CONSTRAINT [PK_Comments] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Comments_Projects_WorkProjectId] FOREIGN KEY ([WorkProjectId]) REFERENCES [Projects] ([Id]),
    CONSTRAINT [FK_Comments_WorkItems_WorkItemId] FOREIGN KEY ([WorkItemId]) REFERENCES [WorkItems] ([Id])
);
GO


CREATE INDEX [IX_Comments_WorkItemId] ON [Comments] ([WorkItemId]);
GO


CREATE INDEX [IX_Comments_WorkProjectId] ON [Comments] ([WorkProjectId]);
GO


CREATE INDEX [IX_Payments_ProjectId] ON [Payments] ([ProjectId]);
GO


CREATE INDEX [IX_WorkItems_ArtifactId] ON [WorkItems] ([ArtifactId]);
GO


CREATE INDEX [IX_WorkItems_PaymentId] ON [WorkItems] ([PaymentId]);
GO


CREATE INDEX [IX_WorkItems_WorkProjectId1] ON [WorkItems] ([WorkProjectId1]);
GO


