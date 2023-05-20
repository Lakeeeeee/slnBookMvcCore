﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient.Server;
using prjBookMvcCore.Models;
using System.Diagnostics;
using System.Net;
using NuGet.Protocol;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;
using static System.Formats.Asn1.AsnWriter;
using static System.Reflection.Metadata.BlobBuilder;

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

        public IActionResult searchList(string keyword)
        {
            List<RecommendInformation> searchResult = GetSearchResult();
            CForHomePage c = new CForHomePage()
            {
                searchResult = searchResult,
            };
            var results = searchResult.Where(item =>
        (item.book != null && item.book.BookTitle.Contains(keyword)) ||
        (item.author != null && item.author.AuthorName.Contains(keyword)) ||
        (item.painter != null && item.painter.PainterName.Contains(keyword)) ||
        (item.translator != null && item.translator.TranslatorName.Contains(keyword)) ||
        (item.publisher != null && item.publisher.PublisherName.Contains(keyword))
    );

            return View(results);
        }


        private List<RecommendInformation> GetSearchResult()
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
                                      join au in db.AuthorDetails
                                      on b.BookId equals au.BookId
                                      join pa in db.PainterDetails
                                      on b.BookId equals pa.BookId
                                      join tr in db.TranslatorDetails
                                      on b.BookId equals tr.BookId
                                      join pub in db.Publishers
                                      on b.PublisherId equals pub.PublisherId

                                      orderby c.CommentTime ascending
                                      orderby c.Stars descending
                                      select new
                                      {
                                          書本ID = b.BookId,
                                          書名 = b.BookTitle,
                                          作者 = au.Author.AuthorName,
                                          繪者 = pa.Painter.PainterName,
                                          譯者 = tr.Translator.TranslatorName,
                                          出版社 = pub.PublisherName,
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
                    Author au = new Author { AuthorName = recommendBook.作者 };
                    Painter pa = new Painter { PainterName = recommendBook.繪者 };
                    Translator tr = new Translator { TranslatorName = recommendBook.譯者 };
                    Publisher pub = new Publisher { PublisherName = recommendBook.出版社 };
                    RecommendInformation tmp = new RecommendInformation()
                    {
                        book = b,
                        bookDiscount = bd,
                        comment = c,
                        author = au,
                        painter = pa,
                        translator = tr,
                        publisher = pub,
                    };
                    ris.Add(tmp);
                }
                return ris;
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
                            分類 = b.CategoryDetails.Select(x => x.SubCategory.Category.CategoryName).FirstOrDefault(),
                            分類ID = b.CategoryDetails.Select(x => x.SubCategory.CategoryId).FirstOrDefault(),
                            子分類 = b.CategoryDetails.Select(x => x.SubCategory.SubCategoryName).FirstOrDefault(),
                            子分類ID = b.CategoryDetails.Select(x => x.SubCategory.SubCategoryId).FirstOrDefault(),
                            折扣 = b.BookDiscountDetails.Select(x => x.BookDiscount.BookDiscountAmount).FirstOrDefault(),
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
                Author a = new Author { AuthorName = item.作者 };
                Category c = new Category { CategoryName = item.分類, CategoryId = item.分類ID };
                SubCategory sc = new SubCategory { SubCategoryName = item.子分類, SubCategoryId = item.子分類ID };
                MenuItem tmp = new MenuItem();
                tmp.book = b;
                tmp.bookDiscount = bd;
                tmp.category = c;
                tmp.subCategory = sc;
                tmp.author = a;
                menuItems.Add(tmp);
            }

            List<Artical> articals = getArticals();
            List<Category> categories = getCategories();
            List<SubCategory> subCategories = getSubCategories();

            MenuInformation menuInformation = new MenuInformation
            {
                articals = articals,
                categories = categories,
                subCategories = subCategories,
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
    }
}