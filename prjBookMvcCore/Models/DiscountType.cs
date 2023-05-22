using System;
using System.Collections.Generic;

namespace prjBookMvcCore.Models
{
    public partial class DiscountType
    {
        public DiscountType()
        {
            OrderDiscounts = new HashSet<OrderDiscount>();
        }

        public int DiscountTypeId { get; set; }
        public string? DiscountTypeName { get; set; }

        public virtual ICollection<OrderDiscount> OrderDiscounts { get; set; }
    }
}
