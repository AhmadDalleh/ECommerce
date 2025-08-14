using ECommerce.Orders.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace ECommerce.Orders.DTOs
{
    public class OrderDto : EntityDto<int>
    {
        public Guid CustomerId { get; set; }

        public OrderStatus Status { get; set; }

        public decimal OrderTotal { get; set; }

        public DateTime CreatedOnUtc { get; set; }

        public DateTime UpdatedOnUtc { get; set; }

        public List<OrderItemDto> OrderItems { get; set; }
    }
}
