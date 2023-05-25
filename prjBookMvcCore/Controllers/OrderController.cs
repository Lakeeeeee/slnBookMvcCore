using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using prjBookMvcCore.Models;
using prjBookMvcCore.ViewModel;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Diagnostics;
using System.Net;

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
            };

            for (int n = 0; n < newCart.Count(); n++)
            {
                newCart[n].Quantity = quantity[n];
            };

            return Json(newCart);
        }
        int y = 0;
        public IActionResult Action2(IFormCollection form)
        {
            int theorderID = 0;
            var MemberId = form["memberId"];
            var ReciverName = form["reciverName"];
            var ReciverPhone = form["reciverPhone"];
            var ShipAddr = form["shipAddr"];
            var PaymentId = int.Parse(form["paymentId"]);
            var ShipmentId = int.Parse(form["shipmentId"]);

            string rebateAmountInput = form["rebateAmountInput"];
            int RebateAmount;
            if (string.IsNullOrEmpty(rebateAmountInput))
            {
                RebateAmount = 0;
            }
            else
            {
                RebateAmount = int.Parse(rebateAmountInput);
            };


            Order order = new Order()
            {
                MemberId = Convert.ToInt32(MemberId),
                OrderDate = DateTime.Now,
                ShipmentId = Convert.ToInt32(ShipmentId),
                PaymentId = Convert.ToInt32(PaymentId),
                PointAmount = RebateAmount,
                ReciverName = ReciverName,
                ReciverPhone = ReciverPhone,
                ShipAddr = ShipAddr,
            };

            _db.Orders.Add(order);
            _db.SaveChanges();

            string oDValue = form["oD"];
            int oD;
            if (int.TryParse(oDValue, out oD))
            {
                order.OrderDiscountId = oD;
                var q = _db.OrderDiscountDetails.Include(x => x.OrderDiscount).Where(x => x.OrderDiscountId == oD && x.MemberId == _user.UserId).FirstOrDefault();
                if (q != null)
                {
                    if (q.OrderDiscount.DiscountTypeId == 2)
                    {
                        _db.Remove(q);
                        _db.SaveChanges();
                    };
                }
            }
            else
            {
                // 解析失敗，oDValue 為 null 或無效的整數表示
                // 在此處理相應的邏輯
                order.OrderDiscountId = 7;
            }

            Member member = _db.Members.Find(order.MemberId);

            switch (member.LevelId)
            {
                case 3:
                    member.Points = (int)((member.Points) + Convert.ToInt32(order.TotalPay) * 0.01);
                    break;
                case 4:
                    member.Points = (int)((member.Points) + Convert.ToInt32(order.TotalPay) * 0.05);
                    break;
                case 5:
                    member.Points = (int)((member.Points) + Convert.ToInt32(order.TotalPay) * 0.05);
                    break;
            };
            _db.SaveChanges();
            theorderID = order.OrderId;
            return Content(theorderID.ToString());
        }
        public IActionResult Action3(IFormCollection formData) //createOrderdetails
        {
            bool isSuccess = false;
            Order order = _db.Orders.OrderByDescending(o => o.OrderId).FirstOrDefault();
            int[] acids = formData["acid"].Select(x => int.Parse(x)).ToArray();
            int[] quantity = formData["quantity"].Select(x => int.Parse(x)).ToArray();

            List<OrderDetail> list = new List<OrderDetail>();
            foreach (var item in acids)
            {
                int bookid = _db.ActionDetials.Where(x => x.ActionToBookId == item).Select(x => x.BookId).FirstOrDefault();
                decimal 折扣 = _db.BookDiscountDetails.Include(x => x.BookDiscount).Where(x => x.BookId == bookid).Select(x => x.BookDiscount).Select(x => x.BookDiscountAmount).FirstOrDefault();
                decimal bookPrice = _db.Books.Find(bookid).UnitPrice;
                OrderDetail orderDetail = new OrderDetail();
                orderDetail.BookId = bookid;
                orderDetail.OrderId = order.OrderId;
                orderDetail.UnitPrice = bookPrice * 折扣;
                list.Add(orderDetail);
            };

            for (int n = 0; n < list.Count(); n++)
            {
                list[n].Quantity = quantity[n];
                var thebook = _db.Books.Where(x => x.BookId == list[n].BookId).FirstOrDefault();
                thebook.UnitInStock = thebook.UnitInStock - quantity[n];
            };
            _db.OrderDetails.AddRange(list);
            _db.SaveChanges();
            isSuccess = true;

            //刪除購物車
            foreach (int item in acids)
            {
                ActionDetial tool = _db.ActionDetials.Find(item);
                _db.ActionDetials.Remove(tool);
            }
            _db.SaveChanges();

            return Content(isSuccess.ToString());
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
                int index = (int)_db.MemberLevels.Where(x => x.LevelId == _user.UserLevelId).Select(x => x.OrderDiscountId).FirstOrDefault();
                OrderDiscount memberDiscount = _db.OrderDiscounts.Find(index);
                discounts.Add(memberDiscount);
            };

            var coupons = _db.OrderDiscountDetails.Where(x => x.MemberId == _user.UserId & x.OrderDiscountStartDate < DateTime.Now & x.OrderDiscountEndDate > DateTime.Now & x.IsOrderDiscountUse == "N").Select(x => x.OrderDiscount);

            //庫碰判斷滿額
            foreach (var coupon in coupons)
            {
                if (total > coupon.OrderDiscountCondition)
                {
                    discounts.Add(coupon);
                }
            }
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
        public IActionResult checkOutFinal(int id)
        {
            Order order = _db.Orders.Include(x => x.Member).Include(x => x.OrderDetails).ThenInclude(x => x.Book).Where(x => x.OrderId == id).FirstOrDefault();
            return View(order);
        }
    }
}

