using AutoMapper;
using AutoMapper;
using ECommerce.Orders.DTOs;
using ECommerce.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace ECommerce.Orders
{
    [Authorize(ECommercePermissions.Orders.Default)]

    public class OrderAppService :
        CrudAppService<Order, OrderDto, int, PagedAndSortedResultRequestDto, CreateUpdateOrderDto>,
        IOrderAppService
    {

        private readonly IRepository<OrderItem, int> _orderItemRepo;
        private readonly IMapper _mapper;

        public OrderAppService(IRepository<Order, int> repository, IRepository<OrderItem, int> orderItemRepo, IMapper mapper) : base(repository)
        {
            _orderItemRepo = orderItemRepo;
            _mapper = mapper;
        }


        protected override async Task<IQueryable<Order>> CreateFilteredQueryAsync(PagedAndSortedResultRequestDto input)
        {
            var q = await base.CreateFilteredQueryAsync(input);

            return q.Include(o=>o.OrderItems).AsQueryable();
        }


        [Authorize(ECommercePermissions.Orders.Manage)]
        public override async Task<OrderDto> CreateAsync(CreateUpdateOrderDto input)
        {
            var items = new List<OrderItem>();

            var entity = await Repository.InsertAsync(new Order(0, input.CustomerId, items), autoSave: true);

            foreach (var i in input.OrderItems)
            {
                items.Add(new OrderItem(0, entity.Id, i.ProductId, i.Quantity, i.UnitPrice));
            }
            await _orderItemRepo.InsertManyAsync(items, autoSave: true);

            entity = await Repository.InsertAsync(new Order(0, input.CustomerId, items), autoSave: true);

            var queryable = await Repository.GetQueryableAsync();
            entity = await queryable
                .Include(o => o.OrderItems)
                .FirstAsync(o => o.Id == entity.Id);
            entity.RecalculateTotal();

            await Repository.UpdateAsync(entity, autoSave: true);

            return await GetAsync(entity.Id);
        }

        [Authorize(ECommercePermissions.Orders.Manage)]
        public override Task<OrderDto> UpdateAsync(int id, CreateUpdateOrderDto input)
        {
            return base.UpdateAsync(id, input);
        }

        [Authorize(ECommercePermissions.Orders.Manage)]
        public override Task DeleteAsync(int id)
        {
            return base.DeleteAsync(id);
        }
        public async Task<OrderDto> RecalculateAsync(int id)
        {
            var queryable = await Repository.GetQueryableAsync();
            var order = await queryable.Include(o=>o.OrderItems).FirstAsync(o => o.Id == id);
            await Repository.UpdateAsync(order,autoSave:true);
            return await GetAsync(id);
            
        }
    }
}
