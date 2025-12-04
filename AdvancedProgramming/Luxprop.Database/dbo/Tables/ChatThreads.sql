CREATE TABLE [dbo].[ChatThreads] (
    [ChatThreadId]    INT            IDENTITY (1, 1) NOT NULL,
    [State]           NVARCHAR (20)  DEFAULT ('Open') NOT NULL,
    [ClientName]      NVARCHAR (150) NULL,
    [ClientEmail]     NVARCHAR (200) NULL,
    [CreatedUtc]      DATETIME       DEFAULT (getutcdate()) NOT NULL,
    [ClosedUtc]       DATETIME       NULL,
    [NeedsAgent]      BIT            DEFAULT ((0)) NOT NULL,
    [CreatedByUserId] INT            NULL,
    PRIMARY KEY CLUSTERED ([ChatThreadId] ASC)
);

