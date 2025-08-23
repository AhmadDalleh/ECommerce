using ECommerce.Orders.DTOs;
using ECommerce.Shopping.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace ECommerce.Shopping
{
    public interface ICheckoutAppService : IApplicationService
    {
        Task<OrderDto> CheckoutAsync(CheckOutDto input);
    }
}
