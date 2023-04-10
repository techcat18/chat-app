using ChatApplication.DAL.Configurations;
using ChatApplication.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChatApplication.DAL.Data;

public class ChatDbContext: DbContext
{
    public DbSet<GroupChat> GroupChats { get; set; }
    
    public ChatDbContext(DbContextOptions<ChatDbContext> options): base(options){}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(GroupChatConfiguration).Assembly);
        
        base.OnModelCreating(modelBuilder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        AuditEntities();
        HandleSoftDelete();

        return base.SaveChangesAsync(cancellationToken);
    }
    
    private void AuditEntities()
    {
        var entries = ChangeTracker
            .Entries()
            .Where(e => e.Entity is AuditableEntity &&
                        (e.State == EntityState.Added ||
                         e.State == EntityState.Modified));

        foreach (var entityEntry in entries)
        {
            var auditEntity = (AuditableEntity)entityEntry.Entity;
            auditEntity.ModifiedAt = DateTime.UtcNow;

            if (entityEntry.State == EntityState.Added)
            {
                auditEntity.CreatedAt = DateTime.UtcNow;
            }
        }
    }

    private void HandleSoftDelete()
    {
        var entries = ChangeTracker
            .Entries()
            .Where(e => e.Entity is AuditableEntity &&
                        e.State == EntityState.Deleted);

        foreach (var entityEntry in entries)
        {
            var auditEntity = (AuditableEntity)entityEntry.Entity;
            auditEntity.IsDeleted = true;
            entityEntry.State = EntityState.Modified;
        }
    }
}