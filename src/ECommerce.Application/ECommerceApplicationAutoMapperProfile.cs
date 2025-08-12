using AutoMapper;
using ECommerce.Catalog.DTOs;
using ECommerce.Catalog;
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


        CreateMap<Category, CategoryDto>();
        CreateMap<CreateUpdateCategoryDto, Category>();

        CreateMap<Product, ProductDto>();
        CreateMap<CreateUpdateProductDto, Product>();

        CreateMap<ProductCategory, ProductCategoryDto>();
        CreateMap<CreateUpdateProductCategoryDto, ProductCategory>();

        CreateMap<ProductPhoto, ProductPhotoDto>();
        CreateMap<CreateUpdateProductPhotoDto, ProductPhoto>();

    }
}
