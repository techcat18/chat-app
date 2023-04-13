using ChatApplication.DAL.Entities.Interfaces;

namespace ChatApplication.DAL.Entities;

public class ChatType: ISoftDeletableEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsDeleted { get; set; }
}