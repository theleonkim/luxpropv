CREATE TABLE [dbo].[ChatMessages] (
    [ChatMessageId] INT            IDENTITY (1, 1) NOT NULL,
    [ChatThreadId]  INT            NOT NULL,
    [Sender]        NVARCHAR (20)  NOT NULL,
    [Text]          NVARCHAR (MAX) NOT NULL,
    [SentUtc]       DATETIME       DEFAULT (getutcdate()) NOT NULL,
    [UsuarioId]     INT            NULL,
    PRIMARY KEY CLUSTERED ([ChatMessageId] ASC),
    CONSTRAINT [FK_ChatMessages_ChatThreads] FOREIGN KEY ([ChatThreadId]) REFERENCES [dbo].[ChatThreads] ([ChatThreadId]) ON DELETE CASCADE
);

