CREATE TABLE [dbo].[Filters] (
    [FilterID]      INT            IDENTITY (1, 1) NOT NULL,
    [UserID]        INT            NOT NULL,
    [Title]         NVARCHAR (254) NULL,
    [FilterContent] NVARCHAR (MAX) NULL,
    [Version]       ROWVERSION     NOT NULL,
    PRIMARY KEY CLUSTERED ([FilterID] ASC),
    FOREIGN KEY ([UserID]) REFERENCES [dbo].[Users] ([UserID])
);

