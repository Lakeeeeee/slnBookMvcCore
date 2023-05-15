using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prjBookMvcCore.Models;
using System.Diagnostics;
using System.Net;

namespace prjBookMvcCore.Controllers
{
    public class HomeController : Controller
    {
        BookShopContext db = new();
        public IActionResult Home()
        {
            return View();
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
            var books = from b in db.Books
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
                            作者 = b.AuthorDetails.Select(x => x.Author.AuthorName).FirstOrDefault(),
                            分類 = b.CategoryDetails.Select(x => x.SubCategory.Category.CategoryName).FirstOrDefault(),
                            分類ID = b.CategoryDetails.Select(x => x.SubCategory.CategoryId).FirstOrDefault(),
                            子分類 = b.CategoryDetails.Select(x => x.SubCategory.SubCategoryName).FirstOrDefault(),
                            子分類ID = b.CategoryDetails.Select(x => x.SubCategory.SubCategoryId).FirstOrDefault(),
                            折扣 = b.BookDiscountDetails.Select(x => x.BookDiscount.BookDiscountAmount).FirstOrDefault(),
                            截止日 = b.BookDiscountDetails.Select(x => x.BookDiscountEndDate).FirstOrDefault(),
                        };
            List<MenuItem> menuItems = new List<MenuItem>();
            foreach(var item in books)
            {
                Book b = new Book
                {
                    BookTitle = item.書名,
                    BookId = item.書本ID,
                    UnitPrice = item.定價,
                    CoverPath = item.路徑,
                };
                BookDiscount bd = new BookDiscount { BookDiscountAmount = item.折扣 };
                BookDiscountDetail bdd = new BookDiscountDetail { BookDiscountEndDate = item.截止日 };
                Author a = new Author { AuthorName = item.作者 };
                Category c = new Category { CategoryName = item.分類,CategoryId = item.分類ID};
                SubCategory sc = new SubCategory { SubCategoryName = item.子分類, SubCategoryId = item.子分類ID};
                MenuItem tmp = new MenuItem();
                tmp.book = b;
                tmp.bookDiscount = bd;
                tmp.bookDiscountDetail = bdd;
                tmp.category = c;
                tmp.subCategory = sc;
                tmp.author = a;
                menuItems.Add(tmp);
            }

            List<Artical> articals = getArticals();
            List<Category> categories = getCategories();
            
            MenuInformation menuInformation = new MenuInformation
            {
                articals = articals,
                categories = categories,
                menuItems = menuItems,
            };
            return View(menuInformation);
        }
        public List<Artical> getArticals()
        {
            using (var db = new BookShopContext())
            {
                var query = from ac in db.Articals
                            orderby ac.ArticalId descending
                            select ac;
                List<Artical> articals = new List<Artical>();
                foreach(var ac in query)
                {
                    articals.Add(ac);
                }
                return articals;
            }
        }

        public List<Category> getCategories()
        {
            using (var db = new BookShopContext())
            {
                var query = from c in db.Categories
                            select c;
                List<Category> categories = new List<Category>();
                foreach (var c in query)
                {
                    categories.Add(c);
                }
                return categories;
            }
        }
    }
}