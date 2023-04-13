using ChatApplication.DAL.Entities.Interfaces;

namespace ChatApplication.DAL.Entities;

public class Chat: IAuditableEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ChatType ChatType { get; set; }
    public int ChatTypeId { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
}