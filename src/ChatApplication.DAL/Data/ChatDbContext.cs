using System.Linq.Expressions;
using ChatApplication.DAL.Entities;
using ChatApplication.DAL.Entities.Interfaces;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ChatApplication.DAL.Data;

public class ChatDbContext: IdentityDbContext<User>
{
    public DbSet<Chat> Chats { get; set; }
    public DbSet<ChatType> ChatTypes { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<UserChat> UserChats { get; set; }
    
    public ChatDbContext(DbContextOptions<ChatDbContext> options): base(options){}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        QueryFilter(modelBuilder);
        
        modelBuilder.Entity<UserChat>()
            .HasKey(uc => new { uc.UserId, uc.ChatId });

        base.OnModelCreating(modelBuilder);
    }

    private void QueryFilter(ModelBuilder modelBuilder)
    {
        var softDeleteEntities = typeof(ISoftDeletableEntity).Assembly.GetTypes()
            .Where(type => typeof(ISoftDeletableEntity)
                               .IsAssignableFrom(type)
                           && type.IsClass
                           && !type.IsAbstract);

        foreach (var softDeleteEntity in softDeleteEntities)
        {
            modelBuilder.Entity(softDeleteEntity)
                .HasQueryFilter(
                    GenerateQueryFilterLambdaExpression(softDeleteEntity)
                );
        }
    }
    
    private LambdaExpression GenerateQueryFilterLambdaExpression(Type type)
    {
        var parameter = Expression.Parameter(type, "e");
        var falseConstant = Expression.Constant(false);
        var propertyAccess = Expression.PropertyOrField(parameter, nameof(ISoftDeletableEntity.IsDeleted));
        var equalExpression = Expression.Equal(propertyAccess, falseConstant);
        var lambda = Expression.Lambda(equalExpression, parameter);

        return lambda;
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
            .Where(e => e.Entity is IAuditableEntity &&
                        (e.State == EntityState.Added ||
                         e.State == EntityState.Modified));

        foreach (var entityEntry in entries)
        {
            var auditEntity = (IAuditableEntity)entityEntry.Entity;
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
            .Where(e => e.Entity is IAuditableEntity &&
                        e.State == EntityState.Deleted);

        foreach (var entityEntry in entries)
        {
            var auditEntity = (IAuditableEntity)entityEntry.Entity;
            auditEntity.IsDeleted = true;
            entityEntry.State = EntityState.Modified;
        }
    }
}