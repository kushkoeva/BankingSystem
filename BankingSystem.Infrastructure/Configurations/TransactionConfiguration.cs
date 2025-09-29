using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankingSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankingSystem.Infrastructure.Configurations
{
    public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.ToTable("Transactions");

            builder.HasKey(t => t.TransactionId);

            builder.Property(t => t.Type)
                   .HasMaxLength(20)
                   .IsRequired();
                     
            builder.OwnsOne(t => t.Money, money =>
            {
                money.Property(m => m.Amount)
                     .HasColumnName("Amount")
                     .HasPrecision(24, 8);

                money.Property(m => m.Currency)
                     .HasColumnName("Currency")
                     .HasMaxLength(8);
            });
        }
    }
}
