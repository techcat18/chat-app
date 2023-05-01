CREATE VIEW [dbo].[ChatView]
	AS SELECT c.Id, c.Name, ct.Id AS ChatTypeId, COUNT(u.Id) AS MembersCount FROM [Chats] c
	INNER JOIN [ChatTypes] ct ON c.ChatTypeId = ct.Id
	INNER JOIN [UserChats] uc ON c.Id = uc.ChatId
	INNER JOIN [AspNetUsers] u ON uc.UserId = u.Id
	GROUP BY c.Id, c.Name, ct.Id
	
