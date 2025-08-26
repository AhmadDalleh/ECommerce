using AutoMapper;
using ECommerce.Orders.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace ECommerce.Orders
{
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


        public override async Task<OrderDto> CreateAsync(CreateUpdateOrderDto input)
        {
            var entity = await Repository.InsertAsync(new Order(0, input.CustomerId), autoSave: true);
            foreach(CreateUpdateOrderItemDto i in input.OrderItems)
            {
                var item = new OrderItem(0, entity.Id, i.ProductId, i.Quantity, i.UnitPrice);

                await _orderItemRepo.InsertAsync(item, autoSave: true);

            }
            var queryable = await Repository.GetQueryableAsync();
            entity = await queryable
                .Include(o => o.OrderItems)
                .FirstAsync(o => o.Id == entity.Id);
            entity.RecalculateTotal();
            return _mapper.Map<OrderDto>(entity); // ✅ Map directly
        }

        public async Task<OrderDto> RecalculateAsync(int id)
        {
            var queryable = await Repository.GetQueryableAsync();
            var order = await queryable.Include(o => o.OrderItems).FirstAsync(o => o.Id == id);
            order.RecalculateTotal();
            await Repository.UpdateAsync(order, autoSave: true);
            return _mapper.Map<OrderDto>(order); // ✅ Map directly
        }
    }
}
