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
    [Authorize(ECommercePermissions.Products.Default)]

    public class ProductPhotoAppService :
        CrudAppService<ProductPhoto,
            ProductPhotoDto,
            int,
            PagedAndSortedResultRequestDto,
            CreateUpdateProductPhotoDto>,
        IProductPhotoAppService
    {
        public ProductPhotoAppService(IRepository<ProductPhoto, int> repository) : base(repository)
        {
            
        }

        [Authorize(ECommercePermissions.Products.Create)]
        public override Task<ProductPhotoDto> CreateAsync(CreateUpdateProductPhotoDto input)
        {
            return base.CreateAsync(input);
        }


        [Authorize(ECommercePermissions.Products.Update)]
        public override Task<ProductPhotoDto> UpdateAsync(int id, CreateUpdateProductPhotoDto input)
        {
            return base.UpdateAsync(id, input);
        }

        [Authorize(ECommercePermissions.Products.Delete)]

        public override Task DeleteAsync(int id)
        {
            return base.DeleteAsync(id);
        }
    }
}
