using ECommerce.Customers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace ECommerce.EntityFrameworkCore.Customers
{
    public class AddressEntityConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.ToTable("Address");
            builder.ConfigureByConvention();
            builder.Property(a => a.FirstName).HasMaxLength(100);
            builder.Property(a => a.LastName).HasMaxLength(100);
            builder.Property(a => a.Email).HasMaxLength(255);

        }
    }
}
