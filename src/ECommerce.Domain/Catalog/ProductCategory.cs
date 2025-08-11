using Volo.Abp.Domain.Entities;

namespace ECommerce.Catalog
{
    public class ProductCategory : Entity<int>
    {
        public int ProductId { get; private set; }
        public int CategoryId { get; private set; }

        // Navigation properties
        public Product Product { get; private set; }
        public Category Category { get; private set; }

        private ProductCategory() { }

        public ProductCategory(int id, int productId, int categoryId) : base(id)
        {
            ProductId = productId;
            CategoryId = categoryId;
        }
    }
}