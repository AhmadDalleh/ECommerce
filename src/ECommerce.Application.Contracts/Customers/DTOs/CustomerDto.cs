using ECommerce.Customers.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace ECommerce.Customers.DTOs
{
    public class CustomerDto : EntityDto<Guid>
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public CustomerType Type { get; set; }

        //public List<Guid> RoleIds { get; set; } = new List<Guid>();

        public List<Guid> AddressIds { get; set; } = new List<Guid>();
    }
}
