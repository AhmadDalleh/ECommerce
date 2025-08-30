using ECommerce.Customers.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Guids;

namespace ECommerce.Customers
{
    public class Customer : Entity<Guid>
    {
        #region Properties
        
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string PasswordHash { get; private set; }
        public bool IsActive { get; private set; }
        public DateTime CreatedOnUtc { get; private set; }
        public CustomerType Type { get; private set; }

        #endregion

        #region Navigation Properties

        public ICollection<CustomerPassword> CustomerPasswords { get; private set; }

        public ICollection<CustomerCustomerRole> CustomerCustomerRoles { get; private set; } = new List<CustomerCustomerRole>();

        public ICollection<CustomerAddress> CustomerAddresses { get; private set; }

        #endregion


        #region Ctor
        private Customer() { }//EF Core needs this

        public Customer(Guid id, string name, string email, string passwordHash, CustomerType type, ICollection<CustomerAddress> customerAddresses)
            : base(id)
        {
            Name = name;
            Email = email;
            PasswordHash = passwordHash;
            IsActive = true;
            CreatedOnUtc = DateTime.UtcNow;
            Type = type;
            CustomerAddresses = customerAddresses;
        }
        #endregion

        #region Helper Methods

        public void AddRole(Guid roleId)
        {
            if(!CustomerCustomerRoles.Any(x=>x.CustomerRoleId == roleId))
            {
                CustomerCustomerRoles.Add(new CustomerCustomerRole(Guid.NewGuid(), this.Id, roleId));
            }
        }

        public void RemoveRole(Guid roleId) 
        {
            var mapping = CustomerCustomerRoles.FirstOrDefault(x=>x.CustomerRoleId == roleId);
            if(mapping != null) CustomerCustomerRoles.Remove(mapping);
        }

        public void UpdateDetails(string name, string email, string passwordHash, CustomerType type, bool isActive)
        {
            Name = name;
            Email = email;
            PasswordHash = passwordHash;    
            Type = type;
            IsActive = isActive;
        }
        #endregion
    }
}
