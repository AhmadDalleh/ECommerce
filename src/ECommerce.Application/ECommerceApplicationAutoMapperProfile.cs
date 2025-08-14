using AutoMapper;
using ECommerce.Catalog.DTOs;
using ECommerce.Catalog;
using ECommerce.Customers;
using ECommerce.Customers.DTOs;
using ECommerce.Orders.DTOs;
using ECommerce.Orders;

namespace ECommerce;

public class ECommerceApplicationAutoMapperProfile : Profile
{
    public ECommerceApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */

        #region Cusomer Mappers 
        CreateMap<Customer, CustomerDto>();

        CreateMap<CreateUpdateCustomerDto, Customer>();

        CreateMap<Address, AddressDto>();

        CreateMap<CreateUpdateAddressDto, Address>();

        CreateMap<CustomerAddress, CustomerAddressDto>();

        CreateMap<CustomerPassword, CustomerPasswordDto>();

        CreateMap<CreateCustomerPasswordDto, CustomerPassword>();

        #endregion


        #region Catlog Mappers 

        CreateMap<Category, CategoryDto>();
        CreateMap<CreateUpdateCategoryDto, Category>();

        CreateMap<Product, ProductDto>();
        CreateMap<CreateUpdateProductDto, Product>();

        CreateMap<ProductCategory, ProductCategoryDto>();
        CreateMap<CreateUpdateProductCategoryDto, ProductCategory>();

        CreateMap<ProductPhoto, ProductPhotoDto>();
        CreateMap<CreateUpdateProductPhotoDto, ProductPhoto>();

        #endregion

        #region Order Mappers 
        CreateMap<Order, OrderDto>();
        CreateMap<CreateUpdateOrderDto, Order>();

        CreateMap<OrderItem, OrderItemDto>();
        CreateMap<CreateUpdateOrderItemDto, OrderItem>();

        #endregion

    }
}
