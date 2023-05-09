using Microsoft.AspNetCore.Mvc;
using prjBookMvcCore.Models;

namespace prjBookMvcCore.Controllers
{
    public class BookController : Controller
    {
        BookShopContext db = new();

        public IActionResult BookInformation()
        {
            Book test = db.Books.FirstOrDefault(x => x.BookId == 10);
            return View(test);
        }
    }
}
