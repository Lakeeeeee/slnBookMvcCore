﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using prjBookMvcCore.Models;
using prjBookMvcCore.ViewModel;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Diagnostics;

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
        int y = 0;
        public IActionResult Action2(IFormCollection form)
        {
            bool isSuccess = false;
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

            var oD = int.Parse(form["oD"]);

            Order order = new Order()
            {
                MemberId = Convert.ToInt32(MemberId),
                OrderDate = DateTime.Now,
                ShipmentId = Convert.ToInt32(ShipmentId),
                PaymentId = Convert.ToInt32(PaymentId),
                OrderDiscountId = oD,
                PointAmount = RebateAmount,
                ReciverName = ReciverName,
                ReciverPhone = ReciverPhone,
                ShipAddr = ShipAddr,
            };

            _db.Orders.Add(order);
            _db.SaveChanges();

            var q = _db.OrderDiscountDetails.Include(x => x.OrderDiscount).Where(x => x.OrderDiscountId == oD && x.MemberId == _user.UserId).FirstOrDefault();
            if (q.OrderDiscount.DiscountTypeId == 2)
            {
                _db.Remove(q);
                _db.SaveChanges();
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
            y = order.OrderId;
            isSuccess = true;
            return Content(isSuccess.ToString());
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
                OrderDetail orderDetail = new OrderDetail();
                Shipment shipment = new Shipment(); 

                orderDetail.BookId = bookid;
                orderDetail.OrderId = order.OrderId;
                decimal freight = shipment.Freight;

                list.Add(orderDetail);
            };

            for (int n = 0; n < list.Count(); n++)
            {
                list[n].Quantity = quantity[n];
            };
            _db.OrderDetails.AddRange(list);
            _db.SaveChanges();
            isSuccess = true;
            return Content(isSuccess.ToString());
        }

        public void creatOrderDetail(int orderId, int bookId, int quantity, decimal unitprice ,BookShopContext db)
        {
            OrderDetail detail = new OrderDetail()
            {
                OrderId = orderId,
                BookId = bookId,
                Quantity = quantity,
                UnitPrice = unitprice
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

