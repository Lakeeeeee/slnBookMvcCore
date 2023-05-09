using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prjBookMvcCore.Models;
using static prjBookMvcCore.Models.BooksCardModel;

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
            if (id == null)
                return RedirectToAction("Home");

            var db = new BooksCardModel();

            var datas = db.Books
                .Where(book => book.BookId == id)
                .Include(book => book.Authors)
                .Include(book => book.BookDiscounts)
                .Select(book => new
                {
                    book.BookId,
                    book.BookTitle,
                    book.UnitPrice,
                    book.PublicationDate,
                    Author = book.Authors.Select(author => author.AuthorName).ToList(),
                    BookDiscountName = book.BookDiscounts.Select(discount => discount.DiscountName).FirstOrDefault()
                })
                .FirstOrDefault();

            return View(datas);
        }
    }
}
