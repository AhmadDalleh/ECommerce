using System;
using Volo.Abp.Application.Dtos;

namespace ECommerce.Customers.DTOs
{
    /// <summary>
    /// DTO for reading customer role information
    /// Used when retrieving customer role data from the API
    /// </summary>
    public class CustomerRoleDto : EntityDto<Guid>
    {
        /// <summary>
        /// Display name of the role (e.g., "Administrators", "Registered")
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// System name used internally (e.g., "Administrators", "Registered")
        /// This is used for programmatic identification
        /// </summary>
        public string SystemName { get; set; }

        /// <summary>
        /// Whether customers with this role get free shipping
        /// Business rule: Administrators and VIP customers might get free shipping
        /// </summary>
        public bool FreeShipping { get; set; }

        /// <summary>
        /// Whether customers with this role are exempt from taxes
        /// Business rule: Business customers or administrators might be tax exempt
        /// </summary>
        public bool TaxExempt { get; set; }

        /// <summary>
        /// Whether this role is currently active
        /// Business rule: Inactive roles can't be assigned to new customers
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Whether this is a system role that can't be deleted
        /// Business rule: Core roles like "Administrators" and "Guests" are system roles
        /// </summary>
        public bool IsSystemRole { get; set; }

        /// <summary>
        /// Whether password lifetime is enforced for this role
        /// Business rule: Administrators and vendors might have password expiration
        /// </summary>
        public bool EnablePasswordLifeTime { get; set; }

        /// <summary>
        /// Override tax display type for this role (null = use default)
        /// Business rule: Different customer types might see different tax information
        /// </summary>
        public int? OverrideTaxDisplayType { get; set; }

        /// <summary>
        /// Default tax display type for this role
        /// Business rule: Business customers might see tax differently than consumers
        /// </summary>
        public int? DefaultTaxDisplayType { get; set; }

        /// <summary>
        /// Product ID that grants this role when purchased
        /// Business rule: Some roles are granted when customers buy specific products
        /// </summary>
        public Guid? PurchasedWithProductId { get; set; }

        /// <summary>
        /// When this role was created
        /// Audit information for compliance and tracking
        /// </summary>
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// When this role was last modified
        /// Audit information for compliance and tracking
        /// </summary>
        public DateTime? LastModificationTime { get; set; }
    }
}
