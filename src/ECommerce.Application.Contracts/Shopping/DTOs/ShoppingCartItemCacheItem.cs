using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Shopping.DTOs
{
    [Serializable]
    public class ShoppingCartItemCacheItem
    {
        public int ProductId {  get; set; }
        public int Quantity { get; set; }
        public DateTime AddedOnUtc { get; set; }
    }
}
