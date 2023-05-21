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

        public IActionResult Action1(IFormCollection form)
        {
            int[] acids = form["acid"].Select(x => int.Parse(x)).ToArray();
            int[] quantity = form["quantity"].Select(x => int.Parse(x)).ToArray();
            List<ShoppingcartInformation> newCart = new List<ShoppingcartInformation>();
            foreach (var item in acids)
            {
                ActionDetial tool = _db.ActionDetials.Find(item);
                ShoppingcartInformation x = new ShoppingcartInformation() { ActionDetial = tool };
                newCart.Add(x);
            } ;

            for(int n= 0; n<newCart.Count(); n++)
            {
                newCart[n].Quantity= quantity[n];
            };

            return Json(newCart);
        }
        public IActionResult Action2() { return View(); }
        public IActionResult Action3(IFormCollection formData) //createOrderdetails
        {
            //var orderData = formData["orderData"];
            //foreach (var data in orderData)
            //{
            //}


            return Content("Return Successe");
        }
        public IActionResult creatOrder(IFormCollection formData)  //towrite
        {
            bool isSuccess = false;

            //var value1 = formData["name1"];  //抓formForOrder裡的name+value
            //var value2 = formData["name2"];  //每個input的name代表欄位, 抓出其value再塞到新order的參數
            //var value3 = formData["name3"];  //依此類推, 有幾個欄位就抓幾個value

            //Order order = new Order(){};
            //_db.Orders.Add(order);
            // _db.SaveChanges();
            isSuccess = true;

            return Content(isSuccess.ToString());
        }

        public void creatOrderDetail(int orderId, int bookId, int quantity, BookShopContext db)
        {
            OrderDetail detail = new OrderDetail()
            {
                OrderId = orderId,
                BookId = bookId,
                Quantity = quantity
            };

            int q = db.BookDiscountDetails.Where(x => x.BookId == detail.BookId).Select(x => x.BookDiscountId).FirstOrDefault();
            decimal price = db.Books.Find(detail.BookId).UnitPrice;
            decimal d = db.BookDiscounts.Find(q).BookDiscountAmount;
            detail.UnitPrice = price * d;
            db.OrderDetails.Add(detail);
        }

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
            return Content(isSuccesse.ToString());
        }

        [Authorize]
        public IActionResult searchDiscount(int total)  //page2抓會員跟酷碰方法
        {
            List<OrderDiscount> discounts = new List<OrderDiscount>();
            if (total > 1000)
            {
                int index = (int)_db.MemberLevels.Where(x=>x.LevelId==_user.UserLevelId).Select(x=>x.OrderDiscountId).FirstOrDefault();
                OrderDiscount memberDiscount = _db.OrderDiscounts.Find(index);
                discounts.Add(memberDiscount);
            };

            var coupons = _db.OrderDiscountDetails.Where(x=>x.MemberId==_user.UserId && x.IsOrderDiscountUse=="N").Select(x=>x.OrderDiscount);

            discounts.AddRange(coupons);
            return Json(discounts);
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

