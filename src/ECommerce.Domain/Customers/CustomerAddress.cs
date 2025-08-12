using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace ECommerce.Customers
{
    public class CustomerAddress : Entity<Guid>
    {
        #region Properties

        public Guid CustomerId { get; set; }
        public Guid AddressId { get; set; }

        #endregion

        #region CTOR
        private CustomerAddress() { }  

        public CustomerAddress(Guid customerId, Guid addressId)
        {
            Id = Guid.NewGuid();
            CustomerId = customerId;
            AddressId = addressId;
        }

        #endregion

        #region Navigation Properties
        public virtual Customer Customer { get; set; }
        public virtual Address Address { get; set; }
        #endregion
    }
}
