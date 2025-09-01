using AutoMapper;
using ECommerce.Catalog;
using ECommerce.Catalog.DTOs;
using ECommerce.Customers.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ECommerce.Customers
{
    public class CustomerAppService : CrudAppService<
        Customer,
        CustomerDto,
        Guid,
        PagedAndSortedResultRequestDto,
        CreateUpdateCustomerDto>, ICustomerAppService
    {
        private readonly IRepository<Customer,Guid> _customerRepository;
        private readonly IRepository<Address, Guid> _addressRepository;
        private readonly IMapper _mapper;
        private readonly IdentityUserManager _userManager;
        private readonly IdentityRoleManager _roleManager;
        public CustomerAppService(IRepository<Customer, Guid> repository, IRepository<Customer, Guid> customerRepository, IMapper mapper, IRepository<Address, Guid> addressRepository, IdentityUserManager userManager, IdentityRoleManager roleManager) : base(repository)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
            _addressRepository = addressRepository;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public override async Task<CustomerDto> CreateAsync(CreateUpdateCustomerDto input)
        {
            var addresses = new List<CustomerAddress>();
            var customer = new Customer(Guid.NewGuid(),input.Name,input.Email,input.PasswordHash,input.Type,addresses);
            foreach (var addressId in input.AddressIds) 
            {
                var exists = await _addressRepository.AnyAsync(a => a.Id == addressId);
                if (exists)
                {
                    customer.CustomerAddresses.Add(new CustomerAddress(Guid.NewGuid(), customer.Id, addressId));
                }
            }

            var identityUser = new IdentityUser(Guid.NewGuid(), input.Email, input.Email, CurrentTenant.Id)
            {
                Name = input.Name,
            };

            var createResult = await _userManager.CreateAsync(identityUser, input.PasswordHash);
            if (!createResult.Succeeded)
            {
                throw new BusinessException("IdentityUserCreationFailed")
                    .WithData("Errors", string.Join(", ", createResult.Errors.Select(e => e.Description)));
            }

            customer.LinkIdentityUser(identityUser.Id);
            await Repository.InsertAsync(customer,autoSave:true);

            //foreach(var roleId in input.RoleIds)
            //{
            //    customer.AddRole(roleId);
            //}
            return MapToGetOutputDto(customer);
        }

        public override async Task<CustomerDto> UpdateAsync(Guid id, CreateUpdateCustomerDto input)
        {
            var customer = await _customerRepository.GetAsync(id);

            customer.UpdateDetails(input.Name, input.Email, input.PasswordHash, input.Type, input.IsActive);


            //customer.CustomerCustomerRoles.Clear();

            //foreach(var roleId in input.RoleIds)
            //{
            //    customer.AddRole(roleId);
            //}
            customer.CustomerAddresses.Clear();

            foreach(var addressId in input.AddressIds)
            {
                customer.CustomerAddresses.Add(new CustomerAddress(Guid.NewGuid(),customer.Id, addressId));
            }
            await Repository.UpdateAsync(customer, autoSave: true);


            return MapToGetOutputDto(customer);
        }

        public override async Task<PagedResultDto<CustomerDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            var queryable = await _customerRepository.GetQueryableAsync();
            queryable = queryable.Include(c => c.CustomerAddresses);

            var totalCount = await AsyncExecuter.CountAsync(queryable);

            var list = await AsyncExecuter.ToListAsync(queryable
                .OrderBy(input.Sorting ?? "Name")
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount));

            var dtos = list.Select(customer =>
            {
                var dto = MapToGetListOutputDto(customer);
                dto.AddressIds = customer.CustomerAddresses.Select(ca => ca.AddressId).ToList();
                return dto;
            }).ToList();
            return new PagedResultDto<CustomerDto>(totalCount, dtos);
        }


        public override async Task<CustomerDto> GetAsync(Guid id)
        {
            var customer = await _customerRepository.GetAsync(id);

            var queryable = await _customerRepository.GetQueryableAsync();
            customer = await queryable
                .Include(o => o.CustomerAddresses)
                .FirstAsync(o => o.Id == id);

            var dto = _mapper.Map<Customer, CustomerDto>(customer);
            dto.AddressIds = customer.CustomerAddresses.Select(pc => pc.AddressId).ToList();
            return dto;

        }

        protected CustomerDto MapToGetOutputDto(Customer customer)
        {
            var dto = ObjectMapper.Map<Customer, CustomerDto>(customer);
            dto.AddressIds = customer.CustomerAddresses.Select(ca => ca.AddressId).ToList();
            //dto.RoleIds = customer.CustomerCustomerRoles.Select(cr => cr.CustomerRoleId).ToList();
            return dto;
        }


        public override async Task DeleteAsync(Guid id)
        {
            var customer = await Repository.GetAsync(id);
            customer.SoftDelete();
            await _customerRepository.UpdateAsync(customer);

        }

    }
}
