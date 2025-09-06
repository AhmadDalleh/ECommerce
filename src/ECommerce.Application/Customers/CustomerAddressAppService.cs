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
    [Authorize(ECommercePermissions.Customers.Manage)]

    public class CustomerAddressAppService : CrudAppService<
        CustomerAddress,
        CustomerAddressDto,
        Guid,
        PagedAndSortedResultRequestDto,
        CreateUpdateCustomerAddressDto>, ICustomerAddressAppService
    {
        public CustomerAddressAppService(IRepository<CustomerAddress, Guid> repository) : base(repository)
        {
        }
    }
}
