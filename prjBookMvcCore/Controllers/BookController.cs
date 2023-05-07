using Microsoft.AspNetCore.Mvc;
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
    }
}
