CREATE VIEW [dbo].[UserView]
	AS SELECT u.Id, u.Email, u.FirstName, u.LastName, u.Image, u.Age FROM [AspNetUsers] u