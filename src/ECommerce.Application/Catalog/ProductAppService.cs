using AutoMapper;
using ECommerce.Catalog.DTOs;
using ECommerce.Orders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectExtending.Modularity;

namespace ECommerce.Catalog
{
    public class ProductAppService :
        CrudAppService<Product,
            ProductDto,
            int,
            PagedAndSortedResultRequestDto,
            CreateUpdateProductDto,
            UpdateProductDto>
        , IProductAppService
    {
        private readonly IRepository<Product, int> _productRepo;
        private readonly IRepository<ProductPhoto,int> _productPhotoRepo;
        private readonly IMapper _mapper;
        public ProductAppService(IRepository<Product, int> repository, IMapper mapper, IRepository<ProductPhoto, int> productPhotoRepo) : base(repository)
        {
            _productRepo = repository;
            _mapper = mapper;
            _productPhotoRepo = productPhotoRepo;
        }

        public override async Task<PagedResultDto<ProductDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
          
            var queryable = await _productRepo.GetQueryableAsync();
            queryable = queryable.Include(o => o.ProductPhotos);
            queryable = queryable.OrderBy(input.Sorting ?? "Id DESC");

            var totalCount = await AsyncExecuter.CountAsync(queryable);

            var products = await AsyncExecuter.ToListAsync(
            queryable.Skip(input.SkipCount).Take(input.MaxResultCount));

            var proudctDtos = _mapper.Map<List<Product>,List<ProductDto>>(products);

            return new PagedResultDto<ProductDto>(
                totalCount,
                proudctDtos);
        }

        public override async Task<ProductDto> GetAsync(int id)
        {
            var product = await _productRepo.GetAsync(id);

            var queryable = await _productRepo.GetQueryableAsync();
            product = await queryable
                .Include(o => o.ProductPhotos)
                .FirstAsync(o => o.Id == id);

            return _mapper.Map<Product, ProductDto>(product);
          
        }
        public override async Task<ProductDto> CreateAsync(CreateUpdateProductDto input)
        {

            //var entity = new Product(0, input.Name, input.ShortDescription, input.FullDescription, input.Price, input.StockQuantity, input.Published)
            //{

            //};
            //_productRepo.InsertAsync(entity, autoSave: true);

            //return _mapper.Map<ProductDto,Product >(entity);

            var photos = new List<ProductPhoto>();

            var entity = await _productRepo.InsertAsync(new Product(0, input.Name, input.ShortDescription, input.FullDescription, input.Price, input.StockQuantity, input.Published), autoSave: true);

            foreach (var i in input.ProductPhotos)
            {
                photos.Add(new ProductPhoto(0, entity.Id, i.PictureUrl, i.DisplayOrder));
            }
            
            await _productPhotoRepo.InsertManyAsync(photos, autoSave: true);

     

            var queryable = await _productRepo.GetQueryableAsync();
            entity = await queryable
                .Include(o => o.ProductPhotos)
                .FirstAsync(o => o.Id == entity.Id);
            
            await Repository.UpdateAsync(entity, autoSave: true);

            return await GetAsync(entity.Id);
        }

        public override async Task<ProductDto> UpdateAsync(int id, UpdateProductDto input)
        {
            var queryable = await _productRepo.GetQueryableAsync();
            var product = await queryable
                .Include(p => p.ProductPhotos)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                throw new EntityNotFoundException(typeof(Product), id);
            }

            product.SetDetails(input.Name, input.ShortDescription, input.FullDescription, input.Price, input.StockQuantity, input.Published);

            var existingPhotos = product.ProductPhotos.ToList();

            var photosToRemove = existingPhotos
                .Where(ep => input.ProductPhotos.All(ip => ip.ProductId != ep.Id))
                .ToList();

            foreach (var photo in photosToRemove) 
            {
                product.ProductPhotos.Remove(photo);
                await _productPhotoRepo.DeleteAsync(photo);
            }
            foreach(var dtoPhoto in input.ProductPhotos)
            {
                var existingPhoto = existingPhotos.FirstOrDefault(p => p.Id == dtoPhoto.ProductId);

                if(existingPhoto != null)
                {
                    existingPhoto.Update(dtoPhoto.PictureUrl, dtoPhoto.DisplayOrder);
                }
                else
                {
                    var newPhoto = new ProductPhoto(0, product.Id, dtoPhoto.PictureUrl, dtoPhoto.DisplayOrder);
                    await _productPhotoRepo.InsertAsync(newPhoto, autoSave: true);
                    product.ProductPhotos.Add(newPhoto);
                }


            }

            await _productRepo.UpdateAsync(product, autoSave: true);

            var updatedProduct = await _productRepo.WithDetailsAsync(x => x.ProductPhotos);

            var finalEntity = await updatedProduct.FirstAsync(x => x.Id == product.Id);

            return _mapper.Map<Product, ProductDto>(finalEntity);

        }




    }
}
