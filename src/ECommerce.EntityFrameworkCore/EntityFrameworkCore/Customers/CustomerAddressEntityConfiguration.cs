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
    public class CustomerAddressEntityConfiguration : IEntityTypeConfiguration<CustomerAddress>
    {
        public void Configure(EntityTypeBuilder<CustomerAddress> builder)
        {
            builder.ToTable("Customer_Address_Mapping"); // match nopCommerce

            // Use Guid as primary key, but ensure CustomerId + AddressId combination is unique
            builder.HasKey(ca => ca.Id);
            
            // Create unique index on CustomerId + AddressId to prevent duplicates
            builder.HasIndex(ca => new { ca.CustomerId, ca.AddressId })
                .IsUnique();

            builder.HasOne(ca => ca.Customer)
                .WithMany(c => c.CustomerAddresses)
                .HasForeignKey(ca => ca.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(ca => ca.Address)
                .WithMany()
                .HasForeignKey(ca => ca.AddressId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
