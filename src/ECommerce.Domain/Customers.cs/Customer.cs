using ECommerce.Customers.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace ECommerce.Customers.cs
{
    public class Customer : Entity<Guid>
    {
        #region Property
        
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string PasswordHash { get; private set; }
        public bool IsActive { get; private set; }
        public DateTime CreatedOnUtc { get; private set; }
        public CustomerType Type { get; private set; }

        #endregion

        #region Ctor
        private Customer() { }//EF Core needs this

        public Customer(Guid id,string name, string email, string passwordHash, CustomerType type)
            :base(id)
        {
            Name = name;
            Email = email;
            PasswordHash = passwordHash;
            IsActive = true;
            CreatedOnUtc = DateTime.UtcNow;
            Type = type;
        }
        #endregion
    }
}
