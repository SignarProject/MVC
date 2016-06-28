CREATE TABLE [dbo].[Comments] (
    [CommentID]        INT            IDENTITY (1, 1) NOT NULL,
    [Text]             NVARCHAR (MAX) NULL,
    [CreationDate]     DATETIME       NOT NULL,
    [ModificationDate] DATETIME       NULL,
    [UserID]           INT            NULL,
    [BugID]            INT            NOT NULL,
    [Version]          ROWVERSION     NOT NULL,
    PRIMARY KEY CLUSTERED ([CommentID] ASC),
    FOREIGN KEY ([BugID]) REFERENCES [dbo].[Bugs] ([BugID]),
    FOREIGN KEY ([UserID]) REFERENCES [dbo].[Users] ([UserID])
);

