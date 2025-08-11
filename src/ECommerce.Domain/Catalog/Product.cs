using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace ECommerce.Catalog
{
    public class Product : FullAuditedAggregateRoot<int>
    {
        public string Name { get; private set; }
        public string ShortDescription { get; private set; }
        public string FullDescription { get; private set; }
        public decimal Price { get; private set; }
        public int StockQuantity { get; private set; }
        public bool Published { get; private set; }
        public string Sku { get; private set; }

        public ICollection<ProductPhoto> ProductPhotos { get; private set; }
        public ICollection<ProductCategory> ProductCategories { get; private set; }

        private Product()
        {
            ProductPhotos = new List<ProductPhoto>();
            ProductCategories = new List<ProductCategory>();
        }

        public Product(
            int id,
            string name,
            string shortDescription,
            string fullDescription,
            decimal price,
            int stockQuantity,
            bool published,
            string sku) : base(id)
        {
            SetDetails(name, shortDescription, fullDescription, price, stockQuantity, published, sku);
            ProductPhotos = new List<ProductPhoto>();
            ProductCategories = new List<ProductCategory>();
        }

        public void SetDetails(
            string name,
            string shortDescription,
            string fullDescription,
            decimal price,
            int stockQuantity,
            bool published,
            string sku)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Product name cannot be empty.", nameof(name));
            if (price < 0)
                throw new ArgumentException("Price cannot be negative.", nameof(price));
            if (stockQuantity < 0)
                throw new ArgumentException("Stock quantity cannot be negative.", nameof(stockQuantity));

            Name = name;
            ShortDescription = shortDescription;
            FullDescription = fullDescription;
            Price = price;
            StockQuantity = stockQuantity;
            Published = published;
            Sku = sku;
        }

    }
}
