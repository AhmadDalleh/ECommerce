using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Customers.DTOs
{
    public class CreateUpdateCustomerAddressDto
    {
        public Guid AddressId { get; set; }
        public AddressDto Address { get; set; }
    }
}
