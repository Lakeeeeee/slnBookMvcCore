using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prjBookMvcCore.Models;
using static prjBookMvcCore.Models.BooksCardViewModel;

namespace prjBookMvcCore.Controllers
{
    public class BookController : Controller
    {
        BookShopContext db = new();

        public IActionResult BookInformation()
        {
            return View();
        }

        public IActionResult _BookCardPartial(int? id)
        {
                var db = new BookShopContext();
            
                // Get book information
                var book = db.Books.FirstOrDefault(b => b.BookId == id);
                if (book == null)
                    return RedirectToAction("Home");

                // Get author information
                var authorDetail = db.AuthorDetails.FirstOrDefault(ad => ad.BookId == id);
                var authorName = "";
                if (authorDetail != null)
                {
                    var author = db.Authors.FirstOrDefault(a => a.AuthorId == authorDetail.AuthorId);
                    if (author != null)
                        authorName = author.AuthorName;
                }

                // Get book discount information
                var bookDiscountDetail = db.BookDiscountDetails.FirstOrDefault(bdd => bdd.BookId == id);
                var bookDiscountName = "";
                decimal bookDiscountAmount = 0m;
                if (bookDiscountDetail != null)
                {
                    var bookDiscount = db.BookDiscounts.FirstOrDefault(bd => bd.BookDiscountId == bookDiscountDetail.BookDiscountId);
                    if (bookDiscount != null)
                    {
                        bookDiscountName = bookDiscount.BookDiscountName;
                        bookDiscountAmount = bookDiscount.BookDiscountAmount;
                    }
                }

                // Create view model
                var Model = new BooksCardViewModel
                {
                    BookId = book.BookId,
                    BookTitle = book.BookTitle,
                    UnitPrice = book.UnitPrice,
                    PublicationDate = book.PublicationDate,
                    AuthorId = authorDetail?.AuthorId ?? 0,
                    AuthorName = authorName,
                    BookDiscountName = bookDiscountName,
                    BookDiscountAmount = bookDiscountAmount
                };
                return View(Model);
        }

    }
}
