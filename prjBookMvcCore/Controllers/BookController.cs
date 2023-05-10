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

            // Find the category and subcategory information
            var category = db.Categories.Where(c => c.CategoryId == id).FirstOrDefault();
            if (category == null)
                return RedirectToAction("Home");
            var subcategories = db.SubCategories.Where(sc => sc.CategoryId == id)
                                                .Select(sc => new { sc.SubCategoryId, sc.SubCategoryName })
                                                .ToList();

            // Find the book information
            var books = db.CategoryDetails.Where(cd => cd.SubCategoryId == subcategories.Select(s => s.SubCategoryId).FirstOrDefault())
                                           .Select(cd => new { cd.BookId })
                                           .Distinct()
                                           .Join(db.Books, cd => cd.BookId, b => b.BookId, (cd, b) => new
                                           {
                                               b.BookId,
                                               b.BookTitle,
                                               b.UnitPrice,
                                               b.PublicationDate,
                                               b.CoverPath
                                           });

            // Find the author information
            var author = db.Authors.Where(a => a.AuthorId == id).FirstOrDefault();
            if (author == null)
                return RedirectToAction("Home");
            var authorBooks = db.AuthorDetails.Where(ad => ad.AuthorId == id)
                                               .Select(ad => new { ad.BookId })
                                               .Join(db.Books, ad => ad.BookId, b => b.BookId, (ad, b) => new
                                               {
                                                   b.BookId,
                                                   b.BookTitle,
                                                   b.UnitPrice,
                                                   b.PublicationDate,
                                                   b.CoverPath
                                               });

            // Find the book discount information
            var bookDiscount = db.BookDiscounts.Where(bd => bd.BookDiscountId == id).FirstOrDefault();
            if (bookDiscount == null)
                return RedirectToAction("Home");
            var bookDiscountBooks = db.BookDiscountDetails.Where(bdd => bdd.BookDiscountId == id)
                                                           .Select(bdd => new { bdd.BookId })
                                                           .Join(db.Books, bdd => bdd.BookId, b => b.BookId, (bdd, b) => new
                                                           {
                                                               b.BookId,
                                                               b.BookTitle,
                                                               b.UnitPrice,
                                                               b.PublicationDate,
                                                               b.CoverPath
                                                           });

            // Find the painter information
            var painter = db.Painters.Where(p => p.PainterId == id).FirstOrDefault();
            if (painter == null)
                return RedirectToAction("Home");
            var painterBooks = db.PainterDetails.Where(pd => pd.PainterId == id)
                                                 .Select(pd => new { pd.BookId })
                                                 .Join(db.Books, pd => pd.BookId, b => b.BookId, (pd, b) => new
                                                 {
                                                     b.BookId,
                                                     b.BookTitle,
                                                     b.UnitPrice,
                                                     b.PublicationDate,
                                                     b.CoverPath
                                                 });

            // Find the translator information
            var translator = db.Translators.Where(t => t.TranslatorId == id).FirstOrDefault();
            if (translator == null)
                return RedirectToAction("Home");
            var translatorBooks = db.TranslatorDetails.Where(td => td.TranslatorId == id)
                                                       .Select(td => new { td.BookId })
                                                       .Join(db.Books, td => td.BookId, b => b.BookId, (td, b) => new
                                                       {
                                                           b.BookId,
                                                           b.BookTitle,
                                                           b.UnitPrice,
                                                           b.PublicationDate,
                                                           b.CoverPath
                                                       });
            // Find the article information
            var artical = db.Articals.Where(a => a.ArticalId == id)
                                     .Select(a => new { a.ArticalId, a.ArticalTitle, a.ArticalDescription, a.ArticalPicture })
                                     .FirstOrDefault();
            if (artical == null)
                return RedirectToAction("Home");

            // Find the book information for the article
            var bookDetail = db.ArticalToBookDetails.FirstOrDefault(ad => ad.ArticalId == id);
            if (bookDetail == null)
                return RedirectToAction("Home");
            var book = db.Books.FirstOrDefault(b => b.BookId == bookDetail.BookId);
            if (book == null)
                return RedirectToAction("Home");

            // Create view model
            var model = new BooksCardViewModel
            {
                BookId = book.BookId,
                BookTitle = book.BookTitle,
                UnitPrice = book.UnitPrice,
                CoverPath=book.CoverPath,
                PublicationDate = book.PublicationDate,
                AuthorId = author.AuthorId,
                AuthorName = author.AuthorName,
                BookDiscountName = bookDiscount.BookDiscountName,
                BookDiscountAmount = bookDiscount.BookDiscountAmount,
                ArticalID = artical.ArticalId,
                ArticalTitle = artical.ArticalTitle,
                ArticalDescription = artical.ArticalDescription,
                ArticalPicture = artical.ArticalPicture,
                CategoryId = category.CategoryId,
                CategoryName = category.CategoryName,
                SubCategoryId = subcategories.Select(s => s.SubCategoryId).FirstOrDefault(),
                SubCategoryName = subcategories.Select(s => s.SubCategoryName).FirstOrDefault()
            };
            return View(model);
        }

    }
}
