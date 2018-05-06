CREATE TABLE [dbo].[UserStatuses] (
    [Id]       INT NOT NULL,
    [UserId]   INT NOT NULL,
    [StatusId] INT NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FKUserStatus] FOREIGN KEY ([StatusId]) REFERENCES [dbo].[Statuses] ([Id]),
    CONSTRAINT [FKUser] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id])
);

