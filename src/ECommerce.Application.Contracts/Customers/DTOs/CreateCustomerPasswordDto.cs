using ECommerce.Customers.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Customers.DTOs
{
    public class CreateCustomerPasswordDto
    {
        [Required]
        public Guid CustomerId { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public PasswordFormat PasswordFormatId { get; set; }

        public string PasswordSalt { get; set; }

        public DateTime CreatedOnUtc { get;  set; } = DateTime.UtcNow;


    }
}
