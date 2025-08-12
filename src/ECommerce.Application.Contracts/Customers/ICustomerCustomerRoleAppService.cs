using ECommerce.Customers.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace ECommerce.Customers
{
    /// <summary>
    /// Application service interface for managing customer-role mappings
    /// Defines the contract that CustomerCustomerRoleAppService must implement
    /// </summary>
    public interface ICustomerCustomerRoleAppService : 
        ICrudAppService<
            CustomerCustomerRoleDto,             // Used to GET customer-role mapping data
            Guid,                               // Primary key type
            PagedAndSortedResultRequestDto,     // Used for GET with pagination
            CreateUpdateCustomerCustomerRoleDto, // Used for POST (create)
            CreateUpdateCustomerCustomerRoleDto> // Used for PUT (update)
    {
        /// <summary>
        /// Get all roles assigned to a specific customer
        /// Business use case: Show customer's current role assignments
        /// </summary>
        /// <param name="customerId">ID of the customer</param>
        /// <returns>List of roles assigned to the customer</returns>
        Task<List<CustomerCustomerRoleDto>> GetCustomerRolesAsync(Guid customerId);

        /// <summary>
        /// Get all customers assigned to a specific role
        /// Business use case: Show all customers with a particular role
        /// </summary>
        /// <param name="roleId">ID of the role</param>
        /// <returns>List of customers assigned to the role</returns>
        Task<List<CustomerCustomerRoleDto>> GetRoleCustomersAsync(Guid roleId);

        /// <summary>
        /// Check if a customer has a specific role
        /// Business use case: Authorization checks, feature access control
        /// </summary>
        /// <param name="customerId">ID of the customer</param>
        /// <param name="roleId">ID of the role to check</param>
        /// <returns>True if customer has the role, false otherwise</returns>
        Task<bool> CustomerHasRoleAsync(Guid customerId, Guid roleId);

        /// <summary>
        /// Check if a customer has any of the specified roles
        /// Business use case: Check if customer has access to premium features
        /// </summary>
        /// <param name="customerId">ID of the customer</param>
        /// <param name="roleIds">IDs of roles to check</param>
        /// <returns>True if customer has any of the roles, false otherwise</returns>
        Task<bool> CustomerHasAnyRoleAsync(Guid customerId, List<Guid> roleIds);

        /// <summary>
        /// Assign multiple roles to a customer
        /// Business use case: Bulk role assignment during customer creation
        /// </summary>
        /// <param name="customerId">ID of the customer</param>
        /// <param name="roleIds">IDs of roles to assign</param>
        /// <returns>List of created mappings</returns>
        Task<List<CustomerCustomerRoleDto>> AssignMultipleRolesAsync(Guid customerId, List<Guid> roleIds);

        /// <summary>
        /// Remove all roles from a customer
        /// Business use case: Customer deactivation or role reset
        /// </summary>
        /// <param name="customerId">ID of the customer</param>
        /// <returns>Number of roles removed</returns>
        Task<int> RemoveAllCustomerRolesAsync(Guid customerId);
    }
}
