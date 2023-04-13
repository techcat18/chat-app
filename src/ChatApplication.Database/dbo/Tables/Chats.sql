CREATE TABLE [dbo].[Chats]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[Name] NVARCHAR(100) NOT NULL,
	[ChatTypeId] INT NOT NULL,
	[CreatedAt] DATE NULL,
	[ModifiedAt] DATE NULL,
	[IsDeleted] BIT,
	FOREIGN KEY ([ChatTypeId]) REFERENCES [ChatTypes] ([Id])
)
