namespace ChatApplication.DAL.Entities.Functions;

public class ChatFunction
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int ChatTypeId { get; set; }
    public int MembersCount { get; set; }
}