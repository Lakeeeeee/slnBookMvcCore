using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient.Server;
using prjBookMvcCore.Models;
using System.Diagnostics;

namespace prjBookMvcCore.Controllers
{
    public class HomeController : Controller
    {
        BookShopContext db = new();

        public IActionResult Home()
        {
            List<RecommendInformation> normal = getNormal();
            List<RecommendInformation> publicationDate = getPublicationDate();
            List<RecommendInformation> commentTimeDate = getCommentTimeDate();

            CForHomePage c = new CForHomePage()
            {
                normal = normal,
                commentTimeDate = commentTimeDate,
                publicationDate = publicationDate,
            };
            return View(c);
        }

        public IActionResult commentList()
        {
            List<RecommendInformation> commentTimeDate = getCommentTimeDateForList();
            CForHomePage c = new CForHomePage()
            {
                commentTimeDate = commentTimeDate,
            };
            return View(c);
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

        public List<RecommendInformation> getNormal()
        {
            Random random = new Random();
            List<int> randomList = new List<int>();
            for (int i = 0; i < 10; i++)
            {
                int n = random.Next(392);
                if (randomList.Contains(n))
                {
                    i--;
                }
                else
                {
                    randomList.Add(n);
                }
            }
            List<RecommendInformation> normal = new List<RecommendInformation>();

            var query = from b in db.Books
                        join sd in db.CategoryDetails on b.BookId equals sd.BookId
                        join sc in db.SubCategories on sd.SubCategoryId equals sc.SubCategoryId
                        where randomList.Contains(b.BookId)
                        select new
                        {
                            書本ID = b.BookId,
                            書名 = b.BookTitle,
                            定價 = b.UnitPrice,
                            路徑 = b.CoverPath,
                            折扣 = b.BookDiscountDetails.Select(x => x.BookDiscount.BookDiscountAmount).FirstOrDefault(),
                        };

            foreach (var recommendBook in query)
            {
                Book b = new Book
                {
                    BookTitle = recommendBook.書名,
                    BookId = recommendBook.書本ID,
                    UnitPrice = recommendBook.定價,
                    CoverPath = recommendBook.路徑
                };
                BookDiscount bd = new BookDiscount
                {
                    BookDiscountAmount = recommendBook.折扣
                };
                RecommendInformation tmp = new RecommendInformation()
                {
                    book = b,
                    bookDiscount = bd,
                };
                normal.Add(tmp);
            }

            return normal;
        }

        public List<RecommendInformation> getCommentTimeDateForList()
        {
            using (var db = new BookShopContext())
            {
                var recommendBooks = (from b in db.Books
                                      join sd in db.CategoryDetails
                                      on b.BookId equals sd.BookId
                                      join sc in db.SubCategories
                                      on sd.SubCategoryId equals sc.SubCategoryId
                                      join c in db.Comments
                                      on b.BookId equals c.BookId

                                      orderby c.CommentTime ascending
                                      orderby c.Stars descending
                                      select new
                                      {
                                          書本ID = b.BookId,
                                          書名 = b.BookTitle,
                                          定價 = b.UnitPrice,
                                          路徑 = b.CoverPath,
                                          折扣 = b.BookDiscountDetails.Select(x => x.BookDiscount.BookDiscountAmount).FirstOrDefault(),
                                          出版日期 = b.PublicationDate,
                                          最新評論 = c.CommentText,
                                          評分 = c.Stars,
                                      }).Take(20);
                List<RecommendInformation> ris = new List<RecommendInformation>();
                int count = 0;
                foreach (var recommendBook in recommendBooks)
                {
                    count++;

                    Book b = new Book { BookTitle = recommendBook.書名, BookId = recommendBook.書本ID, UnitPrice = recommendBook.定價, CoverPath = recommendBook.路徑, PublicationDate = recommendBook.出版日期 };
                    BookDiscount bd = new BookDiscount { BookDiscountAmount = recommendBook.折扣 };
                    Comment c = new Comment { CommentText = recommendBook.最新評論, Stars = recommendBook.評分 };
                    RecommendInformation tmp = new RecommendInformation()
                    {
                        book = b,
                        bookDiscount = bd,
                        comment = c,
                    };
                    ris.Add(tmp);
                }
                return ris;
            }
        }

        public List<RecommendInformation> getCommentTimeDate()
        {
            using (var db = new BookShopContext())
            {
                var recommendBooks = (from b in db.Books
                                     join sd in db.CategoryDetails
                                     on b.BookId equals sd.BookId
                                     join sc in db.SubCategories
                                     on sd.SubCategoryId equals sc.SubCategoryId
                                    join c in db.Comments
                                    on b.BookId equals c.BookId

                                     orderby c.CommentTime ascending
                                    
                                     select new
                                     {
                                         書本ID = b.BookId,
                                         書名 = b.BookTitle,
                                         定價 = b.UnitPrice,
                                         路徑 = b.CoverPath,
                                         折扣 = b.BookDiscountDetails.Select(x => x.BookDiscount.BookDiscountAmount).FirstOrDefault(),
                                         出版日期 = b.PublicationDate,
                                         最新評論 = c.CommentText,
                                         評分 = c.Stars,
                                     }).Take(5);
                List<RecommendInformation> ris = new List<RecommendInformation>();
                int count = 0;
                foreach (var recommendBook in recommendBooks)
                {
                    count++;
                   
                    Book b = new Book { BookTitle = recommendBook.書名, BookId = recommendBook.書本ID, UnitPrice = recommendBook.定價, CoverPath = recommendBook.路徑, PublicationDate = recommendBook.出版日期 };
                    BookDiscount bd = new BookDiscount { BookDiscountAmount = recommendBook.折扣 };
                    Comment c = new Comment { CommentText = recommendBook.最新評論 , Stars = recommendBook.評分};
                    RecommendInformation tmp = new RecommendInformation()
                    {
                        book = b,
                        bookDiscount = bd,
                        comment = c,
                    };
                    ris.Add(tmp);
                }
                return ris;
            }
        }

        public List<RecommendInformation> getPublicationDate()
        {
            using (var db = new BookShopContext())
            {
                var recommendBooks = (from b in db.Books
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
                                          出版日期 = b.PublicationDate,
                                      }).Take(10);
                List<RecommendInformation> ris = new List<RecommendInformation>();
                int count = 0;
                foreach (var recommendBook in recommendBooks)
                {
                    count++;

                    Book b = new Book { BookTitle = recommendBook.書名, BookId = recommendBook.書本ID, UnitPrice = recommendBook.定價, CoverPath = recommendBook.路徑, PublicationDate = recommendBook.出版日期 };
                    BookDiscount bd = new BookDiscount { BookDiscountAmount = recommendBook.折扣 };
                    RecommendInformation tmp = new RecommendInformation()
                    {
                        book = b,
                        bookDiscount = bd,
                    };
                    ris.Add(tmp);
                }
                return ris;
            }
        }
    }
}