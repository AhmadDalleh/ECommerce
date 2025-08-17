using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Shopping.DTOs
{
    public class ShoppingCartItemDto
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; }
        public string Sku { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal LineTotal { get; set; }
        public DateTime AddedOnUtc { get; set; }
    }
}
