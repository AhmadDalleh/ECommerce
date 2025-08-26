using ECommerce.Catalog;
using ECommerce.Orders;
using ECommerce.Orders.DTOs;
using ECommerce.Orders.Enums;
using ECommerce.Shopping.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Caching;
using Volo.Abp.Domain.Repositories;

namespace ECommerce.Shopping
{
    public class CheckoutAppService : ApplicationService, ICheckoutAppService
    {
        private readonly IDistributedCache<ShoppingCartCacheItem> _cache;
        private readonly IRepository<Product, int> _productRepo;
        private readonly IRepository<Order, int> _orderRepo;
        private readonly IRepository<OrderItem, int> _orderItemRepo;

        public CheckoutAppService(
            IDistributedCache<ShoppingCartCacheItem> cache,
            IRepository<Product, int> productRepo,
            IRepository<Order, int> orderRepo,
            IRepository<OrderItem, int> orderItemRepo)
        {
            _cache = cache;
            _productRepo = productRepo;
            _orderRepo = orderRepo;
            _orderItemRepo = orderItemRepo;
        }


        public async Task<OrderDto> CheckoutAsync(CheckOutDto input)
        { 
            List<OrderItem>orderItems = new List<OrderItem>();
            var key = CartCacheKey.ForCustomer(input.CustomerId);
            var cart = await _cache.GetAsync(key) ?? new ShoppingCartCacheItem();
            if (cart.Items.Count == 0) 
            {
                throw new BusinessException("CartIsEmpty");
            }

            var productIds = cart.Items.Select(i => i.ProductId).Distinct().ToList();
            var products = await (await _productRepo.GetQueryableAsync())
                .Where(p => productIds.Contains(p.Id))
                .Select(p => new { p.Id, p.Price })
                .ToListAsync();

            var order = new Order(0, input.CustomerId,orderItems);
            order.SetStatus(OrderStatus.Processing);
            order = await _orderRepo.InsertAsync(order,autoSave:true);

            foreach (var line in cart.Items) 
            {
                var p = products.FirstOrDefault(x=>x.Id == line.ProductId);
                if (p == null) continue;

                var item = new OrderItem(0,order.Id,p.Id,line.Quantity,p.Price);
                await _orderItemRepo.InsertAsync(item,autoSave:true);  
            }
            // 5) Recalculate total and save
            var queryable = await _orderRepo.GetQueryableAsync();
            order = await queryable.Include(o=>o.OrderItems).FirstAsync(o=>o.Id == order.Id);
            //order = await _orderRepo.GetQueryable().Include(o => o.Items).FirstAsync(o => o.Id == order.Id);
            order.RecalculateTotal();
            await _orderRepo.UpdateAsync(order, autoSave: true);

            // 6) Clear cart
            await _cache.RemoveAsync(key);

            // 7) Return OrderDto using CrudAppService mapping pipeline (shortcut: map here)
            return ObjectMapper.Map<Order, OrderDto>(order);
        }
    }
}
