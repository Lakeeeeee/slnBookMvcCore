using GoogleReCaptcha.V3.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prjBookMvcCore.Models;
using System.Diagnostics.Metrics;

namespace prjBookMvcCore.Controllers
{
    public class PromotionsController : Controller
    {
        BookShopContext db = new();
        public UserInforService _userInforService { get; set; }
        public PromotionsController( UserInforService userInforService)
        {
            _userInforService = userInforService;
        }
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


       
        public string Promotions限時登入領取優惠()
        {
            string isSuccess;
            var q = db.OrderDiscountDetails.Where(d =>d.MemberId== _userInforService.UserId & d.OrderDiscountId==4).Select(d => d);
            if (q.Count() != 0)
            {
                isSuccess = "false";
                return isSuccess;
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
                db.OrderDiscountDetails.Add(newmemberdiscount);
                db.SaveChanges();
                isSuccess = "true";
                return isSuccess;
            }
        }

        public IActionResult Index() { return View(); } //建置中
    }
}
