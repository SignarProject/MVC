CREATE TABLE [dbo].[Projects] (
    [ProjectID] INT            IDENTITY (1, 1) NOT NULL,
    [Name]      NVARCHAR (254) NOT NULL,
    [Prefix]    NVARCHAR (10)  NOT NULL,
    [IsDeleted] BIT            NOT NULL,
    [Version]   ROWVERSION     NOT NULL,
    PRIMARY KEY CLUSTERED ([ProjectID] ASC)
);

