namespace prjBookMvcCore.Models
{
    public class ShoppingcartInformation
    {
        private readonly BookShopContext db = new BookShopContext();

        public ActionDetial ActionDetial { get; set; }

        public int BookId { get { return ActionDetial?.BookId ?? 0; } }

        public Book Book { get { return db.Books.Find(BookId); } }

        public int Quantity { get; set; }
    }
}
