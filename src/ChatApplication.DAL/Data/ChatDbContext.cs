using System.Linq.Expressions;
using ChatApplication.DAL.Configurations;
using ChatApplication.DAL.Entities;
using ChatApplication.DAL.Entities.Interfaces;
using ChatApplication.DAL.Functions.Results;
using ChatApplication.DAL.Views;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ChatApplication.DAL.Data;

public class ChatDbContext: IdentityDbContext<User>
{
    public DbSet<Chat> Chats { get; set; }
    public DbSet<ChatType> ChatTypes { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<UserChat> UserChats { get; set; }

    public DbSet<ChatView> ChatView { get; set; }
    public DbSet<UserView> UserView { get; set; }

    public IQueryable<MessageFuncResult> MessagesByChatIdFunc(int chatId)
        => FromExpression(() => MessagesByChatIdFunc(chatId));

    public IQueryable<MessageFuncResult> MessageByIdFunc(int id)
        => FromExpression(() => MessageByIdFunc(id));

    public IQueryable<ChatFuncResult> ChatsByUserIdFunc(string userId)
        => FromExpression(() => ChatsByUserIdFunc(userId));

    public ChatDbContext(DbContextOptions<ChatDbContext> options): base(options){}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        QueryFilter(modelBuilder);

        modelBuilder
            .HasDbFunction(typeof(ChatDbContext)
                .GetMethod(nameof(MessagesByChatIdFunc))!)
            .HasName("GetMessagesByChatIdFunction");

        modelBuilder
            .HasDbFunction(typeof(ChatDbContext)
                .GetMethod(nameof(MessageByIdFunc))!)
            .HasName("GetMessageByIdFunction");

        modelBuilder
            .HasDbFunction(typeof(ChatDbContext)
                .GetMethod(nameof(ChatsByUserIdFunc))!)
            .HasName("GetChatsByUserIdFunction");

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserChatConfiguration).Assembly);

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