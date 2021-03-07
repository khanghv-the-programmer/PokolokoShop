using System;
using System.Collections.Generic;

namespace PokolokoShop.Models
{
    public partial class Image
    {
        public Image()
        {
            ProductImage = new HashSet<ProductImage>();
        }

        public int Idimage { get; set; }
        public string ThumnailImg { get; set; }
        public string Name { get; set; }
        public string LargeImage { get; set; }

        public virtual ICollection<ProductImage> ProductImage { get; set; }
    }
}
