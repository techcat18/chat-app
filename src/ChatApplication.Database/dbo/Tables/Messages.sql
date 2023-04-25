CREATE TABLE [dbo].[Messages]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[ParentMessageId] INT NULL,
	[SenderId] NVARCHAR(450) NOT NULL,
	[ChatId] INT NOT NULL,
	[Content] NVARCHAR(2000) NOT NULL,
	[DateSent] DATETIME NOT NULL,
	[CreatedAt] DATETIME NULL,
	[ModifiedAt] DATETIME NULL,
	[IsDeleted] BIT,
	FOREIGN KEY ([ParentMessageId]) REFERENCES [Messages] ([Id]),
	FOREIGN KEY ([SenderId]) REFERENCES [AspNetUsers] ([Id]),
	FOREIGN KEY ([ChatId]) REFERENCES [Chats] ([Id])
)
