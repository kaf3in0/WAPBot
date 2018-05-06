CREATE TABLE [dbo].[Users] (
    [Id]          INT          IDENTITY (1, 1) NOT NULL,
    [PhoneNumber] VARCHAR (12) NOT NULL,
    [Sectcoins]   INT          NULL,
    [Sectpoints]  INT          NULL,
    [FirstName]   VARCHAR (30) NULL,
    [LastName]    VARCHAR (30) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

