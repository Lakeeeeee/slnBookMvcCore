using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prjBookMvcCore.Models;

namespace prjBookMvcCore.Controllers
{
    public class BookController : Controller
    {
        BookShopContext db = new();

        public IActionResult BookInformation()
        {
            Book test = db.Books.Include(x => x.Language).Include(x => x.Publisher).FirstOrDefault(x => x.BookId == 123);
            return View(test);
        }
    }
}
