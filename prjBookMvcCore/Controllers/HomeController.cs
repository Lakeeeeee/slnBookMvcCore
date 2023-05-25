using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient.Server;
using prjBookMvcCore.Models;
using System.Diagnostics;
using System.Net;
using NuGet.Protocol;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;
using static System.Formats.Asn1.AsnWriter;
using static System.Reflection.Metadata.BlobBuilder;
using static Microsoft.AspNetCore.Razor.Language.TagHelperMetadata;

namespace prjBookMvcCore.Controllers
{
    public class HomeController : Controller
    {
        BookShopContext db = new();
        public IActionResult Home()
        {
            //從資料庫中取得最小值和最大值
            decimal minprice = db.Books.Min(b => b.UnitPrice);
            decimal maxprice = db.Books.Max(b => b.UnitPrice);
            ViewBag.frontprice = minprice.ToString("0");
            ViewBag.backprice = maxprice.ToString("0");

            DateTime mindate = DateTime.Now, maxdate = DateTime.Now;
            int maxtime = 0;
            int mintime = 100000;

            List<DateTime> d = db.Books.Select(x => x.PublicationDate).ToList();

            foreach (var date in d)
            {
                int diff = (DateTime.Now - date).Days;
                if (diff < mintime)
                {
                    mintime = diff;
                    mindate = date;
                }

                if (diff > maxtime)
                {
                    maxtime = diff;
                    maxdate = date;
                }
            }

            ViewBag.frontdate = mindate.ToString("yyyy-MM-dd");
            ViewBag.backdate = (maxdate).ToString("yyyy-MM-dd");

            List<RecommendInformation> normal = getNormal();
            List<RecommendInformation> publicationDate = getPublicationDate();
            List<RecommendInformation> commentTimeDate = getCommentTimeDate();
            List<RecommendInformation> quantity = getQuantity();

            CForHomePage c = new CForHomePage()
            {
                normal = normal,
                commentTimeDate = commentTimeDate,
                publicationDate = publicationDate,
                quantity = quantity,
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

        public IActionResult commentStarList(int countStar)
        {
            List<RecommendInformation> commentStar = GetProductByStar(countStar);
            CForHomePage c = new CForHomePage()
            {
                commentStar = commentStar
            };
            ViewData["countStar"] = countStar;
            return View(c);
        }

        public List<RecommendInformation> GetProductByStar(int countStar)
        {
            using (var db = new BookShopContext())
            {
                var recommendProducts = (from b in db.Books
                                         join c in db.Comments on b.BookId equals c.BookId
                                         where c.Stars == countStar
                                         select new
                                         {
                                             書本ID = b.BookId,
                                             書名 = b.BookTitle,
                                             定價 = b.UnitPrice,
                                             路徑 = b.CoverPath,
                                             折扣 = b.BookDiscountDetails.Select(x => x.BookDiscount.BookDiscountAmount).FirstOrDefault(),
                                             出版日期 = b.PublicationDate,
                                             評論時間 = c.CommentTime,
                                             最新評論 = c.CommentText,
                                             評分 = c.Stars,
                                         }).ToList();

                List<RecommendInformation> ris = new List<RecommendInformation>();

                foreach (var recommendBook in recommendProducts)
                {
                    Book b = new Book { BookTitle = recommendBook.書名, BookId = recommendBook.書本ID, UnitPrice = recommendBook.定價, CoverPath = recommendBook.路徑, PublicationDate = recommendBook.出版日期 };
                    BookDiscount bd = new BookDiscount { BookDiscountAmount = recommendBook.折扣 };
                    Comment c = new Comment { CommentText = recommendBook.最新評論, Stars = recommendBook.評分, CommentTime = recommendBook.評論時間 };
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


        public IActionResult QA()
        {
            return View();
        }

        public IActionResult searchList(string txtKeyword, decimal frontPrice, decimal backPrice, decimal frontdiscount, decimal backdiscount, DateTime frontdate, DateTime backdate)
        {
            ViewBag.KeyWord = txtKeyword;
            ViewBag.frontprice = frontPrice;
            ViewBag.backprice = backPrice;
            ViewBag.frontdiscount = frontdiscount;
            ViewBag.backdiscount = backdiscount;
            ViewBag.frontdate = frontdate;
            ViewBag.backdate = backdate;

            //從資料庫中取得最小值和最大值
            decimal minprice = db.Books.Min(b => b.UnitPrice);
            decimal maxprice = db.Books.Max(b => b.UnitPrice);
            ViewBag.frontprice = minprice.ToString("0");
            ViewBag.backprice = maxprice.ToString("0");

            DateTime mindate = DateTime.Now, maxdate = DateTime.Now;
            int maxtime = 0;
            int mintime = 100000;

            List<DateTime> d = db.Books.Select(x => x.PublicationDate).ToList();



            ViewBag.frontdiscount = frontdiscount;
            ViewBag.backdiscount = backdiscount;

            foreach (var date in d)
            {
                int diff = (DateTime.Now - date).Days;
                if (diff < mintime)
                {
                    mintime = diff;
                    mindate = date;
                }

                if (diff > maxtime)
                {
                    maxtime = diff;
                    maxdate = date;
                }
            }

            ViewBag.frontdate = mindate.ToString("yyyy-MM-dd");
            ViewBag.backdate = (maxdate).ToString("yyyy-MM-dd");

            CForHomePage searchresult = GetSearchResult(txtKeyword, frontPrice, backPrice, frontdiscount, backdiscount, frontdate, backdate);
            return View(searchresult);
        }

        private CForHomePage GetSearchResult(string txtkeyword, decimal frontPrice, decimal backPrice, decimal frontdiscount, decimal backdiscount, DateTime frontdate, DateTime backdate)
        {
            using (var db = new BookShopContext())
            {
                IQueryable<dynamic> recommendBooks = null;
                if (txtkeyword != null && frontPrice != 0 && backPrice != 0 && frontdiscount != 0 && backdiscount != 0 && frontdate != null && backdate != null)
                {
                    recommendBooks = (from b in db.Books
                                      where
                                      ((b.BookTitle.Contains(txtkeyword) ||
                                      b.AuthorDetails.Select(x => x.Author.AuthorName).FirstOrDefault().Contains(txtkeyword)) ||
                                      b.PainterDetails.Select(x => x.Painter.PainterName).FirstOrDefault().Contains(txtkeyword) ||
                                      b.TranslatorDetails.Select(x => x.Translator.TranslatorName).FirstOrDefault().Contains(txtkeyword) ||
                                      b.Publisher.PublisherName.Contains(txtkeyword)) &&

                                      frontPrice <= b.UnitPrice && backPrice >= b.UnitPrice &&

                                      frontdiscount >= b.BookDiscountDetails.Select(x => x.BookDiscount.BookDiscountAmount).FirstOrDefault() &&
                                      backdiscount <= b.BookDiscountDetails.Select(x => x.BookDiscount.BookDiscountAmount).FirstOrDefault() &&

                                      frontdate <= b.PublicationDate && backdate >= b.PublicationDate

                                      select new
                                      {
                                          書本ID = b.BookId,
                                          書名 = b.BookTitle,
                                          作者 = b.AuthorDetails.Select(x => x.Author.AuthorName).FirstOrDefault(),
                                          繪者 = b.PainterDetails.Select(x => x.Painter.PainterName).FirstOrDefault(),
                                          譯者 = b.TranslatorDetails.Select(x => x.Translator.TranslatorName).FirstOrDefault(),
                                          出版社 = b.Publisher.PublisherName,
                                          定價 = b.UnitPrice,
                                          路徑 = b.CoverPath,
                                          折扣 = b.BookDiscountDetails.Select(x => x.BookDiscount.BookDiscountAmount).FirstOrDefault(),
                                          出版日期 = b.PublicationDate,
                                      }).Distinct();
                }
                else if (txtkeyword == null && frontPrice != 0 && backPrice != 0 && frontdiscount != 0 && backdiscount != 0 && frontdate != null && backdate != null)
                {
                    recommendBooks = (from b in db.Books
                                      where
                                      frontPrice <= b.UnitPrice && backPrice >= b.UnitPrice &&

                                      frontdiscount >= b.BookDiscountDetails.Select(x => x.BookDiscount.BookDiscountAmount).FirstOrDefault() &&
                                      backdiscount <= b.BookDiscountDetails.Select(x => x.BookDiscount.BookDiscountAmount).FirstOrDefault() &&

                                      frontdate <= b.PublicationDate && backdate >= b.PublicationDate
                                      select new
                                      {
                                          書本ID = b.BookId,
                                          書名 = b.BookTitle,
                                          作者 = b.AuthorDetails.Select(x => x.Author.AuthorName).FirstOrDefault(),
                                          繪者 = b.PainterDetails.Select(x => x.Painter.PainterName).FirstOrDefault(),
                                          譯者 = b.TranslatorDetails.Select(x => x.Translator.TranslatorName).FirstOrDefault(),
                                          出版社 = b.Publisher.PublisherName,
                                          定價 = b.UnitPrice,
                                          路徑 = b.CoverPath,
                                          折扣 = b.BookDiscountDetails.Select(x => x.BookDiscount.BookDiscountAmount).FirstOrDefault(),
                                          出版日期 = b.PublicationDate,
                                      }).Distinct();
                }
                else
                {
                    recommendBooks = (from b in db.Books
                                      where
                                      (b.BookTitle.Contains(txtkeyword) ||
                                      b.AuthorDetails.Select(x => x.Author.AuthorName).FirstOrDefault().Contains(txtkeyword)) ||
                                      b.PainterDetails.Select(x => x.Painter.PainterName).FirstOrDefault().Contains(txtkeyword) ||
                                      b.TranslatorDetails.Select(x => x.Translator.TranslatorName).FirstOrDefault().Contains(txtkeyword) ||
                                      b.Publisher.PublisherName.Contains(txtkeyword) &&

                                      frontPrice <= b.UnitPrice && backPrice >= b.UnitPrice &&

                                      frontdiscount >= b.BookDiscountDetails.Select(x => x.BookDiscount.BookDiscountAmount).FirstOrDefault() &&
                                      backdiscount <= b.BookDiscountDetails.Select(x => x.BookDiscount.BookDiscountAmount).FirstOrDefault() &&

                         frontdate <= b.PublicationDate && backdate >= b.PublicationDate

                                      select new
                                      {
                                          書本ID = b.BookId,
                                          書名 = b.BookTitle,
                                          作者 = b.AuthorDetails.Select(x => x.Author.AuthorName).FirstOrDefault(),
                                          繪者 = b.PainterDetails.Select(x => x.Painter.PainterName).FirstOrDefault(),
                                          譯者 = b.TranslatorDetails.Select(x => x.Translator.TranslatorName).FirstOrDefault(),
                                          出版社 = b.Publisher.PublisherName,
                                          定價 = b.UnitPrice,
                                          路徑 = b.CoverPath,
                                          折扣 = b.BookDiscountDetails.Select(x => x.BookDiscount.BookDiscountAmount).FirstOrDefault(),
                                          出版日期 = b.PublicationDate,
                                      }).Distinct();
                }

                CForHomePage cForHomePage = new CForHomePage();
                List<CInformation> cis = new List<CInformation>();
                int count = 0;
                foreach (var recommendBook in recommendBooks)
                {
                    count++;

                    Book b = new Book { BookTitle = recommendBook.書名, BookId = recommendBook.書本ID, UnitPrice = recommendBook.定價, CoverPath = recommendBook.路徑, PublicationDate = recommendBook.出版日期 };
                    BookDiscount bd = new BookDiscount { BookDiscountAmount = recommendBook.折扣 };
                    Author au = new Author { AuthorName = recommendBook.作者 };
                    Painter pa = new Painter { PainterName = recommendBook.繪者 };
                    Translator tr = new Translator { TranslatorName = recommendBook.譯者 };
                    Publisher pub = new Publisher { PublisherName = recommendBook.出版社 };
                    CInformation tmp = new CInformation()
                    {
                        book = b,
                        bookDiscount = bd,
                        author = au,
                        painter = pa,
                        translator = tr,
                        publisher = pub,
                    };
                    cis.Add(tmp);
                }
                cForHomePage.cSearchBook = cis;
                return cForHomePage;
            }
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
                            作者ID = b.AuthorDetails.Select(x => x.Author.AuthorId).FirstOrDefault(),
                            分類 = b.CategoryDetails.Select(x => x.SubCategory.Category.CategoryName).FirstOrDefault(),
                            分類ID = b.CategoryDetails.Select(x => x.SubCategory.CategoryId).FirstOrDefault(),
                            子分類 = b.CategoryDetails.Select(x => x.SubCategory.SubCategoryName).FirstOrDefault(),
                            子分類ID = b.CategoryDetails.Select(x => x.SubCategory.SubCategoryId).FirstOrDefault(),
                            折扣 = b.BookDiscountDetails.Select(x => x.BookDiscount.BookDiscountAmount).FirstOrDefault(),
                            銷售數量 = b.OrderDetails.Select(x => x.Quantity).FirstOrDefault(),
                            出版日期 = b.PublicationDate,
                        };
            List<MenuItem> menuItems = new List<MenuItem>();
            foreach (var item in books)
            {
                Book b = new Book
                {
                    BookTitle = item.書名,
                    BookId = item.書本ID,
                    UnitPrice = item.定價,
                    CoverPath = item.路徑,
                    PublicationDate = item.出版日期,
                };
                BookDiscount bd = new BookDiscount { BookDiscountAmount = item.折扣 };
                Author a = new Author { AuthorName = item.作者, AuthorId = item.作者ID };
                Category c = new Category { CategoryName = item.分類, CategoryId = item.分類ID };
                SubCategory sc = new SubCategory { SubCategoryName = item.子分類, SubCategoryId = item.子分類ID };
                OrderDetail od = new OrderDetail { Quantity = item.銷售數量 };
                MenuItem tmp = new MenuItem();
                tmp.book = b;
                tmp.bookDiscount = bd;
                tmp.category = c;
                tmp.subCategory = sc;
                tmp.author = a;
                tmp.orderDetail = od;
                menuItems.Add(tmp);
            }

            List<Artical> articals = getArticals();
            List<Category> categories = getCategories();
            List<SubCategory> subCategories = getSubCategories();
            List<OrderDetail> orderDetails = getQuantities();
            MenuInformation menuInformation = new MenuInformation
            {
                articals = articals,
                categories = categories,
                subCategories = subCategories,
                menuItems = menuItems,
                orderDetails = orderDetails,
            };
            return View(menuInformation);
        }

        private List<OrderDetail> getQuantities()
        {
            using (var db = new BookShopContext())
            {
                var query = from od in db.OrderDetails

                            orderby od.Quantity descending
                            select od;
                List<OrderDetail> orderDetails = new List<OrderDetail>();
                foreach (var od in query)
                {
                    orderDetails.Add(od);
                }
                return orderDetails;
            }
        }

        public List<Artical> getArticals()
        {
            using (var db = new BookShopContext())
            {
                var query = from ac in db.Articals
                            orderby ac.ArticalId descending
                            select ac;
                List<Artical> articals = new List<Artical>();
                foreach (var ac in query)
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
                            出版日期 = b.PublicationDate,
                            折扣 = b.BookDiscountDetails.Select(x => x.BookDiscount.BookDiscountAmount).FirstOrDefault(),
                        };

            foreach (var recommendBook in query)
            {
                Book b = new Book
                {
                    BookTitle = recommendBook.書名,
                    BookId = recommendBook.書本ID,
                    UnitPrice = recommendBook.定價,
                    PublicationDate = recommendBook.出版日期,
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
                                          評論時間 = c.CommentTime,
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
                    Comment c = new Comment { CommentText = recommendBook.最新評論, Stars = recommendBook.評分, CommentTime = recommendBook.評論時間 };
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
                                      orderby c.CommentTime descending
                                      orderby c.Stars descending
                                      select new
                                      {
                                          書本ID = b.BookId,
                                          書名 = b.BookTitle,
                                          定價 = b.UnitPrice,
                                          路徑 = b.CoverPath,
                                          折扣 = b.BookDiscountDetails.Select(x => x.BookDiscount.BookDiscountAmount).FirstOrDefault(),
                                          出版日期 = b.PublicationDate,
                                          評論時間 = c.CommentTime,
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
                    Comment c = new Comment { CommentText = recommendBook.最新評論, Stars = recommendBook.評分, CommentTime = recommendBook.評論時間 };
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

        public List<SubCategory> getSubCategories()
        {
            using (var db = new BookShopContext())
            {
                var query = from sc in db.SubCategories
                            select sc;
                List<SubCategory> subCategories = new List<SubCategory>();
                foreach (var sc in query)
                {
                    subCategories.Add(sc);
                }
                return subCategories;
            }
        }

        public List<RecommendInformation> getQuantity()
        {
            using (var db = new BookShopContext())
            {
                var recommendBooks = (from b in db.Books
                                      join od in db.OrderDetails
                                      on b.BookId equals od.BookId
                                      orderby od.Quantity descending
                                      select new
                                      {
                                          書本ID = b.BookId,
                                          書名 = b.BookTitle,
                                          定價 = b.UnitPrice,
                                          路徑 = b.CoverPath,
                                          折扣 = b.BookDiscountDetails.Select(x => x.BookDiscount.BookDiscountAmount).FirstOrDefault(),
                                          出版日期 = b.PublicationDate,
                                          銷售數量 = b.OrderDetails.Select(x => x.Quantity).FirstOrDefault()
                                      });
                List<RecommendInformation> ris = new List<RecommendInformation>();

                foreach (var recommendBook in recommendBooks)
                {
                    Book b = new Book { BookTitle = recommendBook.書名, BookId = recommendBook.書本ID, UnitPrice = recommendBook.定價, CoverPath = recommendBook.路徑, PublicationDate = recommendBook.出版日期 };
                    BookDiscount bd = new BookDiscount { BookDiscountAmount = recommendBook.折扣 };
                    OrderDetail od = new OrderDetail { Quantity = recommendBook.銷售數量 };
                    RecommendInformation tmp = new RecommendInformation()
                    {
                        book = b,
                        bookDiscount = bd,
                        orderDetail = od,
                    };
                    ris.Add(tmp);
                }
                return ris;
            }
        }

        public IActionResult askMe()
        {
            return View();
        }

    }


}