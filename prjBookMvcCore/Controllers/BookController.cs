using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.ProjectModel;
using prjBookMvcCore.Models;
using System.Linq;
using System.Net;
using System.Xml.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace prjBookMvcCore.Controllers
{
    public class BookController : Controller
    {
        BookShopContext db = new();
        public IActionResult BookInformation(int id)
        {
            int bookId = id;

            //莫名其妙的bug，隨便設一個
            if(bookId == 0)
            {
                bookId = 3;
            }
            var query = from b in db.Books
                        where b.BookId == bookId
                        select new
                        {
                            書本ID = b.BookId,
                            書名 = b.BookTitle,
                            ISBN = b.Isbn,
                            作者ID = b.AuthorDetails.Select(x => x.AuthorId).FirstOrDefault(),
                            作者 = b.AuthorDetails.Select(x => x.Author.AuthorName).FirstOrDefault(),
                            多個作者 = b.AuthorDetails.Select(x => x.Author.AuthorName).ToList(),
                            多個作者ID = b.AuthorDetails.Select(x => x.Author.AuthorId).ToList(),
                            譯者 = b.TranslatorDetails.Select(x => x.Translator.TranslatorName).FirstOrDefault(),
                            繪者 = b.PainterDetails.Select(x => x.Painter.PainterName).FirstOrDefault(),
                            出版社ID = b.Publisher.PublisherId,
                            譯者ID = b.TranslatorDetails.Select(x => x.TranslatorId).FirstOrDefault(),
                            繪者ID = b.PainterDetails.Select(x => x.PainterId).FirstOrDefault(),
                            出版社 = b.Publisher.PublisherName,
                            出版日期 = b.PublicationDate,
                            語言 = b.Language.LanguageName,
                            定價 = b.UnitPrice,
                            封裝方法 = b.BindingMethod,
                            頁數 = b.Pages,
                            規格 = b.Dimensions,
                            庫存 = b.UnitInStock,
                            分類 = b.CategoryDetails.Select(x => x.SubCategory.Category.CategoryName).FirstOrDefault(),
                            分類ID = b.CategoryDetails.Select(x => x.SubCategory.Category.CategoryId).FirstOrDefault(),
                            子分類 = b.CategoryDetails.Select(x => x.SubCategory.SubCategoryName).FirstOrDefault(),
                            子分類ID = b.CategoryDetails.Select(x => x.SubCategory.SubCategoryId).FirstOrDefault(),
                            路徑 = b.CoverPath,
                            詳細資料 = b.Introduction,
                            作者介紹 = b.AboutAuthor,
                            內容簡介 = b.ContentIntro,
                            章節試閱 = b.TryContent,
                            各界推薦 = b.Endorsements,
                            作者序 = b.Foreward,
                            目錄 = b.TableContainer,
                            試讀1 = b.Previews.Select(x => x.PreviewImagePath1).FirstOrDefault(),
                            試讀2 = b.Previews.Select(x => x.PreviewImagePath2).FirstOrDefault(),
                            試讀3 = b.Previews.Select(x => x.PreviewImagePath3).FirstOrDefault(),
                            試讀4 = b.Previews.Select(x => x.PreviewImagePath4).FirstOrDefault(),
                            折扣 = b.BookDiscountDetails.Where(x=>x.BookDiscountStartDate < DateTime.Now & x.BookDiscountEndDate > DateTime.Now).Select(x => x.BookDiscount.BookDiscountAmount).FirstOrDefault(),
                            折扣名稱 = b.BookDiscountDetails.Select(x => x.BookDiscount.BookDiscountName).FirstOrDefault(),
                            截止日 = b.BookDiscountDetails.Select(x => x.BookDiscountEndDate).FirstOrDefault(),
                        };
            foreach (var item in query)
            {
                Book b = new Book
                {
                    BookId = item.書本ID,
                    BookTitle = item.書名,
                    PublicationDate = item.出版日期,
                    UnitPrice = item.定價,
                    Isbn = item.ISBN,
                    BindingMethod = item.封裝方法,
                    Pages = item.頁數,
                    Dimensions = item.規格,
                    UnitInStock = item.庫存,
                    CoverPath = item.路徑,
                    Introduction = item.詳細資料,
                    AboutAuthor = item.作者介紹,
                    ContentIntro = item.內容簡介,
                    TryContent = item.章節試閱,
                    Endorsements = item.各界推薦,
                    Foreward = item.作者序,
                    TableContainer = item.目錄,
                };
                Publisher p = new Publisher { PublisherName = item.出版社 , PublisherId = item.出版社ID};
                Language l = new Language { LanguageName = item.語言 };
                Category c = new Category { CategoryName = item.分類, CategoryId = item.分類ID };
                SubCategory sc = new SubCategory { SubCategoryName = item.子分類, SubCategoryId = item.子分類ID };
                Translator t = new Translator { TranslatorName = item.譯者, TranslatorId = item.譯者ID};
                Painter pt = new Painter { PainterName = item.繪者, PainterId = item.繪者ID };
                Author a = new Author { AuthorName = item.作者, AuthorId = item.作者ID};
                Preview pv = new Preview { PreviewImagePath1 = item.試讀1, PreviewImagePath2 = item.試讀2, PreviewImagePath3 = item.試讀3, PreviewImagePath4 = item.試讀4 };
                BookDiscount bd = new BookDiscount { BookDiscountAmount = item.折扣, BookDiscountName = item.折扣名稱};
                BookDiscountDetail bdd = new BookDiscountDetail { BookDiscountEndDate = item.截止日 };

                List<Author> aa = new List<Author>();
                for(int i = 0; i < item.多個作者.Count(); i++)
                {
                    Author tmp = new Author();
                    tmp.AuthorName = item.多個作者[i];
                    tmp.AuthorId = item.多個作者ID[i];
                    aa.Add(tmp);
                }

                List<CommentInformation> cis = getCommentInformation(bookId);

                List<RecommendInformation> recommendBooks = getRecommendBooks(item.分類,bookId);

                Artical ac = getArtical(bookId);

                CInformation newBook = new CInformation
                {
                    book = b,
                    publisher = p,
                    language = l,
                    category = c,
                    subCategory = sc,
                    author = a,
                    translator = t,
                    painter = pt,
                    commentInformations = cis,
                    preview = pv,
                    recommendBooks = recommendBooks,
                    bookDiscount = bd,
                    bookDiscountDetail = bdd,
                    artical = ac,
                    authors = aa,
                };

                return View(newBook);
            }
            return View();
        }

        public List<CommentInformation> getCommentInformation(int bookId)
        {
            using (var db = new BookShopContext())
            {
                var comments = (from ct in db.Comments
                                join m in db.Members on ct.MemberId equals m.MemberId
                                orderby ct.CommentTime descending
                                where ct.BookId == bookId
                                select new
                                {
                                    留言內容 = ct.CommentText,
                                    留言時間 = ct.CommentTime,
                                    留言作者 = m.MemberName,
                                    留言作者ID = ct.MemberId,
                                    評分 = ct.Stars,
                                }).ToList();
                List<CommentInformation> cis = new List<CommentInformation>();
                foreach (var comment in comments)
                {
                    Comment cc = new Comment { CommentText = comment.留言內容, CommentTime = comment.留言時間, Stars = comment.評分, MemberId = comment.留言作者ID};
                    Member m = new Member { MemberName = comment.留言作者 };
                    CommentInformation tmp = new CommentInformation()
                    {
                        _comment = cc,
                        _member = m,
                    };
                    cis.Add(tmp);
                }
                return cis;
            }
        }
        public List<RecommendInformation> getRecommendBooks(string categoryName,int bookId)
        {
            using (var db = new BookShopContext())
            {
                var recommendBooks = from b in db.Books
                                     join sd in db.CategoryDetails
                                     on b.BookId equals sd.BookId
                                     join sc in db.SubCategories
                                     on sd.SubCategoryId equals sc.SubCategoryId
                                     where sc.Category.CategoryName == categoryName && b.BookId != bookId
                                     select new
                                     {
                                         書本ID = b.BookId,
                                         書名 = b.BookTitle,
                                         定價 = b.UnitPrice,
                                         路徑 = b.CoverPath,
                                         折扣 = b.BookDiscountDetails.Where(x => x.BookDiscountStartDate < DateTime.Now & x.BookDiscountEndDate > DateTime.Now).Select(x => x.BookDiscount.BookDiscountAmount).FirstOrDefault(),
                                         截止日 = b.BookDiscountDetails.Select(x => x.BookDiscountEndDate).FirstOrDefault(),
                                     };
                List<RecommendInformation> ris = new List<RecommendInformation>();
                foreach (var recommendBook in recommendBooks)
                {
                    Book b = new Book { BookTitle = recommendBook.書名, BookId = recommendBook.書本ID, UnitPrice = recommendBook.定價, CoverPath = recommendBook.路徑 };
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

        public IActionResult authorInformation(int id)
        {
            using (var db = new BookShopContext())
            {
                var q = from b in db.Books.Include("AuthorDetails.Author")
                        where b.AuthorDetails.Any(ad => ad.AuthorId == id)
                        orderby b.PublicationDate descending
                        select b;
                ViewData["authorID"] = id;
                return View(q.ToList());

            }
        }
        public IActionResult translatorInformation(int id)
        {
            using (var db = new BookShopContext())
            {
                var q = from b in db.Books.Include("TranslatorDetails.Translator")
                        where b.TranslatorDetails.Any(td => td.TranslatorId == id)
                        orderby b.PublicationDate descending
                        select b;
                ViewData["translatorID"] = id;
                return View(q.ToList());
            }
        }
        public IActionResult painterInformation(int id)
        {
            using (var db = new BookShopContext())
            {
                var q = from b in db.Books.Include("PainterDetails.Painter")
                        where b.PainterDetails.Any(pd => pd.PainterId == id)
                        orderby b.PublicationDate descending
                        select b;
                ViewData["painterID"] = id;
                return View(q.ToList());
            }
        }
        public IActionResult publisherInformation(int id)
        {
            using (var db = new BookShopContext())
            {
                var q = from b in db.Books.Include("Publisher")
                        where b.PublisherId == id
                        orderby b.PublicationDate descending
                        select b;
                ViewData["publisherID"] = id;
                return View(q.ToList());

            }
        }

        public Artical getArtical(int bookID)
        {
            using (var db = new BookShopContext())
            {
                var artical = from atbd in db.ArticalToBookDetails
                              join ac in db.Articals
                              on atbd.ArticalId equals ac.ArticalId
                              where atbd.BookId == bookID
                              select ac;
                if(artical.Count() != 0)
                {
                    Artical newArtical = new Artical();
                    foreach(var item in artical)
                    {
                        newArtical = item;
                    }
                    return newArtical;
                }
                else
                {
                    Random random = new Random();
                    int index = random.Next(30);
                    Artical newArtical = new Artical();
                    var query = from ac in db.Articals
                                 where ac.ArticalId == index
                                 select ac;
                    foreach (var item in query)
                    {
                        newArtical = item;
                    }
                    return newArtical;
                }
            }
        }

    }
}
