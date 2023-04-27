using System;
using System.Collections.Generic;

namespace prjBookMvcCore.Models
{
    public partial class Discount
    {
        public Discount()
        {
            DiscountDetails = new HashSet<DiscountDetail>();
            Orders = new HashSet<Order>();
        }

        public int DiscountId { get; set; }
        public string? DiscountName { get; set; }
        public string? DiscounDescription { get; set; }
        public decimal? DiscountAmount { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        /// <summary>
        /// 1
        /// </summary>
        public int IsActive { get; set; }

        public virtual ICollection<DiscountDetail> DiscountDetails { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
