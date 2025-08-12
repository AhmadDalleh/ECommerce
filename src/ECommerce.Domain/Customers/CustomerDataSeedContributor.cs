using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ECommerce.Customers.Enums;
using Microsoft.Extensions.Logging;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.Uow;

namespace ECommerce.Customers
{
    /* Creates initial data that is needed to test the Customer module */
    public class CustomerDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IRepository<Customer, Guid> _customerRepository;
        private readonly IRepository<CustomerRole, Guid> _customerRoleRepository;
        private readonly IRepository<Address, Guid> _addressRepository;
        private readonly IRepository<CustomerPassword, int> _customerPasswordRepository;
        private readonly IRepository<CustomerAddress, Guid> _customerAddressRepository;
        private readonly IRepository<CustomerCustomerRole, Guid> _customerCustomerRoleRepository;
        private readonly IGuidGenerator _guidGenerator;
        private readonly ILogger<CustomerDataSeedContributor> _logger;

        public CustomerDataSeedContributor(
            IRepository<Customer, Guid> customerRepository,
            IRepository<CustomerRole, Guid> customerRoleRepository,
            IRepository<Address, Guid> addressRepository,
            IRepository<CustomerPassword, int> customerPasswordRepository,
            IRepository<CustomerAddress, Guid> customerAddressRepository,
            IRepository<CustomerCustomerRole, Guid> customerCustomerRoleRepository,
            IGuidGenerator guidGenerator,
            ILogger<CustomerDataSeedContributor> logger)
        {
            _customerRepository = customerRepository;
            _customerRoleRepository = customerRoleRepository;
            _addressRepository = addressRepository;
            _customerPasswordRepository = customerPasswordRepository;
            _customerAddressRepository = customerAddressRepository;
            _customerCustomerRoleRepository = customerCustomerRoleRepository;
            _guidGenerator = guidGenerator;
            _logger = logger;
        }

        [UnitOfWork]
        public virtual async Task SeedAsync(DataSeedContext context)
        {
            _logger.LogInformation("Seeding Customer module data...");

            await SeedCustomerRolesAsync();
            await SeedAddressesAsync();
            await SeedCustomersAsync();
            await SeedCustomerPasswordsAsync();
            await SeedCustomerAddressesAsync();
            await SeedCustomerRoleMappingsAsync();

            _logger.LogInformation("Customer module data seeding completed successfully!");
        }

        private async Task SeedCustomerRolesAsync()
        {
            if (await _customerRoleRepository.GetCountAsync() > 0) return;

            var roles = new List<CustomerRole>
            {
                new CustomerRole(_guidGenerator.Create(), "Administrators", "Administrators"),
                new CustomerRole(_guidGenerator.Create(), "Registered", "Registered"),
                new CustomerRole(_guidGenerator.Create(), "Guests", "Guests"),
                new CustomerRole(_guidGenerator.Create(), "Vendors", "Vendors")
            };

            foreach (var role in roles)
            {
                await _customerRoleRepository.InsertAsync(role);
            }
        }

        private async Task SeedAddressesAsync()
        {
            if (await _addressRepository.GetCountAsync() > 0) return;

            var addresses = new List<Address>
            {
                new Address(_guidGenerator.Create(), "John", "Doe", "john.doe@example.com", "United States", "New York"),
                new Address(_guidGenerator.Create(), "Jane", "Smith", "jane.smith@example.com", "United States", "Los Angeles"),
                new Address(_guidGenerator.Create(), "Bob", "Johnson", "bob.johnson@example.com", "Canada", "Toronto"),
                new Address(_guidGenerator.Create(), "Alice", "Brown", "alice.brown@example.com", "United Kingdom", "London"),
            };

            foreach (var address in addresses)
            {
                await _addressRepository.InsertAsync(address);
            }
        }

        private async Task SeedCustomersAsync()
        {
            if (await _customerRepository.GetCountAsync() > 0) return;

            var customers = new List<Customer>
            {
                new Customer(_guidGenerator.Create(), "John Doe", "john.doe@example.com", "hashed_password_123", CustomerType.Registered, new List<CustomerAddress>()),
                new Customer(_guidGenerator.Create(), "Jane Smith", "jane.smith@example.com", "hashed_password_456", CustomerType.Registered, new List<CustomerAddress>()),
                new Customer(_guidGenerator.Create(), "Bob Johnson", "bob.johnson@example.com", "hashed_password_789", CustomerType.Vendor, new List<CustomerAddress>()),
                new Customer(_guidGenerator.Create(), "Alice Brown", "alice.brown@example.com", "hashed_password_012", CustomerType.Administrator, new List<CustomerAddress>()),
                new Customer(_guidGenerator.Create(), "Guest User", "guest@example.com", "hashed_password_guest", CustomerType.Guest, new List<CustomerAddress>())
            };

            foreach (var customer in customers)
            {
                await _customerRepository.InsertAsync(customer);
            }
        }

        private async Task SeedCustomerPasswordsAsync()
        {
            if (await _customerPasswordRepository.GetCountAsync() > 0) return;

            var customers = await _customerRepository.GetListAsync();
            var passwords = new List<CustomerPassword>();

            foreach (var customer in customers)
            {
                passwords.Add(new CustomerPassword(customer.Id, "hashed_password_current", PasswordFormat.Hashed, "salt_" + customer.Id.ToString("N")[..8]));
                if (customer.Type != CustomerType.Guest)
                {
                    passwords.Add(new CustomerPassword(customer.Id, "hashed_password_old_1", PasswordFormat.Hashed, "salt_old_1_" + customer.Id.ToString("N")[..8]));
                }
            }

            foreach (var password in passwords)
            {
                await _customerPasswordRepository.InsertAsync(password);
            }
        }

        private async Task SeedCustomerAddressesAsync()
        {
            if (await _customerAddressRepository.GetCountAsync() > 0) return;

            var customers = await _customerRepository.GetListAsync();
            var addresses = await _addressRepository.GetListAsync();
            var customerAddresses = new List<CustomerAddress>();

            for (int i = 0; i < Math.Min(customers.Count, addresses.Count); i++)
            {
                customerAddresses.Add(new CustomerAddress(customers[i].Id, addresses[i].Id));
            }

            if (customers.Count > 1 && addresses.Count > 1)
            {
                customerAddresses.Add(new CustomerAddress(customers[0].Id, addresses[1].Id));
            }

            foreach (var customerAddress in customerAddresses)
            {
                await _customerAddressRepository.InsertAsync(customerAddress);
            }
        }

        private async Task SeedCustomerRoleMappingsAsync()
        {
            if (await _customerCustomerRoleRepository.GetCountAsync() > 0) return;

            var customers = await _customerRepository.GetListAsync();
            var roles = await _customerRoleRepository.GetListAsync();

            foreach (var customer in customers)
            {
                CustomerRole? role = customer.Type switch
                {
                    CustomerType.Administrator => roles.Find(r => r.SystemName == "Administrators"),
                    CustomerType.Registered => roles.Find(r => r.SystemName == "Registered"),
                    CustomerType.Vendor => roles.Find(r => r.SystemName == "Vendors"),
                    CustomerType.Guest => roles.Find(r => r.SystemName == "Guests"),
                    _ => null
                };

                if (role != null)
                {
                    await _customerCustomerRoleRepository.InsertAsync(new CustomerCustomerRole(_guidGenerator.Create(), customer.Id, role.Id));
                }
            }
        }
    }
}
