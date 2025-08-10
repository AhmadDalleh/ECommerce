using ECommerce.Customers.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace ECommerce.Customers
{
    public interface ICustomerPasswordAppService: ICrudAppService<
            CustomerPasswordDto,
            int,
            PagedAndSortedResultRequestDto,
            CreateCustomerPasswordDto
        >
    {
    }
}
