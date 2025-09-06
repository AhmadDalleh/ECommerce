using ECommerce.Customers.DTOs;
using ECommerce.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace ECommerce.Customers
{
    [Authorize(ECommercePermissions.Customers.Default)]

    public class AddressAppService : CrudAppService<
        Address,
        AddressDto,
        Guid,
        PagedAndSortedResultRequestDto,
        CreateUpdateAddressDto>,
        IAddressAppService
    {
        public AddressAppService(IRepository<Address, Guid> repository) : base(repository)
        {
        }

        [Authorize(ECommercePermissions.Customers.Manage)]
        public override Task<AddressDto> CreateAsync(CreateUpdateAddressDto input)
        {
            return base.CreateAsync(input);
        }

        [Authorize(ECommercePermissions.Customers.Manage)]

        public override Task<AddressDto> UpdateAsync(Guid id, CreateUpdateAddressDto input)
        {
            return base.UpdateAsync(id, input);
        }

        [Authorize(ECommercePermissions.Customers.Manage)]

        public override Task DeleteAsync(Guid id)
        {
            return base.DeleteAsync(id);
        }
    }
}
