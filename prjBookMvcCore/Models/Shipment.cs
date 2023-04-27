using System;
using System.Collections.Generic;

namespace prjBookMvcCore.Models
{
    public partial class Shipment
    {
        public Shipment()
        {
            Orders = new HashSet<Order>();
        }

        public int ShipmentId { get; set; }
        public string ShipmentName { get; set; } = null!;

        public virtual ICollection<Order> Orders { get; set; }
    }
}
