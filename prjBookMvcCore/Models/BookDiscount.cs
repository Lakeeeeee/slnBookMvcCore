using System;
using System.Collections.Generic;

namespace prjBookMvcCore.Models
{
    public partial class BookDiscount
    {
        public BookDiscount()
        {
            BookDiscountDetails = new HashSet<BookDiscountDetail>();
        }

        public int BookDiscountId { get; set; }
        public string BookDiscountName { get; set; } = null!;
        public decimal BookDiscountAmount { get; set; }

        public virtual ICollection<BookDiscountDetail> BookDiscountDetails { get; set; }
    }
}
