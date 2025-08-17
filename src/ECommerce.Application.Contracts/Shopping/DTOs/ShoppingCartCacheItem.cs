using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Shopping.DTOs
{
    [Serializable]
    public class ShoppingCartCacheItem
    {
        public List<ShoppingCartItemCacheItem> Items { get; set; }
    }
}
