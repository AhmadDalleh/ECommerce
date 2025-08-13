using ECommerce.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.EntityFrameworkCore.Orders
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders");

            builder.HasKey(o=>o.Id);

            builder.Property(o => o.CustomerId)
                .IsRequired();
            
            builder.Property(o=>o.Status)
                .IsRequired();

            builder.Property(o => o.OrderTotal)
                .HasColumnType("decimal(18,2)")
                .IsRequired();
            builder.Property(o=>o.CreatedOnUtc)
                .IsRequired();

            builder.HasMany(o => o.OrderItems)
                .WithOne(oi => oi.Order)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
                
        }
    }
}
