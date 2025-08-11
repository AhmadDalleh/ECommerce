using ECommerce.Customers.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Customers.DTOs
{
    public class CreateUpdateCustomerDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(150)]
        public string Email { get; set; }

        public string PasswordHash { get; set; } 

        [Required]
        public CustomerType Type { get; set; }

        public bool IsActive { get; set; } = true;

        public List<Guid> RoleIds { get; set; } = new();
        public List<Guid> CustomerAddresses { get; set; } = new();

        public DateTime CreatedOnUtc { get; set; } = DateTime.UtcNow;
    }
}
