using ECommerce.Customers.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace ECommerce.Customers
{
    /// <summary>
    /// Application service interface for managing customer-address mappings
    /// Defines the contract that CustomerAddressAppService must implement
    /// </summary>
    public interface ICustomerAddressAppService : 
        ICrudAppService<
            CustomerAddressDto,                  // Used to GET customer-address mapping data
            Guid,                                // Primary key type (composite key mapped to Guid)
            PagedAndSortedResultRequestDto,      // Used for GET with pagination
            CreateUpdateCustomerAddressDto,      // Used for POST (create)
            CreateUpdateCustomerAddressDto>      // Used for PUT (update)
    {
        /// <summary>
        /// Get all addresses for a specific customer
        /// Business use case: Show customer's address book
        /// </summary>
        /// <param name="customerId">ID of the customer</param>
        /// <returns>List of addresses associated with the customer</returns>
        Task<List<CustomerAddressDto>> GetCustomerAddressesAsync(Guid customerId);

        /// <summary>
        /// Get all customers for a specific address
        /// Business use case: Show which customers share an address
        /// </summary>
        /// <param name="addressId">ID of the address</param>
        /// <returns>List of customers associated with the address</returns>
        Task<List<CustomerAddressDto>> GetAddressCustomersAsync(Guid addressId);

        /// <summary>
        /// Check if a customer has a specific address
        /// Business use case: Prevent duplicate address assignments
        /// </summary>
        /// <param name="customerId">ID of the customer</param>
        /// <param name="addressId">ID of the address</param>
        /// <returns>True if customer has the address, false otherwise</returns>
        Task<bool> CustomerHasAddressAsync(Guid customerId, Guid addressId);

        /// <summary>
        /// Assign multiple addresses to a customer
        /// Business use case: Bulk address assignment during customer creation
        /// </summary>
        /// <param name="customerId">ID of the customer</param>
        /// <param name="addressIds">IDs of addresses to assign</param>
        /// <returns>List of created mappings</returns>
        Task<List<CustomerAddressDto>> AssignMultipleAddressesAsync(Guid customerId, List<Guid> addressIds);

        /// <summary>
        /// Remove all addresses from a customer
        /// Business use case: Customer deactivation or address reset
        /// </summary>
        /// <param name="customerId">ID of the customer</param>
        /// <returns>Number of addresses removed</returns>
        Task<int> RemoveAllCustomerAddressesAsync(Guid customerId);

        /// <summary>
        /// Set customer's primary address
        /// Business use case: Mark one address as primary for shipping/billing
        /// </summary>
        /// <param name="customerId">ID of the customer</param>
        /// <param name="addressId">ID of the address to set as primary</param>
        /// <returns>Updated customer address mapping</returns>
        Task<CustomerAddressDto> SetPrimaryAddressAsync(Guid customerId, Guid addressId);

        /// <summary>
        /// Get customer's primary address
        /// Business use case: Get default shipping/billing address
        /// </summary>
        /// <param name="customerId">ID of the customer</param>
        /// <returns>Primary address mapping, or null if none set</returns>
        Task<CustomerAddressDto> GetPrimaryAddressAsync(Guid customerId);
    }
}
