using ECommerce.Customers.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace ECommerce.Customers
{
    /// <summary>
    /// Application service for managing customer passwords
    /// Implements business logic for password management and security
    /// </summary>
    public class CustomerPasswordAppService : CrudAppService<
        CustomerPassword,
        CustomerPasswordDto,
        int,
        PagedAndSortedResultRequestDto,
        CreateCustomerPasswordDto,
        UpdateCustomerPasswordDto>,
        ICustomerPasswordAppService
    {
        private readonly IRepository<CustomerPassword, int> _customerPasswordRepository;
        private readonly IRepository<Customer, Guid> _customerRepository;

        public CustomerPasswordAppService(
            IRepository<CustomerPassword, int> customerPasswordRepository,
            IRepository<Customer, Guid> customerRepository) 
            : base(customerPasswordRepository)
        {
            _customerPasswordRepository = customerPasswordRepository;
            _customerRepository = customerRepository;
            
            // Define permissions for CRUD operations
            GetPolicyName = "ECommerce.CustomerPassword.Read";
            GetListPolicyName = "ECommerce.CustomerPassword.Read";
            CreatePolicyName = "ECommerce.CustomerPassword.Create";
            UpdatePolicyName = "ECommerce.CustomerPassword.Update";
            DeletePolicyName = "ECommerce.CustomerPassword.Delete";
        }

        /// <summary>
        /// Get all passwords for a specific customer
        /// Business logic: Query passwords by customer ID for security audits
        /// </summary>
        public async Task<List<CustomerPasswordDto>> GetCustomerPasswordsAsync(Guid customerId)
        {
            // Validate customer exists
            await _customerRepository.GetAsync(customerId);

            // Get all password entries for this customer, ordered by creation date
            var passwords = await _customerPasswordRepository.GetListAsync(x => x.CustomerId == customerId);
            var orderedPasswords = passwords.OrderByDescending(p => p.CreatedOnUtc).ToList();
            
            // Map to DTOs
            return ObjectMapper.Map<List<CustomerPassword>, List<CustomerPasswordDto>>(orderedPasswords);
        }

        /// <summary>
        /// Get the most recent password for a customer
        /// Business logic: Return the latest password entry for verification
        /// </summary>
        public async Task<CustomerPasswordDto> GetLatestPasswordAsync(Guid customerId)
        {
            // Validate customer exists
            await _customerRepository.GetAsync(customerId);

            // Get the most recent password entry
            var passwords = await _customerPasswordRepository.GetListAsync(x => x.CustomerId == customerId);
            var latestPassword = passwords.OrderByDescending(p => p.CreatedOnUtc).FirstOrDefault();
            
            // Return null if no passwords exist, or map to DTO
            return latestPassword == null ? null! : ObjectMapper.Map<CustomerPassword, CustomerPasswordDto>(latestPassword);
        }

        /// <summary>
        /// Change customer password with verification
        /// Business logic: Secure password change with current password verification
        /// </summary>
        public async Task<CustomerPasswordDto> ChangePasswordAsync(UpdateCustomerPasswordDto input)
        {
            // Validate customer exists
            await _customerRepository.GetAsync(input.CustomerId);

            // Verify current password
            var currentPassword = await GetLatestPasswordAsync(input.CustomerId);
            if (currentPassword == null)
            {
                throw new InvalidOperationException("Customer has no password to verify against.");
            }

            // TODO: Implement actual password verification logic
            // This would typically involve hashing and comparing the current password
            // For now, we'll just create a new password entry

            // Create new password entry
            var newPassword = new CustomerPassword(
                input.CustomerId,
                input.NewPassword,
                input.PasswordFormatId,
                input.PasswordSalt);

            await _customerPasswordRepository.InsertAsync(newPassword);
            
            // Map to DTO and return
            return ObjectMapper.Map<CustomerPassword, CustomerPasswordDto>(newPassword);
        }

        /// <summary>
        /// Validate customer password
        /// Business logic: Check if provided password matches stored password
        /// </summary>
        public async Task<bool> ValidatePasswordAsync(Guid customerId, string password)
        {
            // Validate customer exists
            await _customerRepository.GetAsync(customerId);

            // Get the most recent password
            var latestPassword = await GetLatestPasswordAsync(customerId);
            if (latestPassword == null)
            {
                return false; // No password to validate against
            }

            // TODO: Implement actual password validation logic
            // This would typically involve hashing the provided password and comparing
            // For now, we'll return false as a security measure
            return false;
        }

        /// <summary>
        /// Override create to add business logic
        /// Business logic: Ensure password meets security requirements
        /// </summary>
        public override async Task<CustomerPasswordDto> CreateAsync(CreateCustomerPasswordDto input)
        {
            // Validate customer exists
            await _customerRepository.GetAsync(input.CustomerId);

            // Validate password strength (basic validation)
            if (string.IsNullOrWhiteSpace(input.Password) || input.Password.Length < 8)
            {
                throw new ArgumentException("Password must be at least 8 characters long.");
            }

            // Proceed with creation
            return await base.CreateAsync(input);
        }

        /// <summary>
        /// Override delete to add business logic
        /// Business logic: Prevent deletion of critical password history
        /// </summary>
        public override async Task DeleteAsync(int id)
        {
            // Get the password entry
            var password = await _customerPasswordRepository.GetAsync(id);

            // Check if this is the customer's only password
            var passwords = await _customerPasswordRepository.GetListAsync(x => x.CustomerId == password.CustomerId);
            var passwordCount = passwords.Count;
            if (passwordCount <= 1)
            {
                throw new InvalidOperationException("Cannot delete the customer's only password. Customers must have at least one password.");
            }

            // Proceed with deletion
            await base.DeleteAsync(id);
        }
    }
}
