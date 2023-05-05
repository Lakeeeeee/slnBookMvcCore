﻿using System;
using System.Collections.Generic;

namespace prjBookMvcCore.Models
{
    public partial class Order
    {
        public int OrderId { get; set; }
        public int MemberId { get; set; }
        public DateTime OrderDate { get; set; }
        public int ShipmentId { get; set; }
        public int PaymentId { get; set; }
        public int PayStatusId { get; set; }
        public int ShippingStatusId { get; set; }
        public int? DiscountId { get; set; }
        public string ShipAddr { get; set; } = null!;
        public string? CouponId { get; set; }
        public decimal? DiscountAmount { get; set; }
        public decimal? TotalPay { get; set; }

        public virtual Discount? Discount { get; set; }
        public virtual Member Member { get; set; } = null!;
        public virtual PayStatus PayStatus { get; set; } = null!;
        public virtual Payment Payment { get; set; } = null!;
        public virtual Shipment Shipment { get; set; } = null!;
        public virtual ShippingStatus ShippingStatus { get; set; } = null!;
    }
}
