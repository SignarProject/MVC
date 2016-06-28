CREATE TABLE [dbo].[Attachments] (
    [AttachmentID] INT            IDENTITY (1, 1) NOT NULL,
    [BugID]        INT            NOT NULL,
    [Name]         NVARCHAR (255) NOT NULL,
    [ContentPath]  NVARCHAR (MAX) NULL,
    [Version]      ROWVERSION     NOT NULL,
    PRIMARY KEY CLUSTERED ([AttachmentID] ASC),
    FOREIGN KEY ([BugID]) REFERENCES [dbo].[Bugs] ([BugID])
);

