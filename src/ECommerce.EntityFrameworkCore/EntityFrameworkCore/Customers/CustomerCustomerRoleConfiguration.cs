using ECommerce.Customers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.EntityFrameworkCore.Customers
{
    public class CustomerCustomerRoleConfiguration : IEntityTypeConfiguration<CustomerCustomerRole>
    {
        public void Configure(EntityTypeBuilder<CustomerCustomerRole> builder)
        {
            builder.ToTable("Customer_Customer_Mapping");

            builder.HasKey(x => x.Id);

            builder.HasIndex(x => new {x.CustomerId,x.CustomerRoleId}).IsUnique(false);

            builder.HasOne(x=>x.Customer)
                .WithMany(c=>c.CustomerCustomerRoles)
                .HasForeignKey(x => x.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x=>x.CustomerRole)
                .WithMany(r=>r.CustomerCustomerRoles)
                .HasForeignKey(x=>x.CustomerRoleId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
