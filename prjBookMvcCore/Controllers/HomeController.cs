using Microsoft.AspNetCore.Mvc;
using prjBookMvcCore.Models;
using System.Diagnostics;

namespace prjBookMvcCore.Controllers
{
    public class HomeController : Controller
    {
        BookShopContext db = new();

        public IActionResult Home()
        {
            CForHomePage cForHomePage = new CForHomePage();
            cForHomePage.recommendInformation = getRecommendBooks();
            return View(cForHomePage);
        }

        public IActionResult _ProductPartialView()
        {
            CForHomePage cForHomePage = new CForHomePage();
            cForHomePage.recommendInformation = getRecommendBooks();
            return View(cForHomePage);
        }

        public IActionResult commentList()
        {
            return View();
        }

        public IActionResult QA()
        {
            return View();
        }


        #region(系統預設的東西, 先留著, 之後不需要再刪掉)
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        #endregion

        public IActionResult Menu()
        {
            return View();
        }

        public List<RecommendInformation> getRecommendBooks()
        {
            using (var db = new BookShopContext())
            {
                var recommendBooks = from b in db.Books
                                     join sd in db.CategoryDetails
                                     on b.BookId equals sd.BookId
                                     join sc in db.SubCategories
                                     on sd.SubCategoryId equals sc.SubCategoryId
                                    
                                     orderby b.PublicationDate descending
                                    
                                     select new
                                     {
                                         書本ID = b.BookId,
                                         書名 = b.BookTitle,
                                         定價 = b.UnitPrice,
                                         路徑 = b.CoverPath,
                                         折扣 = b.BookDiscountDetails.Select(x => x.BookDiscount.BookDiscountAmount).FirstOrDefault(),
                                         截止日 = b.BookDiscountDetails.Select(x => x.BookDiscountEndDate).FirstOrDefault(),
                                         出版日期 = b.PublicationDate,
                                     };
                List<RecommendInformation> ris = new List<RecommendInformation>();
                int count = 0;
                foreach (var recommendBook in recommendBooks)
                {
                    count++;
                    if(count == 10)
                    {
                        break;
                    }
                    Book b = new Book { BookTitle = recommendBook.書名, BookId = recommendBook.書本ID, UnitPrice = recommendBook.定價, CoverPath = recommendBook.路徑, PublicationDate = recommendBook.出版日期 };
                    BookDiscount bd = new BookDiscount { BookDiscountAmount = recommendBook.折扣 };
                    BookDiscountDetail bdd = new BookDiscountDetail { BookDiscountEndDate = recommendBook.截止日 };
                    RecommendInformation tmp = new RecommendInformation()
                    {
                        book = b,
                        bookDiscount = bd,
                        bookDiscountDetail = bdd
                    };
                    ris.Add(tmp);
                }
                return ris;
            }
        }
    }
}