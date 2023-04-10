namespace ChatApplication.DAL.Entities;

public class SoftDeletableEntity: BaseEntity
{
    public bool IsDeleted { get; set; }
}