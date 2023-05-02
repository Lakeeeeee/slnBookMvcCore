using Microsoft.AspNetCore.Mvc;

namespace prjBookMvcCore.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult ListCart()
        {
            return View();
        }
        public IActionResult ShoppingCart()
        {
            return View();
        }

        public IActionResult checkOutInfo()
        {
            return View();
        }

        public IActionResult checkOutConfirm()
        {
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
