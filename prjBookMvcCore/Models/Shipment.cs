using System;
using System.Collections.Generic;

namespace prjBookMvcCore.Models
{
    public partial class Shipment
    {
        public int ShipmentId { get; set; }
        public string ShipmentName { get; set; } = null!;
    }
}
