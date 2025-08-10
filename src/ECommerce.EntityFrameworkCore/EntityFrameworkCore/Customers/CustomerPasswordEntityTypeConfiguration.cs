using ECommerce.Customers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.EntityFrameworkCore.Customers
{
    public class CustomerPasswordEntityTypeConfiguration : IEntityTypeConfiguration<CustomerPassword>
    {
        public void Configure(EntityTypeBuilder<CustomerPassword> builder)
        {
            builder.ToTable("CustomersPasswords");
            builder.HasKey(cp=>cp.Id);

            builder.Property(cp=>cp.Password)
                .IsRequired();


            builder.Property(cp => cp.PasswordFormatId)
                .IsRequired();

            builder.Property(cp => cp.CreatedOnUtc)
                .IsRequired();

           builder.HasOne<Customer>()
                .WithMany()
                .HasForeignKey(cp=>cp.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
