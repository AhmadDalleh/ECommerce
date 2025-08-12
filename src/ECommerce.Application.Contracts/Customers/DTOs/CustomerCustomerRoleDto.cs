using System;
using Volo.Abp.Application.Dtos;

namespace ECommerce.Customers.DTOs
{
    /// <summary>
    /// DTO for reading customer-role mapping information
    /// Used when retrieving customer-role relationships from the API
    /// </summary>
    public class CustomerCustomerRoleDto : EntityDto<Guid>
    {
        /// <summary>
        /// ID of the customer in this relationship
        /// Business use: Identify which customer this role assignment belongs to
        /// </summary>
        public Guid CustomerId { get; set; }

        /// <summary>
        /// ID of the role assigned to the customer
        /// Business use: Identify which role the customer has
        /// </summary>
        public Guid CustomerRoleId { get; set; }

        /// <summary>
        /// Customer information (if loaded)
        /// Business use: Display customer details when showing role assignments
        /// </summary>
        public CustomerDto Customer { get; set; }

        /// <summary>
        /// Role information (if loaded)
        /// Business use: Display role details when showing customer assignments
        /// </summary>
        public CustomerRoleDto CustomerRole { get; set; }
    }
}
