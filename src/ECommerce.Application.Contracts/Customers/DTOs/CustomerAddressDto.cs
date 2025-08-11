using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace ECommerce.Customers.DTOs
{
    public class CustomerAddressDto : EntityDto<Guid>
    {
        public Guid AddressId { get; set; }
        public AddressDto Address { get; set; }
    }
}
