using System;
using System.Collections.Generic;

namespace prjBookMvcCore.Models
{
    public partial class BookDiscountDetail
    {
        public int BookDiscountDetailId { get; set; }
        public int BookDiscountId { get; set; }
        public int BookId { get; set; }
        public DateTime BookDiscountStartDate { get; set; }
        public DateTime BookDiscountEndDate { get; set; }
    }
}
