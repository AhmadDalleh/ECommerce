using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Orders.Enums
{
    public enum OrderStatus
    {
        Pending = 0,
        Processing = 1,
        Complete = 2,
        Cancelled = 3
    }
}
