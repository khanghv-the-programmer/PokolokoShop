using System;
using System.Collections.Generic;

namespace PokolokoShop.Models
{
    public partial class ProductImage
    {
        public int IdproductImage { get; set; }
        public int? Idimage { get; set; }
        public int? Idproduct { get; set; }

        public virtual Image IdimageNavigation { get; set; }
        public virtual Product IdproductNavigation { get; set; }
    }
}
