using System;
using System.Collections.Generic;

namespace prjBookMvcCore.Models
{
    public partial class PayStatus
    {
        public PayStatus()
        {
            Orders = new HashSet<Order>();
        }

        public int PayStatusId { get; set; }
        public string PayStatusName { get; set; } = null!;

        public virtual ICollection<Order> Orders { get; set; }
    }
}
