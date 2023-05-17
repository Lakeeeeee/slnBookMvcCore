using GoogleReCaptcha.V3.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using prjBookMvcCore.Models;
using System.Diagnostics.Metrics;

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

        private readonly BookShopContext _bookShopContext;
        private readonly IConfiguration _config;
        private readonly ICaptchaValidator _captchaValidator; //un done
        public UserInforService _userInforService { get; set; }
        public PromotionsController(BookShopContext db, UserInforService userInforService, IConfiguration config, ICaptchaValidator captchaValidator)
        {
            _bookShopContext = db;
            _userInforService = userInforService;
            _captchaValidator = captchaValidator;
            _config = config;
        }
        [HttpPost]
        [Authorize]
        public IActionResult Promotions限時登入領取優惠()
        {
            if (_bookShopContext.OrderDiscountDetails.Where(d=>d.MemberId==_userInforService.UserId).Any(d=>d.OrderDiscountId==4))
            {
                return Content("exist");
            }
            else
            {
                OrderDiscountDetail newmemberdiscount = new OrderDiscountDetail()
                {
                    OrderDiscountId = 4,
                    MemberId = _userInforService.UserId,
                    OrderDiscountStartDate = DateTime.Now,
                    OrderDiscountEndDate = DateTime.Now.AddDays(3),
                };
                _bookShopContext.OrderDiscountDetails.Add(newmemberdiscount);
                _bookShopContext.SaveChanges();
                return Content("notexist");
            }
        }

        public IActionResult Index() { return View(); } //建置中
    }
}
