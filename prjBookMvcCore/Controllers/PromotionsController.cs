using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;
using prjBookMvcCore.Models;
using System.Net;

namespace prjBookMvcCore.Controllers
{
    public class PromotionsController : Controller
    {
        BookShopContext db = new();
        public UserInforService _userInforService { get; set; }
        public PromotionsController(UserInforService userInforService) { _userInforService = userInforService; }

        public IActionResult Promotions註冊會員禮() { return View(); }
        public IActionResult Promotions會員() { return View(); }
        public IActionResult Promotions促銷活動() { return View(); }
        public IActionResult Promotions促銷(int id, int page = 1)
        {
            if (id != 0)
            {
                ViewBag.Discount = db.BookDiscounts.Where(d => d.BookDiscountId == id).Select(d=>d.BookDiscountName).FirstOrDefault();
                int itemsPerPage = 28;//每頁只顯示28個                   
                var bookDiscountDetail = db.BookDiscountDetails.Where(d => d.BookDiscountId == id & d.BookDiscountStartDate < DateTime.Now & d.BookDiscountEndDate > DateTime.Now).Select(d => new { d.BookDiscount.BookDiscountName, d.BookDiscount.BookDiscountAmount, d.Book.BookTitle, d.Book.UnitPrice, d.Book.CoverPath, d.Book.BookId, d.BookDiscountEndDate });

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
                    BookDiscount bd = new BookDiscount { BookDiscountAmount = item.BookDiscountAmount, BookDiscountName = item.BookDiscountName, };
                    BookDiscountDetail bdd = new BookDiscountDetail { BookDiscountEndDate = item.BookDiscountEndDate, };
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
                    bookDiscountId = id,
                };
                return View(menuInformation);
            }
            else { return RedirectToAction("Promotions促銷活動"); }
        }
        public IActionResult Promotions活動總覽圖()
        {
            var datas = db.Articals.OrderByDescending(a => a.ArticalId).Where(a => a.ArticalId != 8 & a.ArticalId != 10 & a.ArticalId != 17).Select(a => a);
            return View(datas);
        }
        public IActionResult Promotions活動總覽表()
        {
            var datas = db.Articals.OrderByDescending(a => a.ArticalId).Where(a => a.ArticalId != 8 & a.ArticalId != 10 & a.ArticalId != 17).Select(a => a);
            return View(datas);
        }
        public IActionResult Promotions活動圖()
        {
            var datas = db.Articals.OrderByDescending(a => a.ArticalId).Where(a => a.ArticalId == 8 || a.ArticalId == 10 || a.ArticalId == 17).Select(a => a);
            return View(datas);
        }
        public IActionResult Promotions活動表()
        {
            var datas = db.Articals.OrderByDescending(a => a.ArticalId).Where(a => a.ArticalId == 8 || a.ArticalId == 10 || a.ArticalId == 17).Select(a => a);
            return View(datas);
        }
        public IActionResult Promotions活動文章Detail(int? id)
        {
            if (id == null) { return RedirectToAction("Promotions活動已結束"); }
            ViewBag.ArticalId = id;
            return View();
        }
        public IActionResult Promotions活動已結束() { return View(); }
        public IActionResult Index() { return View(); } //預設為 建置中
        public IActionResult Promotions領取當周優惠() { return View(); }
        public string Promotions領取特定日優惠(int discountID, DateTime date, int time)
        {
            string isSuccess;
            var q = db.OrderDiscountDetails.Where(d => d.MemberId == _userInforService.UserId & d.OrderDiscountId == discountID & d.OrderDiscountStartDate == date).Select(d => d);
            if (q.Count() != 0)
            {
                isSuccess = "false";
                return isSuccess;
            }
            else
            {
                OrderDiscountDetail newmemberdiscount = new OrderDiscountDetail()
                {
                    OrderDiscountId = discountID,
                    MemberId = _userInforService.UserId,
                    OrderDiscountStartDate = date,
                    OrderDiscountEndDate = date.AddDays(time),
                };
                db.OrderDiscountDetails.Add(newmemberdiscount);
                db.SaveChanges();
                isSuccess = "true";
                return isSuccess;
            }
        }

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
        //TODO：會員等級
        public IActionResult Promotions領取月優惠(int? month)
        {
            if (month == null || month < DateTime.Now.Month) { return RedirectToAction("Promotions活動已結束"); }
            else if (month > DateTime.Now.AddMonths(2).Month) { return RedirectToAction("Index"); }
            else { ViewBag.month = month; return View(); }
        }

        public IActionResult SearchTest(string? txtKeyword)
        {
            ViewBag.KeyWord = txtKeyword;
            BookShopContext db = new BookShopContext();
            if (string.IsNullOrEmpty(txtKeyword))
            {
                var s書 = db.Books.Select(b => b).ToList();
                return View(s書);
            }
            else
            {
                var s書 = db.Books.Where(p => p.BookTitle.Contains(txtKeyword) || p.Isbn.Contains(txtKeyword)).ToList();
                var s作者 = db.Authors.Where(a => a.AuthorName.Contains(txtKeyword)).ToList();
                var s譯者 = db.Translators.Where(t => t.TranslatorName.Contains(txtKeyword)).ToList();
                var s繪者 = db.Painters.Where(p => p.PainterName.Contains(txtKeyword)).ToList();
                var s出版社 = db.Publishers.Where(p => p.PublisherName.Contains(txtKeyword)).ToList();
                var s分類 = db.Categories.Where(c => c.CategoryName.Contains(txtKeyword)).ToList();
                CSearch sTest = new CSearch
                {
                    c書 = s書,
                    c作者 = s作者,
                    c譯者 = s譯者,
                    c繪者 = s繪者,
                    c出版社 = s出版社,
                    c分類 = s分類
                };
                return View(sTest);
            }
        }
    }
}
namespace prjBookMvcCore.Models
{
    public class CSearch
    {
        public List<Book>? c書 { get; set; }
        public List<Author>? c作者 { get; set; }
        public List<Translator>? c譯者 { get; set; }
        public List<Painter>? c繪者 { get; set; }
        public List<Publisher>? c出版社 { get; set; }
        public List<Category>? c分類 { get; set; }
    }
}