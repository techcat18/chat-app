namespace ChatApplication.DAL.Entities.Interfaces;

public interface ISoftDeletableEntity: IBaseEntity
{
    public bool IsDeleted { get; set; }
}