using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.ProjectModel;
using prjBookMvcCore.Models;
using System.Linq;
using System.Net;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace prjBookMvcCore.Controllers
{
    public class BookController : Controller
    {
        BookShopContext db = new();

        public IActionResult BookInformation()
        {
            int bookId = 5;
            var query = from b in db.Books
                        where b.BookId == bookId
                        select new
                        {
                            書本ID = b.BookId,
                            書名 = b.BookTitle,
                            ISBN = b.Isbn,
                            作者 = b.AuthorDetails.Select(x => x.Author.AuthorName).FirstOrDefault(),
                            譯者 = b.TranslatorDetails.Select(x => x.Translator.TranslatorName).FirstOrDefault(),
                            繪者 = b.PainterDetails.Select(x => x.Painter.PainterName).FirstOrDefault(),
                            出版社 = b.Publisher.PublisherName,
                            出版日期 = b.PublicationDate,
                            語言 = b.Language.LanguageName,
                            定價 = b.UnitPrice,
                            封裝方法 = b.BindingMethod,
                            頁數 = b.Pages,
                            規格 = b.Dimensions,
                            庫存 = b.UnitInStock,
                            分類 = b.CategoryDetails.Select(x => x.SubCategory.Category.CategoryName).FirstOrDefault(),
                            子分類 = b.CategoryDetails.Select(x => x.SubCategory.SubCategoryName).FirstOrDefault(),
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
                Publisher p = new Publisher { PublisherName = item.出版社 };
                Language l = new Language { LanguageName = item.語言 };
                Category c = new Category { CategoryName = item.分類 };
                SubCategory sc = new SubCategory { SubCategoryName = item.子分類 };
                Translator t = new Translator { TranslatorName = item.譯者 };
                Painter pt = new Painter { PainterName = item.繪者 };
                Author a = new Author { AuthorName = item.作者 };
                Preview pv = new Preview { PreviewImagePath1 = item.試讀1, PreviewImagePath2 = item.試讀2, PreviewImagePath3 = item.試讀3, PreviewImagePath4 = item.試讀4 };

                List<CommentInformation> cis = getCommentInformation(bookId);

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
    }
}
