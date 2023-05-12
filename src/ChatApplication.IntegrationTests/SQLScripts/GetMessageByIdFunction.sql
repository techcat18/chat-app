CREATE FUNCTION [dbo].[GetMessageByIdFunction]
(
    @id int
)
RETURNS TABLE
AS
RETURN
    SELECT 
        m.Id AS Id, 
        m.ParentMessageId AS ParentMessageId, 
        m.SenderId AS SenderId, 
        m.ChatId AS ChatId, 
        m.Content AS Content, 
        m.DateSent AS DateSent,
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
        LEFT JOIN Chats c ON m.ChatId = c.Id
        LEFT JOIN UserChats uc ON c.Id = uc.ChatId
        LEFT JOIN AspNetUsers u ON uc.UserId = u.Id
    WHERE 
        m.Id = @id
    GROUP BY
        m.Id,
        m.ParentMessageId,
        m.SenderId,
        m.ChatId,
        m.Content,
        m.DateSent