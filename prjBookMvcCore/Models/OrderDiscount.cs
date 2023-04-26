using System;
using System.Collections.Generic;

namespace prjBookMvcCore.Models
{
    public partial class OrderDiscount
    {
        public int OrderDiscountId { get; set; }
        public string OrderDiscountDescprtion { get; set; } = null!;
        public int OrderDiscountCondition { get; set; }
        public decimal OrderDiscountAmount { get; set; }
        public string OrderDiscountCode { get; set; } = null!;
    }
}
