using ECommerce.Shopping.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace ECommerce.Shopping
{
    public interface IShoppingCartAppService : IApplicationService
    {
        Task<ShoppingCartDto> GetAsync(CheckOutDto input);
        Task<ShoppingCartDto> AddItemAsync(AddToCartDto input);
        Task<ShoppingCartDto> UpdateItemAsync(UpdateCartItemDto input);
        Task RemoveItemAsync(UpdateCartItemDto input);
        Task ClearAsync(CheckOutDto input);

    }
}
