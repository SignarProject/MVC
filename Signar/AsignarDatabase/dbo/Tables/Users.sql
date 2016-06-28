CREATE TABLE [dbo].[Users] (
    [UserID]          INT            IDENTITY (1, 1) NOT NULL,
    [Name]            NVARCHAR (35)  NOT NULL,
    [Surname]         NVARCHAR (35)  NOT NULL,
    [Email]           NVARCHAR (254) NOT NULL,
    [AvatarImagePath] NVARCHAR (MAX) NULL,
    [Login]           NVARCHAR (254) NOT NULL,
    [Password]        NVARCHAR (254) NOT NULL,
    [IsAdmin]         BIT            NOT NULL,
    [Version]         ROWVERSION     NOT NULL,
    PRIMARY KEY CLUSTERED ([UserID] ASC)
);

