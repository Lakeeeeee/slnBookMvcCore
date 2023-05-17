using System;
using System.Collections.Generic;

namespace prjBookMvcCore.Models
{
    public partial class Discount
    {
        public int DiscountId { get; set; }
        public int? DiscountTypeId { get; set; }
        public string? DiscountName { get; set; }
        public string? DiscounDescription { get; set; }
        public decimal? DiscountAmount { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        /// <summary>
        /// 1
        /// </summary>
        public int IsActive { get; set; }
    }
}
