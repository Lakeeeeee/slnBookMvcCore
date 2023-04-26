using System;
using System.Collections.Generic;

namespace prjBookMvcCore.Models
{
    public partial class SubCategory
    {
        public SubCategory()
        {
            CategoryDetails = new HashSet<CategoryDetail>();
        }

        public int SubCategoryId { get; set; }
        public string? SubCategoryName { get; set; }
        public int CategoryId { get; set; }

        public virtual Category Category { get; set; } = null!;
        public virtual ICollection<CategoryDetail> CategoryDetails { get; set; }
    }
}
