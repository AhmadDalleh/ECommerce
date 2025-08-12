using System;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.Customers.DTOs
{
    /// <summary>
    /// DTO for creating and updating customer roles
    /// Used when creating new roles or modifying existing ones
    /// </summary>
    public class CreateUpdateCustomerRoleDto
    {
        /// <summary>
        /// Display name of the role
        /// Required and must be unique for user experience
        /// </summary>
        [Required(ErrorMessage = "Role name is required")]
        [StringLength(100, ErrorMessage = "Role name cannot exceed 100 characters")]
        public string Name { get; set; }

        /// <summary>
        /// System name used internally
        /// Required and must be unique for programmatic identification
        /// </summary>
        [Required(ErrorMessage = "System name is required")]
        [StringLength(100, ErrorMessage = "System name cannot exceed 100 characters")]
        [RegularExpression(@"^[A-Za-z0-9_]+$", ErrorMessage = "System name can only contain letters, numbers, and underscores")]
        public string SystemName { get; set; }

        /// <summary>
        /// Whether customers with this role get free shipping
        /// Business rule: Can be used for VIP customers or loyalty programs
        /// </summary>
        public bool FreeShipping { get; set; } = false;

        /// <summary>
        /// Whether customers with this role are exempt from taxes
        /// Business rule: Used for business customers or tax-exempt organizations
        /// </summary>
        public bool TaxExempt { get; set; } = false;

        /// <summary>
        /// Whether this role is currently active
        /// Business rule: Inactive roles can't be assigned to new customers
        /// </summary>
        public bool Active { get; set; } = true;

        /// <summary>
        /// Whether this is a system role that can't be deleted
        /// Business rule: Core roles like "Administrators" and "Guests" are system roles
        /// </summary>
        public bool IsSystemRole { get; set; } = false;

        /// <summary>
        /// Whether password lifetime is enforced for this role
        /// Business rule: Used for security compliance (e.g., administrators, vendors)
        /// </summary>
        public bool EnablePasswordLifeTime { get; set; } = false;

        /// <summary>
        /// Override tax display type for this role (null = use default)
        /// Business rule: Different customer types might see different tax information
        /// </summary>
        [Range(0, 10, ErrorMessage = "Tax display type must be between 0 and 10")]
        public int? OverrideTaxDisplayType { get; set; }

        /// <summary>
        /// Default tax display type for this role
        /// Business rule: Business customers might see tax differently than consumers
        /// </summary>
        [Range(0, 10, ErrorMessage = "Tax display type must be between 0 and 10")]
        public int? DefaultTaxDisplayType { get; set; }

        /// <summary>
        /// Product ID that grants this role when purchased
        /// Business rule: Some roles are granted when customers buy specific products
        /// </summary>
        public Guid? PurchasedWithProductId { get; set; }
    }
}
