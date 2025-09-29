using BankingSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankingSystem.Infrastructure.Configurations
{
    public class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.ToTable("Accounts");

            builder.HasKey(c => c.AccountId);

            builder.OwnsOne(t => t.Balance, balance =>
            {
                balance.Property(m => m.Amount)
                     .HasColumnName("Amount")
                     .HasPrecision(24, 8);

                balance.Property(m => m.Currency)
                     .HasColumnName("Currency")
                     .HasMaxLength(8);
            });

            builder.HasMany(a => a.Transactions)
               .WithOne()
               .HasForeignKey(t => t.AccountId)
               .OnDelete(DeleteBehavior.Cascade);

            builder.Metadata.FindNavigation(nameof(Account.Transactions))!
                .SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
