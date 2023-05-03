using System;
using System.Collections.Generic;

namespace prjBookMvcCore.Models
{
    public partial class ShippingStatus
    {
        public ShippingStatus()
        {
            Orders = new HashSet<Order>();
        }

        public int ShippingStatusId { get; set; }
        public string? ShippingStatusName { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
