using Microsoft.EntityFrameworkCore;
using Mc2.CrudTest.Domain.Entities;
using Mc2.CrudTest.Domain.ValueObjects;

namespace Mc2.CrudTest.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Customer> Customers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.DateOfBirth).IsRequired();
            entity.Property(e => e.BankAccountNumber).IsRequired().HasMaxLength(34);
            
            entity.Property(e => e.Email)
                .HasConversion(
                    email => email.Value,
                    value => Email.Create(value))
                .IsRequired()
                .HasMaxLength(255);
            
            entity.Property(e => e.PhoneNumber)
                .HasConversion(
                    phone => phone.Value,
                    value => PhoneNumber.Create(value))
                .IsRequired()
                .HasMaxLength(20);
            
            entity.HasIndex(e => e.Email).IsUnique();
        });
    }
}