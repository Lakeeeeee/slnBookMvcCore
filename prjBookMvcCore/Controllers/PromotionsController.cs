using Microsoft.AspNetCore.Mvc;
using prjBookMvcCore.Models;

namespace prjBookMvcCore.Controllers
{
    public class PromotionsController : Controller
    {
        BookShopContext db = new();
        public IActionResult Promotions註冊會員禮() { return View(); }

        public IActionResult Promotions會員() { return View(); }

        public IActionResult Promotions促銷活動() { return View(); }

        public IActionResult Promotions活動總覽圖()
        {
            var datas = db.Articals.OrderByDescending(a => a.ArticalId).Select(a => a);
            return View(datas);
        }

        public IActionResult Promotions活動總覽表()
        {
            var datas = db.Articals.OrderByDescending(a => a.ArticalId).Select(a => a);
            return View(datas);
         }

        public IActionResult Promotions活動文章Detail(int? id)
        {
            if (id == null) { return RedirectToAction("Promotions活動已結束"); }
            ViewBag.ArticalId = id;
            return View();
        }

        public IActionResult Promotions活動已結束() { return View(); }

        public IActionResult Promotions限時登入領取優惠() {  return View(); }

        public IActionResult Index() { return View(); } //建置中
    }
}
