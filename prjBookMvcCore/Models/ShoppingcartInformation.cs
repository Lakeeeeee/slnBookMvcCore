namespace prjBookMvcCore.Models
{
    public class ShoppingcartInformation
    {
        private readonly BookShopContext db = new BookShopContext();

        public ActionDetial ActionDetial { get; set; }

        public int bookId { get { return ActionDetial?.BookId ?? 0; } }

        public Book Book { get { return db.Books.Find(bookId); } }

        public string discountName
        {
            get
            {
                return db.BookDiscountDetails.Where(x => x.BookId == bookId & x.BookDiscountStartDate < DateTime.Now & x.BookDiscountEndDate > DateTime.Now).Select(x => x.BookDiscount).Select(x=>x.BookDiscountName).FirstOrDefault();
            }
        }

        public decimal discountAmount { get { return db.BookDiscountDetails.Where(x => x.BookId == bookId & x.BookDiscountStartDate < DateTime.Now & x.BookDiscountEndDate > DateTime.Now).Select(x => x.BookDiscount).Select(x => x.BookDiscountAmount).FirstOrDefault(); } }

        public int Quantity { get; set; }
    }
}
