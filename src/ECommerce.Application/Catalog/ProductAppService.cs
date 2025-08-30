using AutoMapper;
using ECommerce.Catalog.DTOs;
using ECommerce.Orders;
using ECommerce.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private readonly IRepository<ProductPhoto, int> _productPhotoRepo;
        private readonly IRepository<ProductCategory, int> _productCategoryRepo;
        private readonly IMapper _mapper;
        private readonly IImageHandlerAppService _imageHandler;
        public ProductAppService(IRepository<Product, int> repository, IMapper mapper, IRepository<ProductPhoto, int> productPhotoRepo, IImageHandlerAppService imageHandler, IRepository<ProductCategory, int> productCategoryRepo) : base(repository)
        {
            _productRepo = repository;
            _mapper = mapper;
            _productPhotoRepo = productPhotoRepo;
            _imageHandler = imageHandler;
            _productCategoryRepo = productCategoryRepo;
        }

        public override async Task<PagedResultDto<ProductDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {

            var queryable = await _productRepo.GetQueryableAsync();
            queryable = queryable.Include(o => o.ProductPhotos).Include(o => o.ProductCategories);
            queryable = queryable.OrderBy(input.Sorting ?? "Id DESC");

            var totalCount = await AsyncExecuter.CountAsync(queryable);

            var products = await AsyncExecuter.ToListAsync(
            queryable.Skip(input.SkipCount).Take(input.MaxResultCount));

            var productDtos = _mapper.Map<List<Product>, List<ProductDto>>(products);

            foreach (var (entity, dto) in products.Zip(productDtos, (e, d) => (e, d)))
            {
                dto.CategoryIds = entity.ProductCategories.Select(pc => pc.CategoryId).ToList();
            }

            return new PagedResultDto<ProductDto>(
                totalCount,
                productDtos);
        }

        public override async Task<ProductDto> GetAsync(int id)
        {
            var product = await _productRepo.GetAsync(id);

            var queryable = await _productRepo.GetQueryableAsync();
            product = await queryable
                .Include(o => o.ProductPhotos)
                .Include(o=> o.ProductCategories)   
                .FirstAsync(o => o.Id == id);

            var dto = _mapper.Map<Product, ProductDto>(product);
            dto.CategoryIds = product.ProductCategories.Select(pc => pc.CategoryId).ToList();
            return dto;

        }
        public override async Task<ProductDto> CreateAsync([FromForm] CreateUpdateProductDto input)
        {

            //var entity = new Product(0, input.Name, input.ShortDescription, input.FullDescription, input.Price, input.StockQuantity, input.Published)
            //{

            //};
            //_productRepo.InsertAsync(entity, autoSave: true);

            //return _mapper.Map<ProductDto,Product >(entity);

            var entity = await _productRepo.InsertAsync(
                new Product(
                    0, input.Name,
                    input.ShortDescription,
                    input.FullDescription,
                    input.Price,
                    input.StockQuantity,
                    input.Published),
                autoSave: true);
            var photos = new List<ProductPhoto>();

            if (input.UploadedPhotos != null && input.UploadedPhotos.Count > 0)
            {
                var uploadedUrls = await _imageHandler.UploadAsync(input.UploadedPhotos, entity.Name);
                foreach (var (url, index) in uploadedUrls.Select((v, i) => (v, i)))
                {
                    photos.Add(new ProductPhoto(0, entity.Id, url, index));
                }
            }

            foreach (var i in input.ProductPhotos)
            {
                photos.Add(new ProductPhoto(0, entity.Id, i.PictureUrl, i.DisplayOrder));
            }
            if (photos.Any())
                await _productPhotoRepo.InsertManyAsync(photos, autoSave: true);

            if (input.CategoryIds != null && input.CategoryIds.Any())
            {
                var productCategories = input.CategoryIds.Select(cid => new ProductCategory(0, entity.Id, cid)).ToList();
                await _productCategoryRepo.InsertManyAsync(productCategories, autoSave: true);
            }

            var queryable = await _productRepo.GetQueryableAsync();
            entity = await queryable
                .Include(o => o.ProductPhotos)
                .Include(p => p.ProductCategories)
                .FirstAsync(o => o.Id == entity.Id);

            var dto = _mapper.Map<Product, ProductDto>(entity);
            dto.CategoryIds = entity.ProductCategories.Select(pc => pc.CategoryId).ToList();
            return dto;
        }

        public override async Task<ProductDto> UpdateAsync(int id, [FromForm] UpdateProductDto input)
        {
            //var queryable = await _productRepo.GetQueryableAsync();
            //var product = await queryable
            //    .Include(p => p.ProductPhotos)
            //    .FirstOrDefaultAsync(p => p.Id == id);

            //if (product == null)
            //{
            //    throw new EntityNotFoundException(typeof(Product), id);
            //}

            //product.SetDetails(input.Name, input.ShortDescription, input.FullDescription, input.Price, input.StockQuantity, input.Published);

            //if (input.UploadedPhotos != null && input.UploadedPhotos.Count > 0)
            //{
            //    var uploadedUrls = await _imageHandler.UploadAsync(input.UploadedPhotos, product.Name);

            //    foreach (var (url, index) in uploadedUrls.Select((v, i) => (v, i)))
            //    {
            //        var newPhoto = new ProductPhoto(0, product.Id, url, index);
            //        await _productPhotoRepo.InsertAsync(newPhoto, autoSave: true);
            //    }
            //}


            //var existingPhotos = product.ProductPhotos.ToList();

            //var photosToRemove = existingPhotos
            //    .Where(ep => input.ProductPhotos.All(ip => ip.ProductId != ep.Id))
            //    .ToList();

            //foreach (var photo in photosToRemove) 
            //{
            //    product.ProductPhotos.Remove(photo);
            //    await _productPhotoRepo.DeleteAsync(photo);
            //    _imageHandler.DeleteImageAsync(photo.PictureUrl); // delete physical file too

            //}
            //foreach (var dtoPhoto in input.ProductPhotos)
            //{
            //    var existingPhoto = existingPhotos.FirstOrDefault(p => p.Id == dtoPhoto.ProductId);

            //    if(existingPhoto != null)
            //    {
            //        existingPhoto.Update(dtoPhoto.PictureUrl, dtoPhoto.DisplayOrder);
            //    }

            //}

            //await _productRepo.UpdateAsync(product, autoSave: true);

            //var updatedProduct = await _productRepo.WithDetailsAsync(x => x.ProductPhotos);

            //var finalEntity = await updatedProduct.FirstAsync(x => x.Id == product.Id);

            //return _mapper.Map<Product, ProductDto>(finalEntity);

            var products = await _productRepo.WithDetailsAsync(
                p => p.ProductPhotos,
                p => p.ProductCategories
            );

            var product = products.FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                throw new EntityNotFoundException(typeof(Product), id);
            }

            product.SetDetails(
                input.Name,
                input.ShortDescription,
                input.FullDescription,
                input.Price,
                input.StockQuantity,
                input.Published
            );

            if(input.UploadedPhotos !=null && input.UploadedPhotos.Count > 0)
            {
                var uploadedUrls = await _imageHandler.UploadAsync(input.UploadedPhotos, product.Name);

                foreach(var (url,index) in uploadedUrls.Select((v, i) => (v, i)))
                {
                    var newPhoto = new ProductPhoto(0, product.Id, url, index);
                    await _productPhotoRepo.InsertAsync(newPhoto, autoSave: true);
                }
            }

            var existingPhotos = product.ProductPhotos.ToList();

            var photosToRemove = existingPhotos
                .Where(ep => input.ProductPhotos.All(ip => ip.ProductId != ep.Id)) // compare by Id
                .ToList();

            // 5) Update existing photos
            foreach (var dtoPhoto in input.ProductPhotos)
            {
                var existingPhoto = existingPhotos.FirstOrDefault(p => p.Id == dtoPhoto.ProductId);

                if (existingPhoto != null)
                {
                    existingPhoto.Update(dtoPhoto.PictureUrl, dtoPhoto.DisplayOrder);
                }
            }

            var existingCategories = product.ProductCategories.ToList();

            if (existingCategories.Any())
            {
                await _productCategoryRepo.DeleteManyAsync(existingCategories);
            }
            
            if(input.CategoryIds !=null&& input.CategoryIds.Any())
            {
                var productCategories = input.CategoryIds.Select(cid => new ProductCategory(0, product.Id, cid)).ToList();
                await _productCategoryRepo.InsertManyAsync(productCategories,autoSave: true);
            }

            await _productRepo.UpdateAsync(product, autoSave: true);
            var updatedProducts = await _productRepo.WithDetailsAsync(
                p => p.ProductPhotos,
                p => p.ProductCategories
            );


            var finalEntity = updatedProducts.First(x => x.Id == product.Id);

            // 9) Return DTO
            var dto = _mapper.Map<Product, ProductDto>(finalEntity);
            dto.CategoryIds = finalEntity.ProductCategories.Select(pc => pc.CategoryId).ToList();
            return dto;
        }

    }
}
