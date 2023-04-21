CREATE TABLE [AnimationData] (
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
    CONSTRAINT [PK_AnimationData] PRIMARY KEY ([Id])
);
GO


CREATE TABLE [Conditions] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NULL,
    [Description] nvarchar(max) NULL,
    [Tags] nvarchar(max) NULL,
    CONSTRAINT [PK_Conditions] PRIMARY KEY ([Id])
);
GO


CREATE TABLE [Systems] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NULL,
    [Description] nvarchar(max) NULL,
    [BehaviorType] int NOT NULL,
    [Tags] nvarchar(max) NULL,
    CONSTRAINT [PK_Systems] PRIMARY KEY ([Id])
);
GO


CREATE TABLE [Animations] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NULL,
    [Description] nvarchar(max) NULL,
    [Tags] nvarchar(max) NULL,
    [StartFrameDataId] int NULL,
    [ActFrameDataId] int NULL,
    [EndFrameDataId] int NULL,
    CONSTRAINT [PK_Animations] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Animations_AnimationData_ActFrameDataId] FOREIGN KEY ([ActFrameDataId]) REFERENCES [AnimationData] ([Id]),
    CONSTRAINT [FK_Animations_AnimationData_EndFrameDataId] FOREIGN KEY ([EndFrameDataId]) REFERENCES [AnimationData] ([Id]),
    CONSTRAINT [FK_Animations_AnimationData_StartFrameDataId] FOREIGN KEY ([StartFrameDataId]) REFERENCES [AnimationData] ([Id])
);
GO


CREATE TABLE [Frames] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NULL,
    [Description] nvarchar(max) NULL,
    [Tags] nvarchar(max) NULL,
    [Order] int NOT NULL,
    [Speed] real NOT NULL,
    [Duration] real NOT NULL,
    [FrameData] nvarchar(max) NULL,
    [BehaviorAnimationDataId] int NULL,
    CONSTRAINT [PK_Frames] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Frames_AnimationData_BehaviorAnimationDataId] FOREIGN KEY ([BehaviorAnimationDataId]) REFERENCES [AnimationData] ([Id])
);
GO


CREATE TABLE [States] (
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
    CONSTRAINT [PK_States] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_States_Animations_AnimationId] FOREIGN KEY ([AnimationId]) REFERENCES [Animations] ([Id]),
    CONSTRAINT [FK_States_States_BehaviorStateId] FOREIGN KEY ([BehaviorStateId]) REFERENCES [States] ([Id]),
    CONSTRAINT [FK_States_Systems_BehaviorSystemId] FOREIGN KEY ([BehaviorSystemId]) REFERENCES [Systems] ([Id])
);
GO


CREATE TABLE [Actions] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NULL,
    [Description] nvarchar(max) NULL,
    [StateId] uniqueidentifier NOT NULL,
    [StateId1] int NULL,
    [StartAction] nvarchar(max) NULL,
    [MainAction] nvarchar(max) NULL,
    [EndAction] nvarchar(max) NULL,
    [Tags] nvarchar(max) NULL,
    CONSTRAINT [PK_Actions] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Actions_States_StateId1] FOREIGN KEY ([StateId1]) REFERENCES [States] ([Id])
);
GO


CREATE TABLE [Events] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NULL,
    [Description] nvarchar(max) NULL,
    [Tags] nvarchar(max) NULL,
    [BehaviorStateId] int NULL,
    [BehaviorSystemId] int NULL,
    CONSTRAINT [PK_Events] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Events_States_BehaviorStateId] FOREIGN KEY ([BehaviorStateId]) REFERENCES [States] ([Id]),
    CONSTRAINT [FK_Events_Systems_BehaviorSystemId] FOREIGN KEY ([BehaviorSystemId]) REFERENCES [Systems] ([Id])
);
GO


CREATE TABLE [ConditionsData] (
    [Id] int NOT NULL IDENTITY,
    [Key] nvarchar(max) NULL,
    [Data] nvarchar(max) NULL,
    [Operator] int NOT NULL,
    [BehaviorConditionId] int NULL,
    [BehaviorEventId] int NULL,
    [BehaviorStateId] int NULL,
    CONSTRAINT [PK_ConditionsData] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ConditionsData_Conditions_BehaviorConditionId] FOREIGN KEY ([BehaviorConditionId]) REFERENCES [Conditions] ([Id]),
    CONSTRAINT [FK_ConditionsData_Events_BehaviorEventId] FOREIGN KEY ([BehaviorEventId]) REFERENCES [Events] ([Id]),
    CONSTRAINT [FK_ConditionsData_States_BehaviorStateId] FOREIGN KEY ([BehaviorStateId]) REFERENCES [States] ([Id])
);
GO


CREATE TABLE [Messages] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NULL,
    [Description] nvarchar(max) NULL,
    [Tags] nvarchar(max) NULL,
    [TargetName] nvarchar(max) NULL,
    [TargetType] int NOT NULL,
    [BehaviorEventId] int NULL,
    CONSTRAINT [PK_Messages] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Messages_Events_BehaviorEventId] FOREIGN KEY ([BehaviorEventId]) REFERENCES [Events] ([Id])
);
GO


CREATE TABLE [AttributeData] (
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
    CONSTRAINT [PK_AttributeData] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AttributeData_Events_BehaviorEventId] FOREIGN KEY ([BehaviorEventId]) REFERENCES [Events] ([Id]),
    CONSTRAINT [FK_AttributeData_Frames_BehaviorAnimationFrameId] FOREIGN KEY ([BehaviorAnimationFrameId]) REFERENCES [Frames] ([Id]),
    CONSTRAINT [FK_AttributeData_Messages_BehaviorMessageId] FOREIGN KEY ([BehaviorMessageId]) REFERENCES [Messages] ([Id]),
    CONSTRAINT [FK_AttributeData_States_BehaviorStateId] FOREIGN KEY ([BehaviorStateId]) REFERENCES [States] ([Id]),
    CONSTRAINT [FK_AttributeData_States_BehaviorStateId1] FOREIGN KEY ([BehaviorStateId1]) REFERENCES [States] ([Id]),
    CONSTRAINT [FK_AttributeData_States_BehaviorStateId2] FOREIGN KEY ([BehaviorStateId2]) REFERENCES [States] ([Id]),
    CONSTRAINT [FK_AttributeData_Systems_BehaviorSystemId] FOREIGN KEY ([BehaviorSystemId]) REFERENCES [Systems] ([Id])
);
GO


CREATE INDEX [IX_Actions_StateId1] ON [Actions] ([StateId1]);
GO


CREATE INDEX [IX_Animations_ActFrameDataId] ON [Animations] ([ActFrameDataId]);
GO


CREATE INDEX [IX_Animations_EndFrameDataId] ON [Animations] ([EndFrameDataId]);
GO


CREATE INDEX [IX_Animations_StartFrameDataId] ON [Animations] ([StartFrameDataId]);
GO


CREATE INDEX [IX_AttributeData_BehaviorAnimationFrameId] ON [AttributeData] ([BehaviorAnimationFrameId]);
GO


CREATE INDEX [IX_AttributeData_BehaviorEventId] ON [AttributeData] ([BehaviorEventId]);
GO


CREATE INDEX [IX_AttributeData_BehaviorMessageId] ON [AttributeData] ([BehaviorMessageId]);
GO


CREATE INDEX [IX_AttributeData_BehaviorStateId] ON [AttributeData] ([BehaviorStateId]);
GO


CREATE INDEX [IX_AttributeData_BehaviorStateId1] ON [AttributeData] ([BehaviorStateId1]);
GO


CREATE INDEX [IX_AttributeData_BehaviorStateId2] ON [AttributeData] ([BehaviorStateId2]);
GO


CREATE INDEX [IX_AttributeData_BehaviorSystemId] ON [AttributeData] ([BehaviorSystemId]);
GO


CREATE INDEX [IX_ConditionsData_BehaviorConditionId] ON [ConditionsData] ([BehaviorConditionId]);
GO


CREATE INDEX [IX_ConditionsData_BehaviorEventId] ON [ConditionsData] ([BehaviorEventId]);
GO


CREATE INDEX [IX_ConditionsData_BehaviorStateId] ON [ConditionsData] ([BehaviorStateId]);
GO


CREATE INDEX [IX_Events_BehaviorStateId] ON [Events] ([BehaviorStateId]);
GO


CREATE INDEX [IX_Events_BehaviorSystemId] ON [Events] ([BehaviorSystemId]);
GO


CREATE INDEX [IX_Frames_BehaviorAnimationDataId] ON [Frames] ([BehaviorAnimationDataId]);
GO


CREATE INDEX [IX_Messages_BehaviorEventId] ON [Messages] ([BehaviorEventId]);
GO


CREATE INDEX [IX_States_AnimationId] ON [States] ([AnimationId]);
GO


CREATE INDEX [IX_States_BehaviorStateId] ON [States] ([BehaviorStateId]);
GO


CREATE INDEX [IX_States_BehaviorSystemId] ON [States] ([BehaviorSystemId]);
GO


