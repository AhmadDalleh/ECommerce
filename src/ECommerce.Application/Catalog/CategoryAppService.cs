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
    public class CategoryAppService : 
        CrudAppService<Category,
            CategoryDto,
            int,
            PagedAndSortedResultRequestDto,
            CreateUpdateCategoryDto>, ICategoryAppService
    {
        public CategoryAppService(IRepository<Category, int> repository) : base(repository)
        {
        }
    }
}
