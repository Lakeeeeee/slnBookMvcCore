using System;
using System.Collections.Generic;

namespace prjBookMvcCore.Models
{
    public partial class Payment
    {
        public Payment()
        {
            Members = new HashSet<Member>();
            Orders = new HashSet<Order>();
        }

        public int PaymentId { get; set; }
        public string PaymentName { get; set; } = null!;

        public virtual ICollection<Member> Members { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
