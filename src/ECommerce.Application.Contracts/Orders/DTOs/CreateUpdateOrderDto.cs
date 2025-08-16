using ECommerce.Orders.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Orders.DTOs
{
    public class CreateUpdateOrderDto
    {
        public Guid CustomerId {  get; set; }

        public OrderStatus OrderStatus { get; set; }

        public List<CreateUpdateOrderItemDto> OrderItems { get; set; } = new();
    }
}
