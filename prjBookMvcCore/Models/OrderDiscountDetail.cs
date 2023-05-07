using System;
using System.Collections.Generic;

namespace prjBookMvcCore.Models
{
    public partial class OrderDiscountDetail
    {
        public int OrderDiscountDetailId { get; set; }
        public int OrderDiscountId { get; set; }
        public int MemberId { get; set; }
        public DateTime OrderDiscountStartDate { get; set; }
        public DateTime OrderDiscountEndDate { get; set; }
        public string IsOrderDiscountUse { get; set; } = null!;

        public virtual Member Member { get; set; } = null!;
        public virtual OrderDiscount OrderDiscount { get; set; } = null!;
    }
}
