using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Catalog.DTOs
{
    public class CreateUpdateProductCategoryDto
    {
        public int ProductId { get; set; }
        public int CategoryId { get; set; }
    }
}
