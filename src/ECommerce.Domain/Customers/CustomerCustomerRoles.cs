using System;
using Volo.Abp.Domain.Entities;

namespace ECommerce.Customers
{
    public class CustomerCustomerRole : Entity<Guid>
    {
        #region Properties
        public Guid CustomerId { get; private set; }
        public Guid CustomerRoleId { get; private set; }
        #endregion

        #region Navigation Properties

        public Customer Customer { get; private set; }

        public CustomerRole CustomerRole { get; private set; }

        #endregion

        #region CTOR
        private CustomerCustomerRole() { }

      

        public CustomerCustomerRole(Guid id,Guid customerId, Guid customerRoleId) : base(id)
        {
            CustomerId = customerId;
            CustomerRoleId = customerRoleId;
        }

        #endregion

    }
}