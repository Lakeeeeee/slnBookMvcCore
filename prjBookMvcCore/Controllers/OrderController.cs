using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using prjBookMvcCore.Models;
using prjBookMvcCore.ViewModel;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace prjBookMvcCore.Controllers
{
    public class OrderController : Controller
    {
        public UserInforService _user { get; set; }
        BookShopContext _db = new BookShopContext();

        public OrderController(UserInforService user, BookShopContext db)
        {
            _user = user;
            _db = db;
        }

        //public IActionResult ListCart() //購物車暫存
        //{
        //    List<ShoppingcartInformation> cartItems = new List<ShoppingcartInformation>();
        //    var query = from b in _db.Books
        //                join ad in _db.ActionDetials
        //                on b.BookId equals ad.BookId
        //                where ad.MemberId == _user.UserId && ad.ActionId == 4
        //                orderby ad.ActionToBookId descending
        //                select new
        //                {
        //                    書名 = b.BookTitle,
        //                    書本ID = b.BookId,
        //                    定價 = b.UnitPrice,
        //                    出版社 = b.Publisher.PublisherName,
        //                    折扣名 = b.BookDiscountDetails.Select(x => x.BookDiscount.BookDiscountName).FirstOrDefault(),
        //                    折扣 = b.BookDiscountDetails.Select(x => x.BookDiscount.BookDiscountAmount).FirstOrDefault(),
        //                    庫存 = b.UnitInStock,
        //                    ActionID = ad.ActionId,
        //                };
        //    foreach (var item in query)
        //    {
        //        Book b = new Book()
        //        {
        //            BookId = item.書本ID,
        //            BookTitle = item.書名,
        //            UnitInStock = item.庫存,
        //            UnitPrice = item.定價,
        //        };
        //        ActionDetial ad = new ActionDetial { ActionId = item.ActionID };
        //        Publisher p = new Publisher { PublisherName = item.出版社 };
        //        BookDiscount bd = new BookDiscount { BookDiscountAmount = item.折扣, BookDiscountName = item.折扣名 };
        //        CInformation tmp = new CInformation()
        //        {
        //            book = b,
        //            bookDiscount = bd,
        //            publisher = p,
        //        };
        //        ShoppingcartInformation shoppingcartInformation = new ShoppingcartInformation()
        //        {
        //            CInformation = tmp,
        //            ActionDetial = ad,
        //        };
        //        cartItems.Add(shoppingcartInformation);
        //    }
        //    return View(cartItems);
        //}
        [Authorize]
        public IActionResult ShoppingCart()
        {
            List<ShoppingcartInformation> cartItems = new List<ShoppingcartInformation>();

            IEnumerable<ActionDetial> q = _db.ActionDetials.Where(x => x.MemberId == _user.UserId && x.ActionId == 7);

            foreach (var item in q)
            {
                ShoppingcartInformation shoppingcartInformation = new ShoppingcartInformation()
                {
                    ActionDetial = item,
                    Quantity = 1
                };
                cartItems.Add(shoppingcartInformation);
            }
            return View(cartItems);

        }

        public IActionResult itemDelete(int id)
        {
            bool isSuccesse = false;
            var tool = _db.ActionDetials.Find(id);
            if (tool != null)
            {
                _db.ActionDetials.Remove(tool);
                _db.SaveChanges();
                isSuccesse = true;
            };
            return View(isSuccesse.ToString());
        }

        [Authorize]
        public IActionResult searchDiscount(int total)
        {
            List<DiscountType> discountTypes = new List<DiscountType>();
            Member x = _db.Members.Where(x=>x.MemberId==_user.UserId).FirstOrDefault();
            if (total > 1000)
            {

                DiscountType memberDiscountType = _db.DiscountTypes.Where(x=>x.DiscountTypeId == 1).FirstOrDefault();                //回傳會員優惠
                discountTypes.Add(memberDiscountType);
            };

            var q = _db.OrderDiscountDetails.Where(x=>x.MemberId==_user.UserId && x.IsOrderDiscountUse=="N").ToList();

            if (q.Count() > 0)
            {

                DiscountType couponDiscountType = _db.DiscountTypes.Where(x => x.DiscountTypeId == 2).FirstOrDefault(); 
                discountTypes.Add(couponDiscountType);
            };

            return Json(discountTypes);
        }




        public IActionResult checkOutConfirm()
        {
            return View();
        }
        [HttpPost]
        public IActionResult checkOutConfirm(OrderViewModel model)
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

