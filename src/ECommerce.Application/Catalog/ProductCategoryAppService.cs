using ECommerce.Catalog.DTOs;
using ECommerce.Permissions;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize(ECommercePermissions.Categories.Default)]

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

        [Authorize(ECommercePermissions.Categories.Manage)]
        public override Task<ProductCategoryDto> CreateAsync(CreateUpdateProductCategoryDto input)
        {
            return base.CreateAsync(input);
        }

        [Authorize(ECommercePermissions.Categories.Manage)]

        public override Task<ProductCategoryDto> UpdateAsync(int id, CreateUpdateProductCategoryDto input)
        {
            return base.UpdateAsync(id, input);
        }

        [Authorize(ECommercePermissions.Categories.Manage)]

        public override Task DeleteAsync(int id)
        {
            return base.DeleteAsync(id);
        }
    }
}
