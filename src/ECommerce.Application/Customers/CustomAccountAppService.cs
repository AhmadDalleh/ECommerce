using ECommerce.Permissions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Account;
using Volo.Abp.Account.Emailing;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;

namespace ECommerce.Customers
{
    public class CustomAccountAppService : AccountAppService
    {
        private readonly IRepository<Customer, Guid> _customerRepository;

        public CustomAccountAppService(IdentityUserManager userManager,
            IIdentityRoleRepository roleRepository, 
            IAccountEmailer accountEmailer, 
            IdentitySecurityLogManager identitySecurityLogManager, 
            IOptions<IdentityOptions> identityOptions,
            IRepository<Customer, Guid> customerRepository) :
            base(userManager, roleRepository, accountEmailer, identitySecurityLogManager, identityOptions)
        {
            _customerRepository = customerRepository;
        }
        public override async Task<IdentityUserDto> RegisterAsync(RegisterDto input)
        {
            var user = await base.RegisterAsync(input);
           var  customerAddress = new List<CustomerAddress>();  

            var customer = new Customer(user.Id, user.UserName,user.Email,input.Password,Enums.CustomerType.Custoemr,customerAddress);
            await _customerRepository.InsertAsync(customer);

            var roleName = "CUSTOMER"; // default
            var customerRole = await RoleRepository.FindByNormalizedNameAsync(roleName.ToUpperInvariant());

            if (customerRole != null)
            {
                var identityUser = await UserManager.GetByIdAsync(user.Id);
                await UserManager.AddToRoleAsync(identityUser, customerRole.Name);
            }

            return user;
        }
    }
}
