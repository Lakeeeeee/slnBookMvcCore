using System;
using System.Collections.Generic;

namespace prjBookMvcCore.Models
{
    public partial class CategoryDetail
    {
        public int CategoryDetailId { get; set; }
        public int BookId { get; set; }
        public int? SubCategoryId { get; set; }

        public virtual Book Book { get; set; } = null!;
        public virtual SubCategory? SubCategory { get; set; }
    }
}
