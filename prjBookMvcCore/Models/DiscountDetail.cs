using System;
using System.Collections.Generic;

namespace prjBookMvcCore.Models
{
    public partial class DiscountDetail
    {
        public int DiscountDetailId { get; set; }
        public int DiscountId { get; set; }
        public int BookId { get; set; }
        public int CategoryId { get; set; }

        public virtual Book Book { get; set; } = null!;
        public virtual Category Category { get; set; } = null!;
        public virtual Discount Discount { get; set; } = null!;
    }
}
