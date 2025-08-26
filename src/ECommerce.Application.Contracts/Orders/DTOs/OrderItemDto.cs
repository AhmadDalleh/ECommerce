using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace ECommerce.Orders.DTOs
{
    public class OrderItemDto : EntityDto<int>
    {
        public int OrderId { get; set; }

        public int ProductId {  get; set; }

        public int Quantity {  get; set; }

        public decimal UnitPrice { get; set; }
    }
}
