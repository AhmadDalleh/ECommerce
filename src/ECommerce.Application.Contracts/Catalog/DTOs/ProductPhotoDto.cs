using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace ECommerce.Catalog.DTOs
{
    public class ProductPhotoDto : EntityDto<int>
    {
        public int ProductId { get; set; }

        public string PictureUrl { get; set; }

        public int DisplayOrder {  get; set; }
    }
}
