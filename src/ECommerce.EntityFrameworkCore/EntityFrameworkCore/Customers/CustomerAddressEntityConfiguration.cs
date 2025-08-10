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
            builder.HasKey(ca => new { ca.CustomerId, ca.AddressId });

            builder.HasOne<Customer>()
             .WithMany()
             .HasForeignKey(ca => ca.CustomerId)
             .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<Address>()
             .WithMany()
             .HasForeignKey(ca => ca.AddressId)
             .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
