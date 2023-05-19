using System;
using System.Collections.Generic;

namespace prjBookMvcCore.Models
{
    public partial class DiscountType
    {
        public DiscountType()
        {
            MemberLevels = new HashSet<MemberLevel>();
            OrderDiscounts = new HashSet<OrderDiscount>();
            Orders = new HashSet<Order>();
        }

        public int DiscountTypeId { get; set; }
        public string? DiscountTypeName { get; set; }

        public virtual ICollection<MemberLevel> MemberLevels { get; set; }
        public virtual ICollection<OrderDiscount> OrderDiscounts { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
