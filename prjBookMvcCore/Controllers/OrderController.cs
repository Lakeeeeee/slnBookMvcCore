using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using prjBookMvcCore.Models;
using prjBookMvcCore.ViewModel;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace prjBookMvcCore.Controllers
{
    public class OrderController : Controller
    {
        public UserInforService _user { get; set; }
        BookShopContext _db = new BookShopContext();

        public OrderController(UserInforService user, BookShopContext db)
        {
            _user = user;
            _db = db;
        }
        [Authorize]
        public IActionResult ShoppingCart() //開啟頁面的方法
        {
            List<ShoppingcartInformation> cartItems = new List<ShoppingcartInformation>();

            IEnumerable<ActionDetial> q = _db.ActionDetials.Where(x => x.MemberId == _user.UserId && x.ActionId == 7);

            foreach (var item in q)
            {
                ShoppingcartInformation shoppingcartInformation = new ShoppingcartInformation()
                {
                    ActionDetial = item,
                    Quantity = 1
                };
                cartItems.Add(shoppingcartInformation);
            }
            return View(cartItems);

        }

        public IActionResult Action1() { return View(); }
        public IActionResult Action2() { return View(); }
        public IActionResult Action3() { return View(); }

        public IActionResult itemDelete(int id) //刪除購物車項目
        {
            bool isSuccesse = false;
            var tool = _db.ActionDetials.Find(id);
            if (tool != null)
            {
                _db.ActionDetials.Remove(tool);
                _db.SaveChanges();
                isSuccesse = true;
            };
            return View(isSuccesse.ToString());
        } 

        [Authorize]
        public IActionResult searchDiscount(int total)  //page2抓會員跟酷碰類型方法
        {
            List<DiscountType> discountTypes = new List<DiscountType>();
            Member x = _db.Members.Where(x=>x.MemberId==_user.UserId).FirstOrDefault();
            if (total > 1000)
            {

                DiscountType memberDiscountType = _db.DiscountTypes.Where(x=>x.DiscountTypeId == 1).FirstOrDefault();                //回傳會員優惠
                discountTypes.Add(memberDiscountType);
            };

            var q = _db.OrderDiscountDetails.Where(x=>x.MemberId==_user.UserId && x.IsOrderDiscountUse=="N").ToList();

            if (q.Count() > 0)
            {

                DiscountType couponDiscountType = _db.DiscountTypes.Where(x => x.DiscountTypeId == 2).FirstOrDefault(); 
                discountTypes.Add(couponDiscountType);
            };

            return Json(discountTypes);
        }

        //----------------------------------------------------------------
        public IActionResult checkOutConfirm()
        {
            return View();
        }
        [HttpPost]
        public IActionResult checkOutConfirm(OrderViewModel model)
        {

            return View(model);
        }
        public IActionResult checkOutFinal()
        {
            return View();
        }
    }
}

