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
    public class CustomerRoleConfiguration : IEntityTypeConfiguration<CustomerRole>
    {
        public void Configure(EntityTypeBuilder<CustomerRole> builder)
        {
            builder.ToTable("CustomerRoles");

            builder.HasKey(cr=>cr.Id);

            builder.Property(cr => cr.Name).HasMaxLength(255);
            builder.Property(cr => cr.SystemName).HasMaxLength(255);
            builder.Property(cr => cr.DefaultTaxDisplayType).IsRequired(false);
            builder.Property(cr => cr.PurchasedWithProductId).IsRequired(false);

            builder.HasMany(cr=>cr.CustomerCustomerRoles)
                .WithOne(cr=>cr.CustomerRole)
                .HasForeignKey(cr=>cr.CustomerRoleId)
                .OnDelete(DeleteBehavior.Cascade);
                
        }
    }
}
