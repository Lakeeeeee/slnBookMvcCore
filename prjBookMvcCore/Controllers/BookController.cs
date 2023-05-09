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
            return View();
        }

        public IActionResult _BookCardPartial(int? id)
        {
            if (id == null)
                return RedirectToAction("Home");
            IQueryable datas = from s in db.Books.Include(s => s.BookId)
                               where s.BookId == id
                               select s;
            return View(datas);
        }
    }
}
