using ECommerce.Orders.DTOs;
using ECommerce.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace ECommerce.Orders
{
    [Authorize(ECommercePermissions.Orders.Default)]
    public class OrderItemAppService :
        CrudAppService<OrderItem, OrderItemDto, int, PagedAndSortedResultRequestDto, CreateUpdateOrderItemDto>, IOrderItemAppService
    {
        private readonly IRepository<Order, int> _orderRepo;
        public OrderItemAppService(IRepository<OrderItem, int> repository, IRepository<Order, int> orderRepo) : base(repository)
        {
            _orderRepo = orderRepo;
        }
        [Authorize(ECommercePermissions.Orders.Manage)]
        public override async Task<OrderItemDto> CreateAsync(CreateUpdateOrderItemDto input)
        {
            var orderExists = await _orderRepo.AnyAsync(o => o.Id == input.OrderId);
            if (!orderExists)
            {
                throw new BusinessException($"Order with Id {input.OrderId} does not exist.");
            }

            var dto = await base.CreateAsync(input);

            // Update order total after adding new item
            var queryable = await _orderRepo.GetQueryableAsync();
            var order = await queryable.Include(o => o.OrderItems)
                                       .FirstAsync(o => o.Id == input.OrderId);

            order.RecalculateTotal();
            await _orderRepo.UpdateAsync(order, autoSave: true);

            return dto;
        }
        [Authorize(ECommercePermissions.Orders.Manage)]
        public override async Task<OrderItemDto> UpdateAsync(int id, CreateUpdateOrderItemDto input)
        {
            var dto = await base.UpdateAsync(id, input);

            var entity = await Repository.GetAsync(id);
            var queryable = await _orderRepo.GetQueryableAsync();
            var order = await queryable.Include(o=>o.OrderItems).FirstAsync(o=>o.Id == entity.OrderId);
            order.RecalculateTotal();
            await _orderRepo.UpdateAsync(order,autoSave:true);

            return dto;
        }
        [Authorize(ECommercePermissions.Orders.Manage)]
        public override async Task DeleteAsync(int id)
        {
            var entity = await Repository.GetAsync(id);
            var orderId = entity.OrderId;

            await base.DeleteAsync(id);

            var queryable = await _orderRepo.GetQueryableAsync();
            var order = await queryable.Include(o=>o.OrderItems).FirstOrDefaultAsync(o=>o.Id == orderId);

            if(order != null)
            {
                order.RecalculateTotal();
                await _orderRepo.UpdateAsync(order,autoSave:true);
            }
        }
    }
}
