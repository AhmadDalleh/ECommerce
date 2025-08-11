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

        CreateMap<CustomerAddress, CustomerAddressDto>();

        CreateMap<CustomerPassword, CustomerPasswordDto>();

        CreateMap<CreateCustomerPasswordDto, CustomerPassword>();
    }
}
