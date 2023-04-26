using System;
using System.Collections.Generic;

namespace prjBookMvcCore.Models
{
    public partial class BookDiscount
    {
        public int BookDiscountId { get; set; }
        public string BookDiscountName { get; set; } = null!;
        public decimal BookDiscountAmount { get; set; }
    }
}
