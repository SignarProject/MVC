CREATE TABLE [dbo].[UsersToProjects] (
    [UserID]    INT        NOT NULL,
    [ProjectID] INT        NOT NULL,
    [Version]   ROWVERSION NOT NULL,
    CONSTRAINT [UserProject] PRIMARY KEY NONCLUSTERED ([UserID] ASC, [ProjectID] ASC),
    FOREIGN KEY ([ProjectID]) REFERENCES [dbo].[Projects] ([ProjectID]),
    FOREIGN KEY ([UserID]) REFERENCES [dbo].[Users] ([UserID])
);

