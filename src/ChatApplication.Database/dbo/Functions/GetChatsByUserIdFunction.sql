CREATE FUNCTION [dbo].[GetChatsByUserIdFunction]
(
    @id varchar(1000)
)
RETURNS TABLE
AS
RETURN
	SELECT 
		c.Id,
		c.Name,
		ct.Id AS ChatTypeId, 
		(SELECT COUNT(*) FROM [UserChats] uc WHERE uc.ChatId = c.Id) AS MembersCount
		FROM [Chats] c
	INNER JOIN [ChatTypes] ct ON c.ChatTypeId = ct.Id
	INNER JOIN [UserChats] uc ON c.Id = uc.ChatId
	INNER JOIN [AspNetUsers] u ON uc.UserId = u.Id
	WHERE u.Id = @id
	GROUP BY c.Id, c.Name, ct.Id
	
