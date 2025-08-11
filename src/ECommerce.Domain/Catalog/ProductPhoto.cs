using System;
using Volo.Abp.Domain.Entities;

namespace ECommerce.Catalog
{
    public class ProductPhoto:Entity<int>
    {
        public int ProductId { get; private set; }
        public string PictureUrl { get; private set; }
        public int DisplayOrder { get; private set; }

        // Navigation property
        public Product Product { get; private set; }

        private ProductPhoto() { }

        public ProductPhoto(int id, int productId, string pictureUrl, int displayOrder)
            : base(id)
        {
            ProductId = productId;
            PictureUrl = pictureUrl ?? throw new ArgumentNullException(nameof(pictureUrl));
            DisplayOrder = displayOrder;
        }

        public void Update(string pictureUrl, int displayOrder)
        {
            PictureUrl = pictureUrl ?? throw new ArgumentNullException(nameof(pictureUrl));
            DisplayOrder = displayOrder;
        }
    }
}