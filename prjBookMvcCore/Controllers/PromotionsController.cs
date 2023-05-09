using Microsoft.AspNetCore.Mvc;

namespace prjBookMvcCore.Controllers
{
    public class PromotionsController : Controller
    {

        public IActionResult Promotions註冊會員禮()
        {
            return View();
        }

        public IActionResult Promotions會員()
        {
            return View();
        }

        public IActionResult Promotions促銷活動()
        {
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
