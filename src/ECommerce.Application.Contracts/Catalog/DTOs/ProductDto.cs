using ECommerce.Orders.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace ECommerce.Catalog.DTOs
{
    public class ProductDto : EntityDto<int>
    {
        public string Name { get; set; }
        public string ShortDescription { get; set; }

        public string FullDescription { get; set; }

        public decimal Price { get; set; }
        
        public int StockQuantity { get; set; }

        public bool Published { get; set; }

        public string? Sku {  get; set; }

        public List<ProductPhotoDto>? ProductPhotos { get; set; }
        public List<int> CategoryIds { get; set; } 
    }
}
