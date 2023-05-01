CREATE FUNCTION [dbo].[GetMessagesByChatIdFunction]
(
    @chatId int
)
RETURNS TABLE
AS
RETURN
    SELECT 
        MAX(m.Id) AS Id, 
        MAX(m.ParentMessageId) AS ParentMessageId, 
        MAX(m.SenderId) AS SenderId, 
        MAX(m.ChatId) AS ChatId, 
        MAX(m.Content) AS Content, 
        MAX(m.DateSent) AS DateSent,
        (
            SELECT MAX(u2.Email)
            FROM AspNetUsers u2
            WHERE u2.Id = MAX(m.SenderId)
        ) AS SenderEmail,
        (
            SELECT MAX(u2.Image)
            FROM AspNetUsers u2
            WHERE u2.Id = MAX(m.SenderId) AND u2.Image IS NOT NULL
        ) AS SenderProfilePicture
    FROM 
        Messages m
        INNER JOIN Chats c ON m.ChatId = c.Id
        INNER JOIN UserChats uc ON c.Id = uc.ChatId
        INNER JOIN AspNetUsers u ON uc.UserId = u.Id
    WHERE 
        c.Id = @chatId
    GROUP BY 
        m.Id, 
        m.ParentMessageId, 
        m.SenderId, 
        m.ChatId, 
        m.Content, 
        m.DateSent
