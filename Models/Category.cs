using System;
using System.Collections.Generic;

namespace PokolokoShop.Models
{
    public partial class Category
    {
        public Category()
        {
            SubCategory = new HashSet<SubCategory>();
        }

        public int CategoryId { get; set; }
        public string CateName { get; set; }

        public virtual ICollection<SubCategory> SubCategory { get; set; }
    }
}
