using ECommerce.Customers.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace ECommerce.Customers
{
    /// <summary>
    /// Application service interface for managing customer roles
    /// Defines the contract that CustomerRoleAppService must implement
    /// </summary>
    public interface ICustomerRoleAppService : 
        ICrudAppService<
            CustomerRoleDto,                    // Used to GET customer role data
            Guid,                               // Primary key type
            PagedAndSortedResultRequestDto,     // Used for GET with pagination
            CreateUpdateCustomerRoleDto,        // Used for POST (create)
            CreateUpdateCustomerRoleDto>        // Used for PUT (update)
    {
        /// <summary>
        /// Get all active customer roles
        /// Business use case: When assigning roles to customers, only show active roles
        /// </summary>
        /// <returns>List of active customer roles</returns>
        Task<List<CustomerRoleDto>> GetActiveRolesAsync();

        /// <summary>
        /// Get customer roles by system name
        /// Business use case: Programmatically find specific roles (e.g., "Administrators")
        /// </summary>
        /// <param name="systemName">System name to search for</param>
        /// <returns>Customer role if found, null otherwise</returns>
        Task<CustomerRoleDto> GetBySystemNameAsync(string systemName);

        /// <summary>
        /// Get customer roles that provide free shipping
        /// Business use case: Calculate shipping costs based on customer roles
        /// </summary>
        /// <returns>List of roles that provide free shipping</returns>
        Task<List<CustomerRoleDto>> GetFreeShippingRolesAsync();

        /// <summary>
        /// Get customer roles that are tax exempt
        /// Business use case: Calculate taxes based on customer roles
        /// </summary>
        /// <returns>List of roles that are tax exempt</returns>
        Task<List<CustomerRoleDto>> GetTaxExemptRolesAsync();

        /// <summary>
        /// Check if a customer role is a system role
        /// Business use case: Prevent deletion of critical system roles
        /// </summary>
        /// <param name="id">Role ID to check</param>
        /// <returns>True if system role, false otherwise</returns>
        Task<bool> IsSystemRoleAsync(Guid id);

        /// <summary>
        /// Get customer roles that enforce password lifetime
        /// Business use case: Security compliance and password policy enforcement
        /// </summary>
        /// <returns>List of roles that enforce password lifetime</returns>
        Task<List<CustomerRoleDto>> GetPasswordLifetimeRolesAsync();
    }
}
