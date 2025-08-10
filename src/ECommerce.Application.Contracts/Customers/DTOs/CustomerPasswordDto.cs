using ECommerce.Customers.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace ECommerce.Customers.DTOs
{
    public class CustomerPasswordDto : EntityDto<int>
    {
        public Guid CustomerId { get; set; }
        public string Password { get; set; }
        public PasswordFormat PasswordFormatId { get; set; }
        public string PasswordSalt { get; set; }
        public DateTime CreatedOnUtc { get; set; }
    }
}
