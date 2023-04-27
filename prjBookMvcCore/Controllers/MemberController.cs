﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using Microsoft.EntityFrameworkCore;
using prjBookMvcCore.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace prjBookMvcCore.Controllers
{
    public class MemberController : Controller
    {
        BookShopContext db = new BookShopContext();
        
        int testMemID = 66; //預設會員ID

        public IActionResult MemberCenter()
        {
            return View();
        }
        public IActionResult Find_password()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Find_password(int? id)
        {
            
            
            return RedirectToAction("Login");
        }

        public IActionResult reset_Password()
        {
            return View();
        }
        [HttpPost]
        public IActionResult reset_Password(int? id)
        {
            return View();
        }


        public IActionResult Signin()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create()
        {
            return RedirectToAction("Login");
        }


        #region(登入--haven't finish)

        public IActionResult Login()
        {
            return View();
        }

        //[HttpPost]
        //public IActionResult Login(CLoginViewModel vm)
        //{
        //    CCustomer customer = (new CCustomerFactory()).queryByEmail(vm.txtAccount);
        //    if (customer != null)
        //    {
        //        if (customer.fPassword == vm.txtPassword)
        //        {
        //            return RedirectToAction("Home");
        //        }
        //    }
        //    return View();
        //}
        #endregion



        public IActionResult myMessage() //通知訊息
        {
            IEnumerable<CustomerService> q = db.CustomerServices.Where(x => x.MemberId == testMemID);
            return View(q);
        }

        public IActionResult myPublisher() //關注的出版社
        {
            IEnumerable<CollectedPublisher> q = db.CollectedPublishers.Where(x => x.MemberId == testMemID).Include(x=>x.Publisher);
            return View(q);
        }


        public IActionResult myAuthor() //關注的作者
        {
            IEnumerable<CollectedAuthor> q = db.CollectedAuthors.Where(x => x.MemberId == testMemID).Include(x=>x.Author);
            return View(q);
        }

        public IActionResult myNotice() //可購買時通知我 空的
        {
            return View();
        }
        public IActionResult myCollect() //暫存清單
        {

            IEnumerable<Book> q = db.ActionDetials.Where(x => x.MemberId == testMemID && x.ActionId == 2).
             Include(x => x.Book.Publisher).Select(x => x.Book);

            return View(q);
        }

        #region(訂單修改/取消, todo)

        //public IActionResult editOrders(Order id)
        //{
        //    if (id != null)
        //    {
        //        Order order = db.Orders.FirstOrDefault(x=>x.OrderId==id);
        //        if(order != null)
        //        {


        //        }
        //    }
        //    return RedirectToAction("myOrders");
        //}

        //[HttpPost]
        //public ActionResult editOrders()
        //{
        //    var q = db.Orders.Where(x => x.Member.MemberID == testMemID).ToList();
        //    return View(q);
        //}

        #endregion //to do

        public IActionResult myOrders()  //訂單查詢
        {
            var q = db.Orders.Where(x => x.MemberId == testMemID).
                Include(x=>x.Discount).
                Include(x=>x.Payment).
                Include(x=>x.Shipment).
                Include(x=>x.PayStatus).
                Include(x=>x.ShippingStatus).ToList();
            return View(q);
        }

        public IActionResult alretProflie()
        {
            return View();
        }

        [HttpPost]
        public IActionResult resetPassword() //todo
        {
            return RedirectToAction("Login");
        }

        public IActionResult alretPassword()
        {
            return View();
        }

    }
}
