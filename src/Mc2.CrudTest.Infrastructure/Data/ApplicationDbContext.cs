using Microsoft.EntityFrameworkCore;
using Mc2.CrudTest.Infrastructure.EventStore;
using Mc2.CrudTest.Domain.Customers.Events;
using Mc2.CrudTest.Domain.Basic.Events;
using Mc2.CrudTest.Infrastructure.Customers.ReadModels;

namespace Mc2.CrudTest.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<EventStoreEntity> EventStore { get; set; }
    public DbSet<CustomerReadModel> CustomerReadModels { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Ignore<DomainEvent>();
        modelBuilder.Ignore<CustomerCreatedEvent>();
        modelBuilder.Ignore<CustomerUpdatedEvent>();
        modelBuilder.Ignore<CustomerDeletedEvent>();
        


        modelBuilder.Entity<EventStoreEntity>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.AggregateId).IsRequired();
            entity.Property(e => e.AggregateType).IsRequired().HasMaxLength(100);
            entity.Property(e => e.EventType).IsRequired().HasMaxLength(100);
            entity.Property(e => e.EventData).IsRequired();
            entity.Property(e => e.Version).IsRequired();
            entity.Property(e => e.OccurredOn).IsRequired();
            
            entity.HasIndex(e => new { e.AggregateId, e.Version }).IsUnique();
            entity.HasIndex(e => e.AggregateId);
        });

        modelBuilder.Entity<CustomerReadModel>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.DateOfBirth).IsRequired();
            entity.Property(e => e.PhoneNumber).IsRequired().HasMaxLength(20);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
            entity.Property(e => e.BankAccountNumber).IsRequired().HasMaxLength(34);
            entity.Property(e => e.Version).IsRequired();
            entity.Property(e => e.IsDeleted).IsRequired();
            entity.Property(e => e.CreatedAt).IsRequired();
            entity.Property(e => e.UpdatedAt);
            
            entity.HasIndex(e => e.Email).IsUnique().HasFilter("[IsDeleted] = 0");
            entity.HasIndex(e => e.PhoneNumber);
            entity.HasIndex(e => new { e.FirstName, e.LastName, e.DateOfBirth }).IsUnique().HasFilter("[IsDeleted] = 0");
        });
    }
}