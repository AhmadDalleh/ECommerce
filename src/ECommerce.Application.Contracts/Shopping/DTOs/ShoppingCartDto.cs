using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Shopping.DTOs
{
    public class ShoppingCartDto
    {
        public Guid CustomerId { get; set; }
        public List<ShoppingCartItemDto> Items { get; set; } = new();

        public decimal SubTotal { get; set; }
    }
}
