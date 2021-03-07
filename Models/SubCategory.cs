using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace PokolokoShop.Models
{
    public partial class SubCategory
    {
        public SubCategory()
        {
            Product = new HashSet<Product>();
        }

        public int SubCategoryId { get; set; }
        public string SubCateName { get; set; }
        public int? CategoryId { get; set; }
        [JsonIgnore]
        public virtual Category Category { get; set; }
        [JsonIgnore]
        public virtual ICollection<Product> Product { get; set; }
    }
}
