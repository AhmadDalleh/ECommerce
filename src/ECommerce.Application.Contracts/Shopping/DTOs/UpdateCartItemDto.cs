using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Shopping.DTOs
{
    public class UpdateCartItemDto
    {
        public Guid CustomerId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; } // if 0 => remove
    }
}
