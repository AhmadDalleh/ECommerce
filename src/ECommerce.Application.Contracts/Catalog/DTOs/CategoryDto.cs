using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace ECommerce.Catalog.DTOs
{
    public class CategoryDto : EntityDto<int>
    {
        public string Name {  get; set; }

        public string Description { get; set; }

        public int ParentCategoryId { get; set; }

        public string PictureId { get; set; }

        public int DisplayOrdre {  get; set; }

        public bool Published {  get; set; }

        public DateTime? CreatedOnUtc {  get; set; }
        public DateTime? UpdatedOnUtc { get; set; }
    }
}
