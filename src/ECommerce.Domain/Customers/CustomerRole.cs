using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace ECommerce.Customers
{
    public class CustomerRole : FullAuditedAggregateRoot<Guid>
    {
        #region Properties
        public string Name { get; private set; }

        public string SystemName {  get; private set; }

        public bool FreeShipping { get; private set; }
        public bool TaxExempt { get; private set; }
        public bool Active { get; private set; }
        public bool IsSystemRole { get; private set; }
        public bool EnablePasswordLifeTime { get; private set; }
        public int? OverrideTaxDisplayType { get; private set; }
        public int? DefaultTaxDisplayType { get; private set; }

        public Guid? PurchasedWithProductId { get; private set; }

        #endregion

        #region Navigation Properties
        public ICollection<CustomerCustomerRole> CustomerCustomerRoles { get; private set; }

        #endregion

        #region CTOR
        private CustomerRole()
        {
            CustomerCustomerRoles = new List<CustomerCustomerRole>();
        }

        public CustomerRole(Guid id,string name,string systemName) :base (id)
        {
            Name = name;
            SystemName = systemName;
            Active = true;
            CustomerCustomerRoles = new List<CustomerCustomerRole>();
        }

        #endregion

        #region Helper Methods
        public void SetActive(bool active) => Active = active;

        public void UpdateDetails(string name , string systemName)
        {
            Name = name;
            SystemName = systemName;
        }
        #endregion
    }
}
