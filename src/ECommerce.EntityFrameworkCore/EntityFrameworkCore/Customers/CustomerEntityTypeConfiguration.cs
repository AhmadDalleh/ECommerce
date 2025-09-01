using ECommerce.Customers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Identity;

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

            builder.Property(c => c.Email)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(c => c.PasswordHash)
                .IsRequired()
                .HasMaxLength(512);

            builder.Property(c => c.Type)
                .IsRequired();

            builder.Property(c => c.CreatedOnUtc)
                .IsRequired();

            builder.Property(c => c.IsActive)
                .HasDefaultValue(true)
                .IsRequired();

            builder.Property(c => c.IdentityUserId)
                .HasColumnName("IdentityUserId")
                .IsRequired(false);

            builder.HasKey(c=>c.Id);

         
            builder.HasOne<IdentityUser>()                 // requires EF reference to Identity
                   .WithMany()
                   .HasForeignKey(c => c.IdentityUserId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
