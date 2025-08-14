using ECommerce.Orders.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace ECommerce.Orders
{
    public interface IOrderItemAppService : 
        ICrudAppService<OrderItemDto,int,PagedAndSortedResultRequestDto,CreateUpdateOrderItemDto>
    {
    }
}
