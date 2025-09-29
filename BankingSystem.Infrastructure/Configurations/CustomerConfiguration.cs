using BankingSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankingSystem.Infrastructure.Configurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable("Customers");

            builder.HasKey(c => c.CustomerId);

            builder.OwnsOne(c => c.Name, name =>
            {
                name.Property(n => n.FirstName).HasMaxLength(100);
                name.Property(n => n.MiddleName).HasMaxLength(100);
                name.Property(n => n.LastName).HasMaxLength(100);
            });

            builder.OwnsOne(c => c.Email, email =>
            {
                email.Property(e => e.Value)
                     .HasColumnName("Email")
                     .HasMaxLength(255);
            });

            builder.OwnsOne(c => c.Phone, phone =>
            {
                phone.Property(e => e.Value)
                     .HasColumnName("Phone")
                     .HasMaxLength(32);
            });

            builder.OwnsOne(c => c.Address, address =>
            {
                address.Property(a => a.Country).HasMaxLength(100);
                address.Property(a => a.Region).HasMaxLength(100);
                address.Property(a => a.City).HasMaxLength(100);
                address.Property(a => a.Street).HasMaxLength(200);
                address.Property(a => a.ZipCode).HasMaxLength(20);
            });

            builder.Property(c => c.Status)
                   .HasConversion<string>()
                   .HasColumnName("Status")
                   .HasMaxLength(20)
                   .IsRequired();

            builder.HasMany(c => c.Accounts)
                   .WithOne(a => a.Customer)
                   .HasForeignKey(a => a.CustomerId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.Metadata.FindNavigation(nameof(Customer.Accounts))!
                .SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
