using ECommerce.Customers.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace ECommerce.Customers
{
    /// <summary>
    /// Application service for managing customer roles
    /// Implements business logic for customer role operations
    /// </summary>
    public class CustomerRoleAppService : 
        CrudAppService<
            CustomerRole,                       // Domain entity
            CustomerRoleDto,                    // DTO for reading
            Guid,                               // Primary key type
            PagedAndSortedResultRequestDto,     // Request DTO for pagination
            CreateUpdateCustomerRoleDto,        // DTO for creating
            CreateUpdateCustomerRoleDto>,       // DTO for updating
        ICustomerRoleAppService
    {
        private readonly IRepository<CustomerRole, Guid> _customerRoleRepository;

        public CustomerRoleAppService(IRepository<CustomerRole, Guid> repository) 
            : base(repository)
        {
            _customerRoleRepository = repository;
            
            // Define permissions for CRUD operations
            GetPolicyName = "ECommerce.CustomerRole.Read";
            GetListPolicyName = "ECommerce.CustomerRole.Read";
            CreatePolicyName = "ECommerce.CustomerRole.Create";
            UpdatePolicyName = "ECommerce.CustomerRole.Update";
            DeletePolicyName = "ECommerce.CustomerRole.Delete";
        }

        /// <summary>
        /// Get all active customer roles
        /// Business logic: Only return roles that are currently active
        /// </summary>
        public async Task<List<CustomerRoleDto>> GetActiveRolesAsync()
        {
            // Query only active roles
            var activeRoles = await _customerRoleRepository.GetListAsync(x => x.Active);
            
            // Map to DTOs using AutoMapper (inherited from CrudAppService)
            return ObjectMapper.Map<List<CustomerRole>, List<CustomerRoleDto>>(activeRoles);
        }

        /// <summary>
        /// Get customer role by system name
        /// Business logic: System names are unique identifiers for roles
        /// </summary>
        public async Task<CustomerRoleDto> GetBySystemNameAsync(string systemName)
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(systemName))
                throw new ArgumentException("System name cannot be empty", nameof(systemName));

            // Find role by system name
            var role = await _customerRoleRepository.FirstOrDefaultAsync(x => x.SystemName == systemName);
            
            // Return null if not found, or map to DTO
            return role == null ? null! : ObjectMapper.Map<CustomerRole, CustomerRoleDto>(role);
        }

        /// <summary>
        /// Get customer roles that provide free shipping
        /// Business logic: Used for shipping cost calculations
        /// </summary>
        public async Task<List<CustomerRoleDto>> GetFreeShippingRolesAsync()
        {
            // Query roles with free shipping enabled
            var freeShippingRoles = await _customerRoleRepository.GetListAsync(x => x.FreeShipping && x.Active);
            
            // Map to DTOs
            return ObjectMapper.Map<List<CustomerRole>, List<CustomerRoleDto>>(freeShippingRoles);
        }

        /// <summary>
        /// Get customer roles that are tax exempt
        /// Business logic: Used for tax calculations
        /// </summary>
        public async Task<List<CustomerRoleDto>> GetTaxExemptRolesAsync()
        {
            // Query roles with tax exemption enabled
            var taxExemptRoles = await _customerRoleRepository.GetListAsync(x => x.TaxExempt && x.Active);
            
            // Map to DTOs
            return ObjectMapper.Map<List<CustomerRole>, List<CustomerRoleDto>>(taxExemptRoles);
        }

        /// <summary>
        /// Check if a customer role is a system role
        /// Business logic: System roles cannot be deleted
        /// </summary>
        public async Task<bool> IsSystemRoleAsync(Guid id)
        {
            // Get the role
            var role = await _customerRoleRepository.GetAsync(id);
            
            // Return whether it's a system role
            return role.IsSystemRole;
        }

        /// <summary>
        /// Get customer roles that enforce password lifetime
        /// Business logic: Used for security compliance
        /// </summary>
        public async Task<List<CustomerRoleDto>> GetPasswordLifetimeRolesAsync()
        {
            // Query roles with password lifetime enabled
            var passwordLifetimeRoles = await _customerRoleRepository.GetListAsync(x => x.EnablePasswordLifeTime && x.Active);
            
            // Map to DTOs
            return ObjectMapper.Map<List<CustomerRole>, List<CustomerRoleDto>>(passwordLifetimeRoles);
        }

        /// <summary>
        /// Override delete to prevent deletion of system roles
        /// Business logic: System roles are critical and cannot be removed
        /// </summary>
        public override async Task DeleteAsync(Guid id)
        {
            // Check if it's a system role
            if (await IsSystemRoleAsync(id))
            {
                throw new InvalidOperationException("Cannot delete system roles. System roles are critical for application functionality.");
            }

            // Proceed with deletion if not a system role
            await base.DeleteAsync(id);
        }

        /// <summary>
        /// Override update to prevent modification of critical system role properties
        /// Business logic: Some system role properties should not be changed
        /// </summary>
        public override async Task<CustomerRoleDto> UpdateAsync(Guid id, CreateUpdateCustomerRoleDto input)
        {
            // Get existing role
            var existingRole = await _customerRoleRepository.GetAsync(id);

            // If it's a system role, prevent changing critical properties
            if (existingRole.IsSystemRole)
            {
                // Prevent changing system name for system roles
                if (input.SystemName != existingRole.SystemName)
                {
                    throw new InvalidOperationException("Cannot change system name of system roles.");
                }

                // Prevent deactivating system roles
                if (!input.Active)
                {
                    throw new InvalidOperationException("Cannot deactivate system roles.");
                }
            }

            // Proceed with update
            return await base.UpdateAsync(id, input);
        }
    }
}
