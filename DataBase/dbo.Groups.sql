CREATE TABLE [dbo].[Groups] (
    [Id]               INT           IDENTITY (1, 1) NOT NULL,
    [Name]             VARCHAR (70)  NOT NULL,
    [LastMesageUserId] INT           NOT NULL,
    [LastMesageText]   VARCHAR (MAX) NOT NULL,
    [LastMesageDate]   VARCHAR (MAX) NOT NULL,
    [LastMesageTime]   VARCHAR (MAX) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FKGroupUserId] FOREIGN KEY ([LastMesageUserId]) REFERENCES [dbo].[Users] ([Id])
);

