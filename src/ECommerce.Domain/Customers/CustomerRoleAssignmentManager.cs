using ECommerce.Customers.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;

namespace ECommerce.Customers
{
    /// <summary>
    /// Domain Service (Manager) for managing customer role assignments
    /// 
    /// WHY THIS IS A DOMAIN SERVICE:
    /// 1. Business logic involves multiple entities (Customer, CustomerRole, CustomerCustomerRole)
    /// 2. Complex business rules that don't belong to any single entity
    /// 3. Cross-entity validation and business logic
    /// 4. Reusable business logic that can be called from multiple places
    /// 
    /// ABP CONVENTION: Domain Services end with "Manager" not "Service"
    /// </summary>
    public class CustomerRoleAssignmentManager : DomainService
    {
        /// <summary>
        /// Validate if a customer can be assigned a specific role
        /// 
        /// BUSINESS RULES:
        /// - Customer must be active
        /// - Role must be active
        /// - Customer type must be compatible with role
        /// - Customer can't have conflicting roles
        /// </summary>
        public async Task<CustomerRoleAssignmentValidationResult> ValidateRoleAssignmentAsync(
            Customer customer, 
            CustomerRole role)
        {
            var result = new CustomerRoleAssignmentValidationResult();

            // Rule 1: Customer must be active
            if (!customer.IsActive)
            {
                result.AddError("Customer must be active to assign roles.");
                return result;
            }

            // Rule 2: Role must be active
            if (!role.Active)
            {
                result.AddError($"Role '{role.Name}' is not active and cannot be assigned.");
                return result;
            }

            // Rule 3: Customer type compatibility
            if (!IsCustomerTypeCompatibleWithRole(customer.Type, role))
            {
                result.AddError($"Customer type '{customer.Type}' is not compatible with role '{role.Name}'.");
                return result;
            }

            // Rule 4: Check for conflicting roles
            var conflictingRoles = await GetConflictingRolesAsync(customer, role);
            if (conflictingRoles.Any())
            {
                result.AddError($"Role '{role.Name}' conflicts with existing roles: {string.Join(", ", conflictingRoles.Select(r => r.Name))}");
                return result;
            }

            // Rule 5: Check role limits
            if (await WouldExceedRoleLimitAsync(customer, role))
            {
                result.AddError($"Assigning role '{role.Name}' would exceed the maximum allowed roles for this customer type.");
                return result;
            }

            result.IsValid = true;
            return result;
        }

        /// <summary>
        /// Check if customer type is compatible with role
        /// 
        /// BUSINESS RULES:
        /// - Guests can only have Guest role
        /// - Administrators can have any role
        /// - Vendors can have Vendor and Registered roles
        /// - Registered customers can have Registered and Guest roles
        /// </summary>
        private bool IsCustomerTypeCompatibleWithRole(CustomerType customerType, CustomerRole role)
        {
            return role.SystemName switch
            {
                "Guests" => customerType == CustomerType.Guest,
                "Administrators" => customerType == CustomerType.Administrator,
                "Vendors" => customerType == CustomerType.Vendor || customerType == CustomerType.Administrator,
                "Registered" => customerType == CustomerType.Registered || customerType == CustomerType.Vendor || customerType == CustomerType.Administrator,
                _ => true // Custom roles can be assigned to any type
            };
        }

        /// <summary>
        /// Get roles that conflict with the proposed role
        /// 
        /// BUSINESS RULES:
        /// - Can't have both Administrator and Guest roles
        /// - Can't have both Vendor and Guest roles
        /// - Some roles are mutually exclusive
        /// </summary>
        private async Task<List<CustomerRole>> GetConflictingRolesAsync(Customer customer, CustomerRole newRole)
        {
            var conflictingRoles = new List<CustomerRole>();

            // Get customer's current roles
            var currentRoles = customer.CustomerCustomerRoles
                .Select(ccr => ccr.CustomerRole)
                .Where(r => r != null)
                .ToList();

            foreach (var currentRole in currentRoles)
            {
                if (AreRolesConflicting(currentRole, newRole))
                {
                    conflictingRoles.Add(currentRole);
                }
            }

            return conflictingRoles;
        }

        /// <summary>
        /// Check if two roles conflict with each other
        /// 
        /// BUSINESS RULES:
        /// - Administrator and Guest are conflicting
        /// - Vendor and Guest are conflicting
        /// - Some roles are mutually exclusive
        /// </summary>
        private bool AreRolesConflicting(CustomerRole role1, CustomerRole role2)
        {
            // Administrator and Guest conflict
            if ((role1.SystemName == "Administrators" && role2.SystemName == "Guests") ||
                (role1.SystemName == "Guests" && role2.SystemName == "Administrators"))
            {
                return true;
            }

            // Vendor and Guest conflict
            if ((role1.SystemName == "Vendors" && role2.SystemName == "Guests") ||
                (role1.SystemName == "Guests" && role2.SystemName == "Vendors"))
            {
                return true;
            }

            // Same role conflict
            if (role1.SystemName == role2.SystemName)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Check if assigning this role would exceed customer's role limit
        /// 
        /// BUSINESS RULES:
        /// - Guests can only have 1 role
        /// - Regular customers can have up to 3 roles
        /// - Administrators can have unlimited roles
        /// </summary>
        private async Task<bool> WouldExceedRoleLimitAsync(Customer customer, CustomerRole newRole)
        {
            var currentRoleCount = customer.CustomerCustomerRoles.Count;
            var maxRoles = GetMaxRolesForCustomerType(customer.Type);

            // If new role is already assigned, no limit increase
            if (customer.CustomerCustomerRoles.Any(ccr => ccr.CustomerRoleId == newRole.Id))
            {
                return false;
            }

            return currentRoleCount >= maxRoles;
        }

        /// <summary>
        /// Get maximum allowed roles for customer type
        /// 
        /// BUSINESS RULES:
        /// - Guests: 1 role (Guest only)
        /// - Registered: 3 roles
        /// - Vendors: 3 roles
        /// - Administrators: Unlimited
        /// </summary>
        private int GetMaxRolesForCustomerType(CustomerType customerType)
        {
            return customerType switch
            {
                CustomerType.Guest => 1,
                CustomerType.Registered => 3,
                CustomerType.Vendor => 3,
                CustomerType.Administrator => int.MaxValue,
                _ => 3
            };
        }

        /// <summary>
        /// Calculate customer's effective permissions based on all assigned roles
        /// 
        /// BUSINESS RULES:
        /// - Free shipping: Any role with free shipping grants it
        /// - Tax exempt: Any role with tax exemption grants it
        /// - Password lifetime: Most restrictive setting applies
        /// </summary>
        public async Task<CustomerEffectivePermissions> CalculateEffectivePermissionsAsync(Customer customer)
        {
            var permissions = new CustomerEffectivePermissions();

            if (customer.CustomerCustomerRoles == null || !customer.CustomerCustomerRoles.Any())
            {
                return permissions;
            }

            var roles = customer.CustomerCustomerRoles
                .Select(ccr => ccr.CustomerRole)
                .Where(r => r != null && r.Active)
                .ToList();

            foreach (var role in roles)
            {
                // Free shipping: Any role grants it
                if (role.FreeShipping)
                {
                    permissions.HasFreeShipping = true;
                }

                // Tax exempt: Any role grants it
                if (role.TaxExempt)
                {
                    permissions.IsTaxExempt = true;
                }

                // Password lifetime: Most restrictive (true) applies
                if (role.EnablePasswordLifeTime)
                {
                    permissions.EnforcePasswordLifetime = true;
                }

                // Track role names for display
                permissions.RoleNames.Add(role.Name);
            }

            return permissions;
        }
    }

    /// <summary>
    /// Result of role assignment validation
    /// Contains validation status and any error messages
    /// </summary>
    public class CustomerRoleAssignmentValidationResult
    {
        public bool IsValid { get; set; }
        public List<string> Errors { get; set; } = new List<string>();

        public void AddError(string error)
        {
            Errors.Add(error);
        }
    }

    /// <summary>
    /// Effective permissions for a customer based on all assigned roles
    /// Calculated by combining permissions from all active roles
    /// </summary>
    public class CustomerEffectivePermissions
    {
        public bool HasFreeShipping { get; set; }
        public bool IsTaxExempt { get; set; }
        public bool EnforcePasswordLifetime { get; set; }
        public List<string> RoleNames { get; set; } = new List<string>();
    }
}
