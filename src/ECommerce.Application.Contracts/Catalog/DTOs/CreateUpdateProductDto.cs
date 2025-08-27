using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Catalog.DTOs
{
    public class CreateUpdateProductDto
    {
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        [Required]
        [MaxLength(500)]
        public string ShortDescription { get; set; }

        [Required]
        [MaxLength(4000)]
        public string FullDescription { get; set; }

        [Required]
        public decimal Price { get; set; }
        
        [Required]
        public int StockQuantity { get; set; }

        [Required]
        public bool Published { get; set; }
    }
}
