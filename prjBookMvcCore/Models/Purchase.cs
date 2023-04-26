using System;
using System.Collections.Generic;

namespace prjBookMvcCore.Models
{
    public partial class Purchase
    {
        public Purchase()
        {
            PurchaseDetails = new HashSet<PurchaseDetail>();
        }

        public int PurchaseId { get; set; }
        public DateTime PurchaseDate { get; set; }
        public DateTime ArrivedData { get; set; }

        public virtual ICollection<PurchaseDetail> PurchaseDetails { get; set; }
    }
}
