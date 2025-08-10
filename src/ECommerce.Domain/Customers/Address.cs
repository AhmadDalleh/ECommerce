using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace ECommerce.Customers
{
    public class Address : Entity<Guid>
    {
        #region Properties
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Email { get; private set; }
        public string Company { get; private set; }
        public string Country { get; private set; }
        public string City { get; private set; }
        public string Address1 { get; private set; }
        public string Address2 { get; private set; }
        public string ZipPostalCode  { get; private set; }
        public string PhoneNumber { get; private set; }

        public DateTime CreatedOnUtc { get; private set; }

        #endregion

        #region CTOR
        private Address() { }

        public Address(Guid id, string firstName, string lastName, string email, string country, string city) : base(id)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Country = country;
            City = city;
            CreatedOnUtc = DateTime.UtcNow;
        }
        #endregion
    }
}
