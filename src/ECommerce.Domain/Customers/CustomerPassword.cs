using ECommerce.Customers.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace ECommerce.Customers
{
    public class CustomerPassword : Entity<int>
    {
        #region Property
        public Guid CustomerId { get; private set; }
        public string Password { get; private set; }
        public PasswordFormat PasswordFormatId { get; private set; }
        public string? PasswordSalt { get; private set; }
        public DateTime CreatedOnUtc { get; private set; }

        #endregion

        public Customer Customer { get; private set; }

        #region Ctor
        private CustomerPassword() { }

        public CustomerPassword(Guid customerId, string password, PasswordFormat passwordFormat, string? passwordSalt)
        {
            CustomerId = customerId;
            Password = password;
            PasswordFormatId = passwordFormat;
            PasswordSalt = passwordSalt;
            CreatedOnUtc = DateTime.UtcNow;
        }

        #endregion
    }
}
