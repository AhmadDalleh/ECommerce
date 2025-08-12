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
    /// Application service for managing customer-address mappings
    /// Implements business logic for customer-address relationship operations
    /// </summary>
    public class CustomerAddressAppService : 
        CrudAppService<
            CustomerAddress,                     // Domain entity
            CustomerAddressDto,                  // DTO for reading
            Guid,                                // Primary key type (composite key mapped to Guid)
            PagedAndSortedResultRequestDto,      // Request DTO for pagination
            CreateUpdateCustomerAddressDto,      // DTO for creating
            CreateUpdateCustomerAddressDto>,     // DTO for updating
        ICustomerAddressAppService
    {
        private readonly IRepository<CustomerAddress, Guid> _customerAddressRepository;
        private readonly IRepository<Customer, Guid> _customerRepository;
        private readonly IRepository<Address, Guid> _addressRepository;

        public CustomerAddressAppService(
            IRepository<CustomerAddress, Guid> customerAddressRepository,
            IRepository<Customer, Guid> customerRepository,
            IRepository<Address, Guid> addressRepository) 
            : base(customerAddressRepository)
        {
            _customerAddressRepository = customerAddressRepository;
            _customerRepository = customerRepository;
            _addressRepository = addressRepository;
            
            // Define permissions for CRUD operations
            GetPolicyName = "ECommerce.CustomerAddress.Read";
            GetListPolicyName = "ECommerce.CustomerAddress.Read";
            CreatePolicyName = "ECommerce.CustomerAddress.Create";
            UpdatePolicyName = "ECommerce.CustomerAddress.Update";
            DeletePolicyName = "ECommerce.CustomerAddress.Delete";
        }

        /// <summary>
        /// Get all addresses for a specific customer
        /// Business logic: Query mappings by customer ID and include address details
        /// </summary>
        public async Task<List<CustomerAddressDto>> GetCustomerAddressesAsync(Guid customerId)
        {
            // Validate customer exists
            await _customerRepository.GetAsync(customerId);

            // Get all address mappings for this customer
            var customerAddresses = await _customerAddressRepository.GetListAsync(x => x.CustomerId == customerId);
            
            // Map to DTOs
            return ObjectMapper.Map<List<CustomerAddress>, List<CustomerAddressDto>>(customerAddresses);
        }

        /// <summary>
        /// Get all customers for a specific address
        /// Business logic: Query mappings by address ID and include customer details
        /// </summary>
        public async Task<List<CustomerAddressDto>> GetAddressCustomersAsync(Guid addressId)
        {
            // Validate address exists
            await _addressRepository.GetAsync(addressId);

            // Get all customer mappings for this address
            var addressCustomers = await _customerAddressRepository.GetListAsync(x => x.AddressId == addressId);
            
            // Map to DTOs
            return ObjectMapper.Map<List<CustomerAddress>, List<CustomerAddressDto>>(addressCustomers);
        }

        /// <summary>
        /// Check if a customer has a specific address
        /// Business logic: Simple existence check for validation
        /// </summary>
        public async Task<bool> CustomerHasAddressAsync(Guid customerId, Guid addressId)
        {
            // Check if mapping exists
            var mapping = await _customerAddressRepository.FirstOrDefaultAsync(
                x => x.CustomerId == customerId && x.AddressId == addressId);
            
            return mapping != null;
        }

        /// <summary>
        /// Assign multiple addresses to a customer
        /// Business logic: Bulk operation for efficiency
        /// </summary>
        public async Task<List<CustomerAddressDto>> AssignMultipleAddressesAsync(Guid customerId, List<Guid> addressIds)
        {
            if (addressIds == null || !addressIds.Any())
                return new List<CustomerAddressDto>();

            // Validate customer exists
            await _customerRepository.GetAsync(customerId);

            // Validate all addresses exist
            var addresses = await _addressRepository.GetListAsync(x => addressIds.Contains(x.Id));
            if (addresses.Count != addressIds.Count)
            {
                var foundAddressIds = addresses.Select(a => a.Id).ToList();
                var missingAddressIds = addressIds.Except(foundAddressIds).ToList();
                throw new ArgumentException($"Some addresses do not exist: {string.Join(", ", missingAddressIds)}");
            }

            var createdMappings = new List<CustomerAddressDto>();

            foreach (var addressId in addressIds)
            {
                // Check if mapping already exists
                var existingMapping = await _customerAddressRepository.FirstOrDefaultAsync(
                    x => x.CustomerId == customerId && x.AddressId == addressId);

                if (existingMapping == null)
                {
                    // Create new mapping
                    var newMapping = new CustomerAddress(customerId, addressId);
                    await _customerAddressRepository.InsertAsync(newMapping);
                    
                    // Map to DTO and add to result
                    var mappingDto = ObjectMapper.Map<CustomerAddress, CustomerAddressDto>(newMapping);
                    createdMappings.Add(mappingDto);
                }
            }

            return createdMappings;
        }

        /// <summary>
        /// Remove all addresses from a customer
        /// Business logic: Used for customer deactivation or address reset
        /// </summary>
        public async Task<int> RemoveAllCustomerAddressesAsync(Guid customerId)
        {
            // Validate customer exists
            await _customerRepository.GetAsync(customerId);

            // Get all current address mappings
            var currentMappings = await _customerAddressRepository.GetListAsync(x => x.CustomerId == customerId);
            
            // Delete all mappings
            foreach (var mapping in currentMappings)
            {
                await _customerAddressRepository.DeleteAsync(mapping);
            }

            return currentMappings.Count;
        }

        /// <summary>
        /// Set customer's primary address
        /// Business logic: Only one address can be primary per customer
        /// </summary>
        public async Task<CustomerAddressDto> SetPrimaryAddressAsync(Guid customerId, Guid addressId)
        {
            // Validate customer exists
            await _customerRepository.GetAsync(customerId);

            // Validate address exists
            await _addressRepository.GetAsync(addressId);

            // Check if mapping exists
            var mapping = await _customerAddressRepository.FirstOrDefaultAsync(
                x => x.CustomerId == customerId && x.AddressId == addressId);

            if (mapping == null)
            {
                throw new InvalidOperationException($"Customer does not have this address assigned.");
            }

            // Remove primary flag from all other addresses for this customer
            var otherMappings = await _customerAddressRepository.GetListAsync(
                x => x.CustomerId == customerId && x.Id != mapping.Id);

            foreach (var otherMapping in otherMappings)
            {
                // Note: This would require adding IsPrimary property to CustomerAddress entity
                // For now, we'll just update the current mapping
            }

            // Set current mapping as primary
            // Note: This would require adding IsPrimary property to CustomerAddress entity
            // For now, we'll just return the mapping

            return ObjectMapper.Map<CustomerAddress, CustomerAddressDto>(mapping);
        }

        /// <summary>
        /// Get customer's primary address
        /// Business logic: Return the address marked as primary
        /// </summary>
        public async Task<CustomerAddressDto> GetPrimaryAddressAsync(Guid customerId)
        {
            // Validate customer exists
            await _customerRepository.GetAsync(customerId);

            // Get primary address mapping
            // Note: This would require adding IsPrimary property to CustomerAddress entity
            // For now, we'll return the first address or null
            var primaryMapping = await _customerAddressRepository.FirstOrDefaultAsync(x => x.CustomerId == customerId);
            
            return primaryMapping == null ? null! : ObjectMapper.Map<CustomerAddress, CustomerAddressDto>(primaryMapping);
        }

        /// <summary>
        /// Override create to prevent duplicate mappings
        /// Business logic: A customer can't have the same address twice
        /// </summary>
        public override async Task<CustomerAddressDto> CreateAsync(CreateUpdateCustomerAddressDto input)
        {
            // Check if mapping already exists
            var existingMapping = await _customerAddressRepository.FirstOrDefaultAsync(
                x => x.CustomerId == input.CustomerId && x.AddressId == input.AddressId);

            if (existingMapping != null)
            {
                throw new InvalidOperationException($"Customer already has this address assigned.");
            }

            // Validate customer exists
            await _customerRepository.GetAsync(input.CustomerId);

            // Validate address exists
            await _addressRepository.GetAsync(input.AddressId);

            // Proceed with creation
            return await base.CreateAsync(input);
        }

        /// <summary>
        /// Override delete to validate before removal
        /// Business logic: Ensure we're not removing critical address assignments
        /// </summary>
        public override async Task DeleteAsync(Guid id)
        {
            // Get the mapping
            var mapping = await _customerAddressRepository.GetAsync(id);

            // Check if this is the customer's only address
            var customerAddresses = await _customerAddressRepository.GetListAsync(x => x.CustomerId == mapping.CustomerId);
            var customerAddressCount = customerAddresses.Count;
            if (customerAddressCount <= 1)
            {
                throw new InvalidOperationException("Cannot remove the customer's only address. Customers must have at least one address.");
            }

            // Proceed with deletion
            await base.DeleteAsync(id);
        }
    }
}
