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

        public CustomerAddress(Guid id,Guid customerId, Guid addressId):base(id)
        {
            CustomerId = customerId;
            AddressId = addressId;
        }

     

        #endregion

        #region Apstrach enitiy mithod
        public override object?[] GetKeys() => new object[] {CustomerId, AddressId};
        #endregion
    }
}
