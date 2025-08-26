using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Shopping
{
    public static class CartCacheKey
    {
        public static string ForCustomer(Guid customerId) => $"Cart:Customer:{customerId}";
    }
}
