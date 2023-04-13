CREATE TABLE [dbo].[Messages]
(
	[MessageId] INT NOT NULL PRIMARY KEY,
	[ParentMessageId] INT NULL,
	[SenderId] NVARCHAR(450) NOT NULL,
	[ChatId] INT NOT NULL,
	[Content] NVARCHAR(2000) NOT NULL,
	[DateSent] DATE NOT NULL,
	FOREIGN KEY ([ParentMessageId]) REFERENCES [Messages] ([MessageId]),
	FOREIGN KEY ([SenderId]) REFERENCES [AspNetUsers] ([Id]),
	FOREIGN KEY ([ChatId]) REFERENCES [Chats] ([Id])
)
