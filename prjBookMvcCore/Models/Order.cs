using System;
using System.Collections.Generic;
using System.Net;


namespace prjBookMvcCore.Models
{
    public partial class Order
    {

		private readonly BookShopContext db = new BookShopContext();
		public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
		}
  	
		public Book Book { get { return db.Books.Find(Book); } }
		public int OrderId { get; set; }
        public int MemberId { get; set; }
        public DateTime OrderDate { get; set; }
        public int ShipmentId { get; set; }
        public int PaymentId { get; set; }
        public int PayStatusId { get; set; }
        public int ShippingStatusId { get; set; }
        public int? OrderDiscountId { get; set; }
        public decimal? PointAmount { get; set; }
        public decimal? TotalPay { get; set; }
        public string? ReciverName { get; set; }
        public string? ReciverPhone { get; set; }
        public string ShipAddr { get; set; } = null!;
        public decimal? FinalPay { get; set; }

        public virtual Member Member { get; set; } = null!;
        public virtual OrderDiscount? OrderDiscount { get; set; }
        public virtual PayStatus PayStatus { get; set; } = null!;
        public virtual Payment Payment { get; set; } = null!;
        public virtual Shipment Shipment { get; set; } = null!;
        public virtual ShippingStatus ShippingStatus { get; set; } = null!;
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
