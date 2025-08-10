using ECommerce.Customers.DTOs;
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
    }
}
