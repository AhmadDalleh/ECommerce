using ECommerce.Customers.cs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.EntityFrameworkCore.Customers
{
    public class CustomerEntityTypeConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable("Customers");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(c=>c.Email)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(c => c.PasswordHash)
                .IsRequired();

            builder.Property(c => c.Type)
                .IsRequired();

            builder.Property(c => c.CreatedOnUtc)
                .IsRequired();
        }
    }
}
