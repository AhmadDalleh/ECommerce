using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Catalog.DTOs
{
    public class CreateUpdateCategoryDto 
    {
        [MaxLength(200)]
        [Required]
        public string Name { get; set; }

        [MaxLength(200)]
        public string Description { get; set; }
        
        public int? ParentCategoryId { get; set; }

        public string PictureId { get; set; }

        public int? DisplayOrder { get; set; }

        public bool Published { get; set; }


    }
}
