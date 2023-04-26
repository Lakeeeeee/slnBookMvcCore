using System;
using System.Collections.Generic;

namespace prjBookMvcCore.Models
{
    public partial class PurchaseDetail
    {
        public int PurchaseDetailId { get; set; }
        public int PurchaseId { get; set; }
        public int PublisherId { get; set; }
        public int BookId { get; set; }
        public string Isbn { get; set; } = null!;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        public virtual Book Book { get; set; } = null!;
        public virtual Publisher Publisher { get; set; } = null!;
        public virtual Purchase Purchase { get; set; } = null!;
    }
}
