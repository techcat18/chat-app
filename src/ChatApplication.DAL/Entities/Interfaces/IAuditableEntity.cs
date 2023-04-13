namespace ChatApplication.DAL.Entities.Interfaces;

public interface IAuditableEntity: ISoftDeletableEntity
{
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
}