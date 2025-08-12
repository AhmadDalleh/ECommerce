using ECommerce.Customers.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Microsoft.Extensions.Logging;

namespace ECommerce.Customers
{
    /// <summary>
    /// Application service for managing customer-role mappings
    /// Implements business logic for customer-role relationship operations
    /// </summary>
    public class CustomerCustomerRoleAppService : 
        CrudAppService<
            CustomerCustomerRole,                // Domain entity
            CustomerCustomerRoleDto,             // DTO for reading
            Guid,                               // Primary key type
            PagedAndSortedResultRequestDto,     // Request DTO for pagination
            CreateUpdateCustomerCustomerRoleDto, // DTO for creating
            CreateUpdateCustomerCustomerRoleDto>, // DTO for updating
        ICustomerCustomerRoleAppService
    {
        private readonly IRepository<CustomerCustomerRole, Guid> _customerCustomerRoleRepository;
        private readonly IRepository<Customer, Guid> _customerRepository;
        private readonly IRepository<CustomerRole, Guid> _customerRoleRepository;
        private readonly IGuidGenerator _guidGenerator;
        private readonly ILogger<CustomerCustomerRoleAppService> _logger;

        public CustomerCustomerRoleAppService(
            IRepository<CustomerCustomerRole, Guid> customerCustomerRoleRepository,
            IRepository<Customer, Guid> customerRepository,
            IRepository<CustomerRole, Guid> customerRoleRepository,
            IGuidGenerator guidGenerator,
            ILogger<CustomerCustomerRoleAppService> logger) 
            : base(customerCustomerRoleRepository)
        {
            _customerCustomerRoleRepository = customerCustomerRoleRepository;
            _customerRepository = customerRepository;
            _customerRoleRepository = customerRoleRepository;
            _guidGenerator = guidGenerator;
            _logger = logger;
            
            // Define permissions for CRUD operations
            GetPolicyName = "ECommerce.CustomerCustomerRole.Read";
            GetListPolicyName = "ECommerce.CustomerCustomerRole.Read";
            CreatePolicyName = "ECommerce.CustomerCustomerRole.Create";
            UpdatePolicyName = "ECommerce.CustomerCustomerRole.Update";
            DeletePolicyName = "ECommerce.CustomerCustomerRole.Delete";
        }

        /// <summary>
        /// Get all roles assigned to a specific customer
        /// Business logic: Query mappings by customer ID and include role details
        /// </summary>
        public async Task<List<CustomerCustomerRoleDto>> GetCustomerRolesAsync(Guid customerId)
        {
            // Validate customer exists
            await _customerRepository.GetAsync(customerId);

            // Get all role mappings for this customer
            var customerRoles = await _customerCustomerRoleRepository.GetListAsync(x => x.CustomerId == customerId);
            
            // Map to DTOs
            return ObjectMapper.Map<List<CustomerCustomerRole>, List<CustomerCustomerRoleDto>>(customerRoles);
        }

        /// <summary>
        /// Get all customers assigned to a specific role
        /// Business logic: Query mappings by role ID and include customer details
        /// </summary>
        public async Task<List<CustomerCustomerRoleDto>> GetRoleCustomersAsync(Guid roleId)
        {
            // Validate role exists
            await _customerRoleRepository.GetAsync(roleId);

            // Get all customer mappings for this role
            var roleCustomers = await _customerCustomerRoleRepository.GetListAsync(x => x.CustomerRoleId == roleId);
            
            // Map to DTOs
            return ObjectMapper.Map<List<CustomerCustomerRole>, List<CustomerCustomerRoleDto>>(roleCustomers);
        }

        /// <summary>
        /// Check if a customer has a specific role
        /// Business logic: Simple existence check for authorization
        /// </summary>
        public async Task<bool> CustomerHasRoleAsync(Guid customerId, Guid roleId)
        {
            // Check if mapping exists
            var mapping = await _customerCustomerRoleRepository.FirstOrDefaultAsync(
                x => x.CustomerId == customerId && x.CustomerRoleId == roleId);
            
            return mapping != null;
        }

        /// <summary>
        /// Check if a customer has any of the specified roles
        /// Business logic: Used for feature access control
        /// </summary>
        public async Task<bool> CustomerHasAnyRoleAsync(Guid customerId, List<Guid> roleIds)
        {
            if (roleIds == null || !roleIds.Any())
                return false;

            // Check if customer has any of the specified roles
            var mapping = await _customerCustomerRoleRepository.FirstOrDefaultAsync(
                x => x.CustomerId == customerId && roleIds.Contains(x.CustomerRoleId));
            
            return mapping != null;
        }

        /// <summary>
        /// Assign multiple roles to a customer
        /// Business logic: Bulk operation for efficiency
        /// </summary>
        public async Task<List<CustomerCustomerRoleDto>> AssignMultipleRolesAsync(Guid customerId, List<Guid> roleIds)
        {
            if (roleIds == null || !roleIds.Any())
                return new List<CustomerCustomerRoleDto>();

            // Validate customer exists
            await _customerRepository.GetAsync(customerId);

            // Validate all roles exist and are active
            var roles = await _customerRoleRepository.GetListAsync(x => roleIds.Contains(x.Id) && x.Active);
            if (roles.Count != roleIds.Count)
            {
                var foundRoleIds = roles.Select(r => r.Id).ToList();
                var missingRoleIds = roleIds.Except(foundRoleIds).ToList();
                throw new ArgumentException($"Some roles do not exist or are inactive: {string.Join(", ", missingRoleIds)}");
            }

            var createdMappings = new List<CustomerCustomerRoleDto>();

            foreach (var roleId in roleIds)
            {
                // Check if mapping already exists
                var existingMapping = await _customerCustomerRoleRepository.FirstOrDefaultAsync(
                    x => x.CustomerId == customerId && x.CustomerRoleId == roleId);

                if (existingMapping == null)
                {
                    // Create new mapping
                    var newMapping = new CustomerCustomerRole(_guidGenerator.Create(), customerId, roleId);
                    await _customerCustomerRoleRepository.InsertAsync(newMapping);
                    
                    // Map to DTO and add to result
                    var mappingDto = ObjectMapper.Map<CustomerCustomerRole, CustomerCustomerRoleDto>(newMapping);
                    createdMappings.Add(mappingDto);
                }
            }

            return createdMappings;
        }

        /// <summary>
        /// Remove all roles from a customer
        /// Business logic: Used for customer deactivation or role reset
        /// </summary>
        public async Task<int> RemoveAllCustomerRolesAsync(Guid customerId)
        {
            // Validate customer exists
            await _customerRepository.GetAsync(customerId);

            // Get all current role mappings
            var currentMappings = await _customerCustomerRoleRepository.GetListAsync(x => x.CustomerId == customerId);
            
            // Delete all mappings
            foreach (var mapping in currentMappings)
            {
                await _customerCustomerRoleRepository.DeleteAsync(mapping);
            }

            return currentMappings.Count;
        }

        /// <summary>
        /// Override create to prevent duplicate mappings
        /// Business logic: A customer can't have the same role twice
        /// </summary>
        public override async Task<CustomerCustomerRoleDto> CreateAsync(CreateUpdateCustomerCustomerRoleDto input)
        {
            // Check if mapping already exists
            var existingMapping = await _customerCustomerRoleRepository.FirstOrDefaultAsync(
                x => x.CustomerId == input.CustomerId && x.CustomerRoleId == input.CustomerRoleId);

            if (existingMapping != null)
            {
                throw new InvalidOperationException($"Customer already has this role assigned.");
            }

            // Validate customer exists
            await _customerRepository.GetAsync(input.CustomerId);

            // Validate role exists and is active
            var role = await _customerRoleRepository.GetAsync(input.CustomerRoleId);
            if (!role.Active)
            {
                throw new InvalidOperationException($"Cannot assign inactive role '{role.Name}' to customer.");
            }

            // Proceed with creation
            return await base.CreateAsync(input);
        }

        /// <summary>
        /// Override delete to validate before removal
        /// Business logic: Ensure we're not removing critical role assignments
        /// </summary>
        public override async Task DeleteAsync(Guid id)
        {
            // Get the mapping
            var mapping = await _customerCustomerRoleRepository.GetAsync(id);

            // Check if this is a system role assignment
            var role = await _customerRoleRepository.GetAsync(mapping.CustomerRoleId);
            if (role.IsSystemRole)
            {
                // For system roles, we might want to prevent removal
                // This depends on your business rules
                // For now, we'll allow it but log a warning
                _logger.LogWarning($"Removing system role '{role.Name}' from customer {mapping.CustomerId}");
            }

            // Proceed with deletion
            await base.DeleteAsync(id);
        }
    }
}
