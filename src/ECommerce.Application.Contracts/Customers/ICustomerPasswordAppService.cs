using ECommerce.Customers.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace ECommerce.Customers
{
    /// <summary>
    /// Application service interface for managing customer passwords
    /// Defines the contract that CustomerPasswordAppService must implement
    /// </summary>
    public interface ICustomerPasswordAppService : ICrudAppService<
        CustomerPasswordDto,
        int,
        PagedAndSortedResultRequestDto,
        CreateCustomerPasswordDto,
        UpdateCustomerPasswordDto>
    {
        /// <summary>
        /// Get all passwords for a specific customer
        /// Business use case: Show password history for security audits
        /// </summary>
        /// <param name="customerId">ID of the customer</param>
        /// <returns>List of password history entries</returns>
        Task<List<CustomerPasswordDto>> GetCustomerPasswordsAsync(Guid customerId);

        /// <summary>
        /// Get the most recent password for a customer
        /// Business use case: Get current password for verification
        /// </summary>
        /// <param name="customerId">ID of the customer</param>
        /// <returns>Most recent password entry, or null if none exists</returns>
        Task<CustomerPasswordDto> GetLatestPasswordAsync(Guid customerId);

        /// <summary>
        /// Change customer password with verification
        /// Business use case: Secure password change process
        /// </summary>
        /// <param name="input">Password change request with verification</param>
        /// <returns>Updated password entry</returns>
        Task<CustomerPasswordDto> ChangePasswordAsync(UpdateCustomerPasswordDto input);

        /// <summary>
        /// Validate customer password
        /// Business use case: Login verification and password strength checking
        /// </summary>
        /// <param name="customerId">ID of the customer</param>
        /// <param name="password">Password to validate</param>
        /// <returns>True if password is valid, false otherwise</returns>
        Task<bool> ValidatePasswordAsync(Guid customerId, string password);
    }
}
