CREATE TABLE [dbo].[Quests] (
    [Id]          INT           IDENTITY (1, 1) NOT NULL,
    [TypeId]      INT           NOT NULL,
    [RarityId]    INT           NOT NULL,
    [StatusId]    INT           NULL,
    [IsEnabled]   BIT           DEFAULT ((1)) NULL,
    [Title]       VARCHAR (50)  NOT NULL,
    [Text]        VARCHAR (MAX) NOT NULL,
    [TimeInHours] INT           DEFAULT ((24)) NOT NULL,
    [Sectcoins]   INT           DEFAULT ((0)) NULL,
    [SectPoints]  INT           DEFAULT ((0)) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FKType] FOREIGN KEY ([TypeId]) REFERENCES [dbo].[Types] ([Id]),
    CONSTRAINT [FKRarity] FOREIGN KEY ([RarityId]) REFERENCES [dbo].[Rarities] ([Id]),
    CONSTRAINT [FKQuestStatus] FOREIGN KEY ([StatusId]) REFERENCES [dbo].[Statuses] ([Id]),
);

