using System;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.Customers.DTOs
{
    /// <summary>
    /// DTO for creating and updating customer-address mappings
    /// Used when assigning addresses to customers or modifying existing assignments
    /// </summary>
    public class CreateUpdateCustomerAddressDto
    {
        /// <summary>
        /// ID of the customer to assign the address to
        /// Required: Must specify which customer gets the address
        /// Business rule: Customer must exist in the system
        /// </summary>
        [Required(ErrorMessage = "Customer ID is required")]
        public Guid CustomerId { get; set; }

        /// <summary>
        /// ID of the address to assign to the customer
        /// Required: Must specify which address to assign
        /// Business rule: Address must exist in the system
        /// </summary>
        [Required(ErrorMessage = "Address ID is required")]
        public Guid AddressId { get; set; }

        /// <summary>
        /// Whether this is the customer's primary address
        /// Business rule: Used for default shipping/billing address
        /// </summary>
        public bool IsPrimary { get; set; } = false;

        /// <summary>
        /// Address type (e.g., Shipping, Billing, Both)
        /// Business rule: Different address types serve different purposes
        /// </summary>
        public string AddressType { get; set; } = "Both";

        /// <summary>
        /// Custom name for this address (e.g., "Home", "Work", "Summer House")
        /// Business rule: Helps customers organize multiple addresses
        /// </summary>
        [StringLength(100, ErrorMessage = "Address name cannot exceed 100 characters")]
        public string AddressName { get; set; }

        /// <summary>
        /// Whether this address is currently active
        /// Business rule: Inactive addresses can't be used for orders
        /// </summary>
        public bool IsActive { get; set; } = true;
    }
}
