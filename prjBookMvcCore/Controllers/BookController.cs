using Microsoft.AspNetCore.Mvc;

namespace prjBookMvcCore.Controllers
{
    public class BookController : Controller
    {
        public IActionResult BookInformation()
        {
            return View();
        }
    }
}
