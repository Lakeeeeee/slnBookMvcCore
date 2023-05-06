using System;
using System.Collections.Generic;

namespace prjBookMvcCore.Models
{
    public partial class Category
    {
        public Category()
        {
            SubCategories = new HashSet<SubCategory>();
        }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;

        public virtual ICollection<SubCategory> SubCategories { get; set; }
    }
}
