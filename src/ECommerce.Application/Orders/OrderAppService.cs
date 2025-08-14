using ECommerce.Orders.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Orders
{
    public class OrderAppService :
        CrudAppService<Order, OrderDto, int, PagedAndSortedResultRequestDto, CreateUpdateOrderDto>,
        IOrderAppService
    {

        private readonly IRepository<OrderItem, int> _orderItemRepo;
        public OrderAppService(IRepository<Order, int> repository, IRepository<OrderItem, int> orderItemRepo) : base(repository)
        {
            _orderItemRepo = orderItemRepo;
        }


        protected override async Task<IQueryable<Order>> CreateFilteredQueryAsync(PagedAndSortedResultRequestDto input)
        {
            var q = await base.CreateFilteredQueryAsync(input);

            return q.Include(o=>o.OrderItems).AsQueryable();
        }


        public override async Task<OrderDto> CreateAsyc(CreateUpdateOrderDto input)
        {
            var item = await Repository.InsertAsync(new Order(0, input.CustomerId), autoSave: true);

            foreach(var i in input.OrderItems)
            {
                var item = new OrderItem(0, i.orderId)
            }
        }
        public Task<OrderDto> RecalculateAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
