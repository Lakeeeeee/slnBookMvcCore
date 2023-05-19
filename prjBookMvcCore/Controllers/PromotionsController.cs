using GoogleReCaptcha.V3.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prjBookMvcCore.Models;
using System.Diagnostics.Metrics;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace prjBookMvcCore.Controllers
{
    public class PromotionsController : Controller
    {
        BookShopContext db = new();
        public UserInforService _userInforService { get; set; }
        public PromotionsController(UserInforService userInforService)
        {
            _userInforService = userInforService;
        }
        public IActionResult Promotions註冊會員禮() { return View(); }

        public IActionResult Promotions會員() { return View(); }

        public IActionResult Promotions促銷活動() { return View(); }

        public IActionResult Promotions促銷(int id, int page = 1)
        {
            if (id != 0)
            {
                int discountId = id;
                ViewBag.DiscountId = discountId;
                int itemsPerPage = 28;//每頁只顯示28個                   
                var bookDiscountDetail = db.BookDiscountDetails.Where(d => d.BookDiscountId == discountId & d.BookDiscountStartDate < DateTime.Now & d.BookDiscountEndDate > DateTime.Now).Select(d => new { d.BookDiscount.BookDiscountName, d.BookDiscount.BookDiscountAmount, d.Book.BookTitle, d.Book.UnitPrice, d.Book.CoverPath, d.Book.BookId, d.BookDiscountEndDate });

                //頁面顯示控制
                int totalItems = bookDiscountDetail.Count();
                int totalPages = (int)Math.Ceiling((double)totalItems / itemsPerPage);
                int offset = (page - 1) * itemsPerPage;
                var books = bookDiscountDetail.Skip(offset).Take(itemsPerPage);

                List<MenuItem> menuItems = new List<MenuItem>();
                foreach (var item in books)
                {
                    Book b = new Book
                    {
                        BookTitle = item.BookTitle,
                        BookId = item.BookId,
                        UnitPrice = item.UnitPrice,
                        CoverPath = item.CoverPath,
                    };
                    BookDiscount bd = new BookDiscount { BookDiscountAmount = item.BookDiscountAmount, BookDiscountName=item.BookDiscountName,};
                    BookDiscountDetail bdd = new BookDiscountDetail{ BookDiscountEndDate = item.BookDiscountEndDate,};
                    MenuItem tmp = new MenuItem();
                    tmp.book = b;
                    tmp.bookDiscount = bd;
                    tmp.bookDiscountDetail = bdd;
                    menuItems.Add(tmp);
                }

                MenuInformation menuInformation = new MenuInformation
                {
                    menuItems = menuItems,
                    CurrentPage = page,
                    TotalPages = totalPages,
                    bookDiscountId= discountId
                };
                return View(menuInformation);
            }
            else { return RedirectToAction("Promotions促銷活動"); }
        }

        public IActionResult Promotions活動總覽圖()
        {
            var datas = db.Articals.OrderByDescending(a => a.ArticalId).Select(a => a);
            return View(datas);
        }

        public IActionResult Promotions活動總覽表()
        {
            var datas = db.Articals.OrderByDescending(a => a.ArticalId).Select(a => a);
            return View(datas);
        }

        public IActionResult Promotions活動文章Detail(int? id)
        {
            if (id == null) { return RedirectToAction("Promotions活動已結束"); }
            ViewBag.ArticalId = id;
            return View();
        }

        public IActionResult Promotions活動已結束() { return View(); }

        public string Promotions限時登入領取優惠()
        {
            string isSuccess;
            var q = db.OrderDiscountDetails.Where(d => d.MemberId == _userInforService.UserId & d.OrderDiscountId == 4).Select(d => d);
            if (q.Count() != 0)
            {
                isSuccess = "false";
                return isSuccess;
            }
            else
            {
                OrderDiscountDetail newmemberdiscount = new OrderDiscountDetail()
                {
                    OrderDiscountId = 4,
                    MemberId = _userInforService.UserId,
                    OrderDiscountStartDate = DateTime.Now,
                    OrderDiscountEndDate = DateTime.Now.AddDays(3),
                };
                db.OrderDiscountDetails.Add(newmemberdiscount);
                db.SaveChanges();
                isSuccess = "true";
                return isSuccess;
            }
        }

        public IActionResult Index() { return View(); } //建置中
    }
}
