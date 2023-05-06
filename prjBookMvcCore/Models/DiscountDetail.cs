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
    }
}
