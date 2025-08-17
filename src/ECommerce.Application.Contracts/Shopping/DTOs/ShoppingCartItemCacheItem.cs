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
        public string Quantiyt { get; set; }
        public DateTime AddedOnUtc { get; set; }
    }
}
