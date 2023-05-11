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
            {
                var subcategory = db.SubCategories.Where(sc => sc.SubCategoryId == id).FirstOrDefault();
                if (subcategory == null)
                    return RedirectToAction("Home");
                else
                    category = db.Categories.Where(c => c.CategoryId == subcategory.CategoryId).FirstOrDefault();
            }

            // Find the subcategory information
            var subcategoryInfo = db.SubCategories.Where(sc => sc.CategoryId == category.CategoryId && sc.SubCategoryId == id)
                                                   .Select(sc => new { sc.SubCategoryId, sc.SubCategoryName, sc.CategoryId })
                                                   .FirstOrDefault();

            // Find the book information
            var bookIds = db.CategoryDetails.Where(cd => cd.SubCategoryId == subcategoryInfo.SubCategoryId)
                                             .Select(cd => cd.BookId)
                                             .Distinct()
                                             .ToList();

            var bookInfo = db.Books.Where(b => bookIds.Contains(b.BookId))
                               .Join(db.AuthorDetails, b => b.BookId, ad => ad.BookId, (b, ad) => new { b, ad.AuthorId })
                               .Join(db.Authors, b_ad => b_ad.AuthorId, a => a.AuthorId, (b_ad, a) => new { b_ad.b, a.AuthorName, b_ad.AuthorId })
                               .Join(db.BookDiscountDetails, ba => ba.b.BookId, bd => bd.BookId, (ba, bd) => new { ba.b, ba.AuthorName, ba.AuthorId, bd.BookDiscountId })
                               .Join(db.BookDiscounts, b_bd => b_bd.BookDiscountId, bd => bd.BookDiscountId, (b_bd, bd) => new { b_bd.b, b_bd.AuthorName, b_bd.AuthorId, bd.BookDiscountName, bd.BookDiscountAmount })
                               .Select(b => new
                               {
                                   b.b.BookId,
                                   b.b.BookTitle,
                                   b.b.UnitPrice,
                                   b.b.PublicationDate,
                                   b.AuthorName,
                                   b.BookDiscountName,
                                   b.BookDiscountAmount,
                                   b.b.CoverPath,
                                   b.AuthorId
                               })
                               .FirstOrDefault();

            if (bookInfo == null)
                return RedirectToAction("Home");

            // Create view model
            var model = new BooksCardViewModel
            {
                BookId = bookInfo.BookId,
                BookTitle = bookInfo.BookTitle,
                UnitPrice = bookInfo.UnitPrice,
                CoverPath = bookInfo.CoverPath,
                PublicationDate = bookInfo.PublicationDate,
                AuthorId = bookInfo.AuthorId,
                AuthorName = bookInfo.AuthorName,
                BookDiscountName = bookInfo.BookDiscountName,
                BookDiscountAmount = bookInfo.BookDiscountAmount,
                CategoryId = category?.CategoryId??0,
                CategoryName = category?.CategoryName,
                SubCategoryId = subcategoryInfo?.SubCategoryId??0,
                SubCategoryName = subcategoryInfo?.SubCategoryName
            };
            return View(model);
        }

    }
}
