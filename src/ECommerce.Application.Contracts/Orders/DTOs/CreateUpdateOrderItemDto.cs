using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace ECommerce.Orders.DTOs
{
    public class CreateUpdateOrderItemDto 
    {
        [AllowNull]
        public int OrderId { get; set; }
        [Required]
        public int ProductId {  get; set; }

        [Required]
        public int Quantity {  get; set; }

        [Required]
        public decimal UnitPrice { get; set; }

    }
}
