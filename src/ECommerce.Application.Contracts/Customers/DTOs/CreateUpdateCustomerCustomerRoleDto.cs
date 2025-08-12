using System;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.Customers.DTOs
{
    /// <summary>
    /// DTO for creating and updating customer-role mappings
    /// Used when assigning roles to customers or modifying existing assignments
    /// </summary>
    public class CreateUpdateCustomerCustomerRoleDto
    {
        /// <summary>
        /// ID of the customer to assign the role to
        /// Required: Must specify which customer gets the role
        /// Business rule: Customer must exist in the system
        /// </summary>
        [Required(ErrorMessage = "Customer ID is required")]
        public Guid CustomerId { get; set; }

        /// <summary>
        /// ID of the role to assign to the customer
        /// Required: Must specify which role to assign
        /// Business rule: Role must exist and be active
        /// </summary>
        [Required(ErrorMessage = "Customer role ID is required")]
        public Guid CustomerRoleId { get; set; }
    }
}
