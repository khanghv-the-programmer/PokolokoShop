using System;
using System.Collections.Generic;

namespace PokolokoShop.Models
{
    public partial class Product
    {
        public Product()
        {
            ProductImage = new HashSet<ProductImage>();
        }

        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int QuantityInStock { get; set; }
        public string Material { get; set; }
        public string Description { get; set; }
        public int BrandId { get; set; }
        public int SubCategoryId { get; set; }
        public string Status { get; set; }
        public DateTime DateModified { get; set; }

        public virtual Brand Brand { get; set; }
        public virtual SubCategory SubCategory { get; set; }
        public virtual ICollection<ProductImage> ProductImage { get; set; }
    }
}
