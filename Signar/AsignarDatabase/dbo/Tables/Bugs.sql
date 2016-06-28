CREATE TABLE [dbo].[Bugs] (
    [BugID]            INT            IDENTITY (1, 1) NOT NULL,
    [Subject]          NVARCHAR (254) NOT NULL,
    [Description]      NVARCHAR (MAX) NOT NULL,
    [AssigneeID]       INT            NULL,
    [ProjectID]        INT            NOT NULL,
    [CreationDate]     DATETIME       NOT NULL,
    [ModificationDate] DATETIME       NOT NULL,
    [BugStatus]        TINYINT        NOT NULL,
    [Priority]         TINYINT        NOT NULL,
    [Version]          ROWVERSION     NOT NULL,
    PRIMARY KEY CLUSTERED ([BugID] ASC),
    FOREIGN KEY ([AssigneeID]) REFERENCES [dbo].[Users] ([UserID]),
    FOREIGN KEY ([ProjectID]) REFERENCES [dbo].[Projects] ([ProjectID])
);

