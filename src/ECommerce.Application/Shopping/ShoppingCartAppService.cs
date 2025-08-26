using ECommerce.Catalog;
using ECommerce.Shopping.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Scriban.Syntax;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Caching;
using Volo.Abp.Domain.Repositories;

namespace ECommerce.Shopping
{
    public class ShoppingCartAppService : ApplicationService, IShoppingCartAppService
    {
        private readonly IDistributedCache<ShoppingCartCacheItem> _cache;
        private readonly IRepository<Product,int> _productRepository;

        public ShoppingCartAppService(IDistributedCache<ShoppingCartCacheItem> cache, IRepository<Product, int> productRepository)
        {
            _cache = cache;
            _productRepository = productRepository;
        }

        public async Task<ShoppingCartDto> AddItemAsync(AddToCartDto input)
        {
            if (input.Quantity <= 0)
            {
                throw new BusinessException("QuantityMustBePositive");
            }
            var key = CartCacheKey.ForCustomer(input.CustomerId);
            var cart = await _cache.GetAsync(key) ?? new ShoppingCartCacheItem();

            var existing = cart.Items.FirstOrDefault(i => i.ProductId == input.ProductId);
            if(existing == null)
            {
                cart.Items.Add(new ShoppingCartItemCacheItem
                {
                    ProductId = input.ProductId,
                    Quantity = input.Quantity,
                    AddedOnUtc = DateTime.UtcNow,
                });
            }
            else
            {
                existing.Quantity += input.Quantity;
            }

            await _cache.SetAsync(key, cart, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(7)
            });

            return await BuildCartDtoAsync(input.CustomerId, cart);
        }

        public async Task ClearAsync(CheckOutDto input)
        {
            await _cache.RemoveAsync(CartCacheKey.ForCustomer(input.CustomerId));
        }

        public async Task<ShoppingCartDto> GetAsync(CheckOutDto input)
        {
            var cart = await _cache.GetAsync(CartCacheKey.ForCustomer(input.CustomerId))
                       ?? new ShoppingCartCacheItem();

            return await BuildCartDtoAsync(input.CustomerId, cart);

        }

        public async Task RemoveItemAsync(UpdateCartItemDto input)
        {
            var key = CartCacheKey.ForCustomer(input.CustomerId);
            var cart = await _cache.GetAsync(key)??new ShoppingCartCacheItem(); 

            cart.Items.RemoveAll(i=>i.ProductId == input.ProductId);

            await _cache.SetAsync(key, cart, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(7)
            });
        }

        public async Task<ShoppingCartDto> UpdateItemAsync(UpdateCartItemDto input)
        {
            var key = CartCacheKey.ForCustomer(input.CustomerId);
            var cart = await _cache.GetAsync(key) ?? new ShoppingCartCacheItem();

            var existing = cart.Items.FirstOrDefault(i => i.ProductId == input.ProductId);
            if (existing == null)
                return await BuildCartDtoAsync(input.CustomerId, cart);

            if (input.Quantity <= 0)
            {
                cart.Items.Remove(existing);
            }
            else
            {
                existing.Quantity = input.Quantity;
            }
            await _cache.SetAsync(key, cart, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(7)
            });
            return await BuildCartDtoAsync(input.CustomerId, cart);
        }
        public async Task<int> DebugCartCountAsync(Guid customerId)
        {
            var key = CartCacheKey.ForCustomer(customerId);
            var savedCart = await _cache.GetAsync(key);
            return savedCart?.Items.Count ?? 0;
        }

        private async Task<ShoppingCartDto> BuildCartDtoAsync(Guid customerId, ShoppingCartCacheItem cart)
        {
            var result = new ShoppingCartDto { CustomerId = customerId };
            if(cart.Items.Count == 0)
            {
                return result;
            }
            var productIds = cart.Items.Select(x => x.ProductId).Distinct().ToList();
            var products = await (await _productRepository.GetQueryableAsync())
                                .Where(p => productIds.Contains(p.Id))
                                .Select(p => new { p.Id, p.Name, p.Sku, p.Price })
                                .ToListAsync();

            foreach (var line in cart.Items)
            {
                var p = products.FirstOrDefault(x => x.Id == line.ProductId);
                if (p == null) continue;
                var dto = new ShoppingCartItemDto
                {
                    ProductId = p.Id,
                    ProductName = p.Name,
                    Sku = p.Sku,
                    UnitPrice = p.Price,
                    Quantity = line.Quantity,
                    LineTotal = p.Price * line.Quantity,
                    AddedOnUtc = line.AddedOnUtc
                };
                result.Items.Add(dto);
            }
            result.SubTotal = result.Items.Sum(x=>x.LineTotal);
            return result;
        }
    }
}
