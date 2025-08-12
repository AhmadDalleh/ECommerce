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
        public Guid CustomerId { get; set; }
        public Guid AddressId { get; set; }
        public CustomerDto Customer { get; set; }
        public AddressDto Address { get; set; }
        public bool IsPrimary { get; set; }
        public string AddressType { get; set; }
        public string AddressName { get; set; }
        public bool IsActive { get; set; }
    }
}
