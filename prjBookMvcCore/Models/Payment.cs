using System;
using System.Collections.Generic;

namespace prjBookMvcCore.Models
{
    public partial class Payment
    {
        public Payment()
        {
            Members = new HashSet<Member>();
        }

        public int PaymentId { get; set; }
        public string PaymentName { get; set; } = null!;

        public virtual ICollection<Member> Members { get; set; }
    }
}
