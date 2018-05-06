CREATE TABLE [dbo].[UserQuests] (
    [Id]      INT IDENTITY (1, 1) NOT NULL,
    [QuestId] INT NOT NULL,
    [UserId]  INT NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FKQuest] FOREIGN KEY ([QuestId]) REFERENCES [dbo].[Quests] ([Id]),
    CONSTRAINT [FKUserQuest] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id])
);

