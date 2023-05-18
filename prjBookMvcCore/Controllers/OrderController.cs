using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using prjBookMvcCore.Models;
using prjBookMvcCore.ViewModel;

namespace prjBookMvcCore.Controllers
{
    public class OrderController : Controller
    {
        public UserInforService _user { get; set; }
        public OrderController(UserInforService user, BookShopContext db)
        {
            _user = user;
            this.db = db;
        }

        BookShopContext db = new();
        public IActionResult ListCart()
        {
            List<ShoppingcartInformation> cartItems = new List<ShoppingcartInformation>();
            var query = from b in db.Books
                        join ad in db.ActionDetials
                        on b.BookId equals ad.BookId
                        where ad.MemberId == _user.UserId && ad.ActionId == 4
                        orderby ad.ActionToBookId descending
                        select new
                        {
                            書名 = b.BookTitle,
                            書本ID = b.BookId,
                            定價 = b.UnitPrice,
                            出版社 = b.Publisher.PublisherName,
                            折扣名 = b.BookDiscountDetails.Select(x => x.BookDiscount.BookDiscountName).FirstOrDefault(),
                            折扣 = b.BookDiscountDetails.Select(x => x.BookDiscount.BookDiscountAmount).FirstOrDefault(),
                            庫存 = b.UnitInStock,
                            ActionID = ad.ActionId,
                        };
            foreach (var item in query)
            {
                Book b = new Book()
                {
                    BookId = item.書本ID,
                    BookTitle = item.書名,
                    UnitInStock = item.庫存,
                    UnitPrice = item.定價,
                };
                ActionDetial ad = new ActionDetial { ActionId = item.ActionID };
                Publisher p = new Publisher { PublisherName = item.出版社 };
                BookDiscount bd = new BookDiscount { BookDiscountAmount = item.折扣, BookDiscountName = item.折扣名 };
                CInformation tmp = new CInformation()
                {
                    book = b,
                    bookDiscount = bd,
                    publisher = p,
                };
                ShoppingcartInformation shoppingcartInformation = new ShoppingcartInformation()
                {
                    CInformation = tmp,
                    ActionDetial = ad,
                };
                cartItems.Add(shoppingcartInformation);
            }
            return View(cartItems);
        }
        [Authorize]
        public IActionResult ShoppingCart(int memberID)
        {
            List<ShoppingcartInformation> cartItems = new List<ShoppingcartInformation>();
            var query = from b in db.Books
                        join ad in db.ActionDetials
                        on b.BookId equals ad.BookId
                        where ad.MemberId == _user.UserId
                        orderby ad.ActionToBookId descending
                        select new
                        {
                            書名 = b.BookTitle,
                            書本ID = b.BookId,
                            定價 = b.UnitPrice,
                            出版社 = b.Publisher.PublisherName,
                            折扣名 = b.BookDiscountDetails.Select(x => x.BookDiscount.BookDiscountName).FirstOrDefault(),
                            折扣 = b.BookDiscountDetails.Select(x => x.BookDiscount.BookDiscountAmount).FirstOrDefault(),
                            庫存 = b.UnitInStock,
                            ActionID = ad.ActionId,
                        };
            foreach (var item in query)
            {
                Book b = new Book()
                {
                    BookId = item.書本ID,
                    BookTitle = item.書名,
                    UnitInStock = item.庫存,
                    UnitPrice = item.定價,
                };
                ActionDetial ad = new ActionDetial { ActionId = item.ActionID };
                Publisher p = new Publisher { PublisherName = item.出版社};
                BookDiscount bd = new BookDiscount { BookDiscountAmount = item.折扣, BookDiscountName = item.折扣名 };
                CInformation tmp = new CInformation(){
                    book = b,
                    bookDiscount = bd,
                    publisher = p,
                };
                ShoppingcartInformation shoppingcartInformation = new ShoppingcartInformation()
                {
                    CInformation = tmp,
                    ActionDetial = ad,
                };
                cartItems.Add(shoppingcartInformation);
            }
            return View(cartItems);
        }

        public IActionResult checkOutInfo()
        {
            Member member = new Member();
            var query = from m in db.Members
                        join discount in db.OrderDiscountDetails
                        on m.MemberId equals discount.MemberId
                        select new
                        {
                            回饋金 = m.Points,
                            會員等級 = m.Level.LevelName,
                            等級優惠 = m.Level.LevelDiscount,
                            酷碰劵 = discount.OrderDiscount.OrderDiscountDescprtion,
                            酷碰券內容 = discount.OrderDiscount.OrderDiscountAmount
                        };
            foreach (var item in query)
            {
                Member m = new Member()
                {
                     Points = item.回饋金,
                };
                MemberLevel ml = new MemberLevel()
                {
                    LevelDiscount = item.等級優惠,
                    LevelName = item.會員等級
                };
                OrderDiscount od = new OrderDiscount()
                {
                    OrderDiscountDescprtion = item.酷碰劵,
                    OrderDiscountAmount = item.酷碰券內容,
                };
                checkoutInformation ci = new checkoutInformation()
                {
                    member = m,
                    memberlevel = ml,
                    orderdiscount = od,
                };
                return View(ci);
            }
            return View();
        }

        public IActionResult checkOutConfirm()
        {
            return View();
        }
        [HttpPost]
        public IActionResult checkOutConfirm(CheckOutConfirmViewModel model)
        {
            return View(model);
        }
        public IActionResult checkOutFinal()
        {
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }

        
}

    }

