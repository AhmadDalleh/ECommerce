using ECommerce.Catalog.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace ECommerce.Catalog
{
    public class ProductCategoryAppService :
        CrudAppService<ProductCategory,
            ProductCategoryDto,
            int, PagedAndSortedResultRequestDto,
            CreateUpdateProductCategoryDto>,
        IProductCategoryAppService
    {
        public ProductCategoryAppService(IRepository<ProductCategory, int> repository) : base(repository)
        {
        }
    }
}
