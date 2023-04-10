namespace ChatApplication.DAL.Entities;

public abstract class AuditableEntity: SoftDeletableEntity
{
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
}