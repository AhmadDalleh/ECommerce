using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Catalog.DTOs
{
    public class CreateUpdateProductWithPhoto
    {
        [Required]
        [MaxLength(1000)]
        public string PictureUrl { get; set; }


        public int DisplayOrder { get; set; }
    }
}
