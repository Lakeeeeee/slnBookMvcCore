using System;
using System.Collections.Generic;

namespace prjBookMvcCore.Models
{
    public partial class OrderDiscount
    {
        public OrderDiscount()
        {
            OrderDiscountDetails = new HashSet<OrderDiscountDetail>();
        }

        public int OrderDiscountId { get; set; }
        public int DiscountTypeId { get; set; }
        public string OrderDiscountnName { get; set; } = null!;
        public string OrderDiscountDescprtion { get; set; } = null!;
        public int OrderDiscountCondition { get; set; }
        public decimal OrderDiscountAmount { get; set; }
        public string? OrderDiscountCode { get; set; }

        public virtual DiscountType DiscountType { get; set; } = null!;
        public virtual ICollection<OrderDiscountDetail> OrderDiscountDetails { get; set; }
    }
}
