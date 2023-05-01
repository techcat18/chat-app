namespace ChatApplication.BLL.Models.User;

public class UpdateUserModel
{
    public string Id { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int? Age { get; set; }
    public byte[]? ImageBytes { get; set; }
}