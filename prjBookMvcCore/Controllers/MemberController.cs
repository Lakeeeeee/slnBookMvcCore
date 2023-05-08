using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using Microsoft.EntityFrameworkCore;
using prjBookMvcCore.Models;
using prjBookMvcCore.ViewModel;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Net;
using GoogleReCaptcha.V3.Interface;
using System.Text.Encodings.Web;
using Newtonsoft.Json.Linq;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using System.Net.Mail;

namespace prjBookMvcCore.Controllers
{
    public class MemberController : Controller
    {
        private readonly BookShopContext _bookShopContext ;
        private readonly ICaptchaValidator _captchaValidator ;
        public UserInforService _userInforService { get; set; }
        public MemberController(BookShopContext db, UserInforService userInforService,ICaptchaValidator captchaValidator)
        {
            _bookShopContext = db;
            _userInforService = userInforService;
            _captchaValidator = captchaValidator;
        }
        MemberManeger _cm = new MemberManeger();

        public IActionResult Signin()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(NewMemberViewModel member) //註冊方法
        {
            if(_bookShopContext.Members.Any(x=>x.MemberEmail==member.MemberEmail_P))
            {
                return Content("exist");
            }
            else
            {
                Member newmember = new Member()
                {
                    MemberEmail = member.MemberEmail_P,
                    MemberPassword = member.MemberPassword_P,
                    MemberName = member.MemberName_P,
                    MemberBrithDate = member.MemberBrithDate_P,
                    Memberphone = member.Memberphone_P,
                    MemberAddress = member.MemberAddress_P
                };
                _bookShopContext.Add(newmember);
                _bookShopContext.SaveChanges();
                if (member.isSubscribe)
                {
                    MessageSubscribe subscribe = new MessageSubscribe()
                    {
                        MemberId = newmember.MemberId,
                        MessageTypeId = 1,
                    };
                    _bookShopContext.MessageSubscribes.Add(subscribe);
                }
                _bookShopContext.SaveChanges();
                _cm.writeWelcomeLetter(newmember, _bookShopContext);
                return Content("notexist");
            }
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public IActionResult Login(CLoginViewModel vm)
        {
            Member user = _bookShopContext.Members.Include(x=>x.Level).Include(x=>x.Orders).Include(x=>x.MessageMemberDetails).FirstOrDefault(x=>x.MemberEmail==vm.Account_P)!;
            if (user != null)
            {
                if (user.MemberPassword == vm.Password_P)
                {
                    var useClain = new List<Claim>
                    {
                        new Claim("Id", user.MemberId.ToString()),
                        new Claim(ClaimTypes.Name, user.MemberName),
                    };

                    //建構cookie用戶驗證物件的狀態存取
                    var varClainsIdentity = new ClaimsIdentity(useClain, CookieAuthenticationDefaults.AuthenticationScheme);
                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(varClainsIdentity));
                    return Redirect("~/Home/Home");
                }
            }
            string script = "<script>alert('登入失敗');window.history.back();</script>";
            return Content(script, "text/html", System.Text.Encoding.UTF8);
        }

        public IActionResult Find_password() //忘記密碼
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Find_password(string target) //填完表單後發post然後寄出email
        {
            bool isEmailExist =_bookShopContext.Members.Any(x => x.MemberEmail == target);

            if (!isEmailExist)
            {
                return Content(isEmailExist.ToString());
            }
            else
            {
                MailMessage mail = new MailMessage();


            }
            
            return RedirectToAction("Login");
        }

        public IActionResult reset_Password() //忘記密碼的重設密碼頁面
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult reset_PasswordMethod(int? id) //忘記密碼的重設密碼方法
        {
            return RedirectToAction("Login");
        }

        //==========================================以下會員才能訪問
        [Authorize]
        [HttpGet]
        public IActionResult LogOut()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("~/Home/Home");
        }


        [Authorize]
        public IActionResult alretPassword()  //會員專區的重設密碼頁面
        {
            return View();
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult alretPasswordMethod() //todo  //會員專區的重設密碼方法
        {
            return RedirectToAction("Login");
        }
        [Authorize]
        public IActionResult MemberCenter()
        {
            return View();
        }
        [Authorize]
        public IActionResult gerMemberInfor(int id)
        {
            var q = from user in _bookShopContext.Members
                    where user.MemberId == id
                    select new
                    {
                        user_leverl = user.Level.LevelName,
                        user_msCount = user.MessageMemberDetails.Count(x=>x.ReadStatu==0),
                        user_odCount = user.Orders.Count,
                        user_points = user.Points,
                        user_coupons = user.OrderDiscountDetails.Count,
                    };
            return Json(q.FirstOrDefault());
        }


        [Authorize]
        public IActionResult myMessage() //通知訊息
        {
            IEnumerable<MessageMemberDetail> q =  _bookShopContext.MessageMemberDetails.Where(x=>x.MemberId== _userInforService.UserId).Include(x=>x.Message);
            return View(q);
        }

        [Authorize]
        public IActionResult getMessage(int Inputid) //訊息細節
        {
            MessageMemberDetail target = _bookShopContext.MessageMemberDetails.Find(Inputid)!;
            target.ReadStatu = 1; _bookShopContext.SaveChanges();
            var q = from x in _bookShopContext.MessageMemberDetails
                    join y in _bookShopContext.Messages on x.MessageId equals y.MessageId
                    join z in _bookShopContext.MessageTypes on y.MessageTypeId equals z.MessageTypeId
                    where x.MessageMemberDetailId == Inputid
                    select new
                    {
                         time = x.UpdateTime,
                         read_a = (x.ReadStatu==1)?"已讀":"未讀",
                         content_a = y.MessageContent,
                         type_a = z.MessageTypeName
                    };

            return Json(q.FirstOrDefault());
        }
        [Authorize]
        public IActionResult myCoupons() //coupons
        {
            IEnumerable<OrderDiscountDetail> q = _bookShopContext.OrderDiscountDetails.Where(x => x.MemberId == _userInforService.UserId).Include(x=>x.OrderDiscount);
            return View(q);
        }

        [Authorize]
        public IActionResult myPublisher() //關注的出版社
        {
            IEnumerable<CollectedPublisher> q = _bookShopContext.CollectedPublishers.Where(x => x.MemberId == _userInforService.UserId).Include(x=>x.Publisher);
            return View(q);
        }
        [Authorize]
        public IActionResult myAuthor() //關注的作者
        {
            IEnumerable<CollectedAuthor> q = _bookShopContext.CollectedAuthors.Where(x => x.MemberId == _userInforService.UserId).Include(x=>x.Author);
            return View(q);
        }
        [Authorize]
        public IActionResult myNotice() //可購買時通知我 空的
        {
            return View();
        }
        [Authorize]
        public IActionResult myCollect() //暫存清單
        {
            
            IEnumerable<Book> q = _bookShopContext.ActionDetials.Where(x => x.MemberId == _userInforService.UserId && x.ActionId == 2).
             Include(x => x.Book.Publisher).Select(x => x.Book);

            return View(q);
        }
        [Authorize]
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
        [Authorize]
        public IActionResult myOrders()  //訂單查詢
        {
            var q = _bookShopContext.Orders.Where(x => x.MemberId == _userInforService.UserId).
                Include(x => x.Payment).
                Include(x => x.Shipment).
                Include(x => x.PayStatus).
                Include(x => x.ShippingStatus).ToList();
            return View(q);
        }
        [Authorize]
        public IActionResult alretProflie()
        {
            Member member = _bookShopContext.Members.Where(x => x.MemberId == _userInforService.UserId).
                Include(x=>x.Level).
                Include(x=>x.Payment).
                FirstOrDefault()!;
            return View(member);
        } //
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult alretProflie(Member member)
        {
            Member memberupdate = _bookShopContext.Members.FirstOrDefault(x => x.MemberId == member.MemberId)!;
            if (memberupdate != null)
            {
                memberupdate.MemberName = member.MemberName;
                memberupdate.MemberEmail = member.MemberEmail;
                memberupdate.MemberBrithDate = member.MemberBrithDate;
                memberupdate.MemberAddress = member.MemberAddress;
                memberupdate.PaymentId = member.PaymentId;
                _bookShopContext.SaveChanges();
            };
            return RedirectToAction("alretProflie");
        }
    }
}
