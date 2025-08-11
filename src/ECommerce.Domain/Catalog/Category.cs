using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace ECommerce.Catalog
{
    public class Category : FullAuditedAggregateRoot<int>
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public int? ParentCategoryId { get; private set; }
        public string PictureId { get; private set; }
        public int DisplayOrder { get; private set; }
        public bool Published { get; private set; }
        public DateTime CreatedOnUtc { get; private set; }
        public DateTime? UpdatedOnUtc { get; private set; }

        public ICollection<ProductCategory> ProductCategories { get; private set; }

        private Category()
        {
            ProductCategories = new List<ProductCategory>();
        }

        public Category(
            int id,
            string name,
            string description,
            int parentCategoryId,
            string pictureId,
            int displayOrder,
            bool published)
            : base(id)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Description = description;
            ParentCategoryId = parentCategoryId;
            PictureId = pictureId;
            DisplayOrder = displayOrder;
            Published = published;
            CreatedOnUtc = DateTime.UtcNow;
            ProductCategories = new List<ProductCategory>();
        }

        public void UpdateDetails(string name, string description, int displayOrder, bool published)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be empty", nameof(name));

            Name = name;
            Description = description;
            DisplayOrder = displayOrder;
            Published = published;
            UpdatedOnUtc = DateTime.UtcNow;
        }

    }
}
