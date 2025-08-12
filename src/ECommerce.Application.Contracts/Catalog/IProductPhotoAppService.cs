using ECommerce.Catalog.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace ECommerce.Catalog
{
    public interface IProductPhotoAppService : ICrudAppService<ProductPhotoDto,int,PagedAndSortedResultRequestDto,CreateUpdateProductPhotoDto>
    {
    }
}
