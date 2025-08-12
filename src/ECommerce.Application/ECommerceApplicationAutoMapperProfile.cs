using AutoMapper;
using ECommerce.Customers;
using ECommerce.Customers.DTOs;

namespace ECommerce;

public class ECommerceApplicationAutoMapperProfile : Profile
{
    public ECommerceApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */

        CreateMap<Customer, CustomerDto>();

        CreateMap<CreateUpdateCustomerDto, Customer>();

        CreateMap<Address, AddressDto>();

        CreateMap<CreateUpdateAddressDto, Address>();

        CreateMap<CustomerPassword, CustomerPasswordDto>();

        CreateMap<CreateCustomerPasswordDto, CustomerPassword>();
        CreateMap<UpdateCustomerPasswordDto, CustomerPassword>();

        // Customer Role mappings
        CreateMap<CustomerRole, CustomerRoleDto>();
        CreateMap<CreateUpdateCustomerRoleDto, CustomerRole>();

        // Customer-CustomerRole mappings
        CreateMap<CustomerCustomerRole, CustomerCustomerRoleDto>();
        CreateMap<CreateUpdateCustomerCustomerRoleDto, CustomerCustomerRole>();

        // Customer-Address mappings
        CreateMap<CustomerAddress, CustomerAddressDto>();
        CreateMap<CreateUpdateCustomerAddressDto, CustomerAddress>();
    }
}
