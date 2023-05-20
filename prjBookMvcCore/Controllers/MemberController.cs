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
using System.Web;
using System.Text;
using System.Reflection.Metadata;
using Microsoft.CodeAnalysis.Scripting;
using NuGet.Protocol;
using System.Configuration;

namespace prjBookMvcCore.Controllers
{
    public class MemberController : Controller
    {
        private readonly BookShopContext _bookShopContext;
        private readonly IConfiguration _config;
        private readonly ICaptchaValidator _captchaValidator ; //un done
        public UserInforService _userInforService { get; set; }
        public MemberController(BookShopContext db, UserInforService userInforService, IConfiguration config,ICaptchaValidator captchaValidator)
        {
            _bookShopContext = db;
            _userInforService = userInforService;
            _captchaValidator = captchaValidator;
            _config = config;
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
                OrderDiscountDetail newmemberdiscount = new OrderDiscountDetail()
                {
                    OrderDiscountId = 6,
                    MemberId = newmember.MemberId,
                    OrderDiscountStartDate = DateTime.Now,
                    OrderDiscountEndDate = DateTime.Now.AddMonths(1),
                };
                _bookShopContext.OrderDiscountDetails.Add(newmemberdiscount);
                _bookShopContext.SaveChanges();
                if (member.isSubscribe)
                {
                    MessageSubscribe subscribe = new MessageSubscribe()
                    {
                        MessageTypeId = 1,
                        MemberId = newmember.MemberId
                    };
                    _bookShopContext.MessageSubscribes.Add(subscribe);
                }
                _bookShopContext.SaveChanges(); 
                _cm.write註冊會員禮Letter(newmember, _bookShopContext);
                _cm.writeWelcomeLetter(newmember, _bookShopContext);
               
                return Content("notexist");
            }
        }

        public IActionResult Login() //登入頁面
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(CLoginViewModel vm) //登入方法
        {
            Member user = _bookShopContext.Members.Include(x=>x.Level).Include(x=>x.Orders).Include(x=>x.MessageMemberDetails).FirstOrDefault(x=>x.MemberEmail==vm.Account_P)!;
            if (user != null)
            {
                if (user.MemberPassword == vm.Password_P)
                {
                    var useClain = new List<Claim>
                    {
                        new Claim("Id", user.MemberId.ToString()),
                        new Claim("UserLevelId", user.LevelId.ToString()),
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
        public IActionResult Find_password() //忘記密碼頁面
        {
            return View();
        }
        [Route("Member/find")]
        public IActionResult Find_password(string target) //忘記密碼方法
        {
            bool isEmailExist = _bookShopContext.Members.Any(x => x.MemberEmail == target);
            if (isEmailExist)
            {
                Member member = _bookShopContext.Members.FirstOrDefault(x => x.MemberEmail == target)!;
                string sVerify = member.MemberId + "|" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                sVerify = HttpUtility.UrlEncode(sVerify);
                int portNumber = HttpContext.Connection.LocalPort;
                string webPath = "https://localhost:"+ portNumber + "/";
                string receivePage = "Member/ResetPwd";
                string mailContent = "請點擊以下連結，返回網站重新設定密碼，逾期 5 分鐘後，此連結將會失效。<br><br>";
                mailContent = mailContent + "<a href='" + webPath + receivePage + "?verify=" + sVerify + "'  target='_blank'>點此連結</a>";
                string mailSubject = "[讀本] 重設密碼申請信";
                string SmtpServer = "smtp.gmail.com";
                string GoogleMailUserID = _config["GoogleMailUserID"];
                string GoogleMailUserPwd = _config["GoogleMailUserPwd"];
                int port = 587;
                MailMessage mms = new MailMessage();
                mms.From = new MailAddress(GoogleMailUserID);
                mms.Subject = mailSubject;
                mms.Body = mailContent;
                mms.IsBodyHtml = true;
                mms.SubjectEncoding = Encoding.UTF8;
                mms.To.Add(new MailAddress(target));
                using (SmtpClient client = new SmtpClient(SmtpServer, port))
                {
                    client.EnableSsl = true;
                    client.Credentials = new NetworkCredential(GoogleMailUserID, GoogleMailUserPwd);
                    client.Send(mms);
                }
            }
            return Content(isEmailExist.ToString());
        }

        public IActionResult ResetPwd(string verify)  //重設密碼頁面
        {
            string script = "<script>alert('驗證碼錯誤或逾期失效');window.close();</script>";

            if (verify == "")
            {
                return Content(script, "text/html", System.Text.Encoding.UTF8);
            }
            int UserID = Convert.ToInt32(verify.Split('|')[0]);
            string ResetTime = verify.Split('|')[1];
            DateTime dResetTime = Convert.ToDateTime(ResetTime);
            TimeSpan TS = new TimeSpan(DateTime.Now.Ticks - dResetTime.Ticks);
            double diff = Convert.ToDouble(TS.TotalMinutes);
            if (diff > 5)
            {
                return Content(script, "text/html", System.Text.Encoding.UTF8);
            }
            Member member = _bookShopContext.Members.FirstOrDefault(x => x.MemberId == UserID)!;
            
            return View(member);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult doResetPwd(Member target)  //修改密碼方法
        {
            
            if (_bookShopContext.Members.Any(x => x.MemberId == target.MemberId))
            {
                Member member = _bookShopContext.Members.FirstOrDefault(x => x.MemberId == target.MemberId)!;
                member.MemberPassword = target.MemberPassword;
                _bookShopContext.SaveChanges();
                return Json(new
                {
                    success = "true",
                    message = "密碼重新設定成功"
                });
            }
            else
            {
                return Json(new
                {
                    success = "false",
                    message = "密碼重新設定失敗"
                });
            }
        }

        //==========================================以下會員才能訪問
        [Authorize]
        [HttpGet]
        public IActionResult LogOut() //登出方法
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("~/Home/Home");
        }

        [Authorize]
        public IActionResult alretPassword()  //會員專區的重設密碼頁面
        {
            var member = _bookShopContext.Members.Where(x => x.MemberId == _userInforService.UserId).FirstOrDefault();
            return View(member);
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult alretPasswordMethod(IFormCollection form) //todo  //會員專區的重設密碼方法
        {
            bool isSuccess = false;
            int memId = Convert.ToInt32(form["memId"]);
            string oldPwd = form["oldPwd"].ToString();
            var newPwd = form["newPwd"];

            Member updateTool = _bookShopContext.Members.Find(_userInforService.UserId)!;
            if (memId == _userInforService.UserId && oldPwd == updateTool.MemberPassword)
            {
                updateTool.MemberPassword = newPwd;
                _bookShopContext.SaveChanges();
                isSuccess = true;
                return Json(new
                {
                    success = isSuccess.ToString(),
                    message = "修改密碼成功"
                });
            }
            return Json(new
            {
                success = isSuccess.ToString(),
                message = "更新失敗"
            });

        }
        [Authorize]
        public IActionResult MemberCenter() //會員中心頁面
        {
            var q = _bookShopContext.Members.Where(x => x.MemberId == _userInforService.UserId).Include(x=>x.Level).FirstOrDefault();
            return View(q);
        }
        [Authorize]
        public IActionResult gerMemberInfor(int id) //取得會員狀態方法
        {
            var q = _bookShopContext.Members.Where(x => x.MemberId == id).Select(x => new
            {
                user_msCount = x.MessageMemberDetails.Count(x => x.ReadStatu == 0),
                user_odCount = x.Orders.Count,
                user_coupons = x.OrderDiscountDetails.Count,
            });
            return Json(q.FirstOrDefault());
        }


        [Authorize]
        public IActionResult myMessage() //通知訊息頁面
        {
            IEnumerable<MessageMemberDetail> q =  _bookShopContext.MessageMemberDetails.Where(x=>x.MemberId== _userInforService.UserId).Include(x=>x.Message);
            return View(q);
        }

        [Authorize]
        public IActionResult getMessage(int id) //訊息細節方法
        {
            MessageMemberDetail target = _bookShopContext.MessageMemberDetails.Find(id)!;
            target.ReadStatu = 1; _bookShopContext.SaveChanges();
            var q = from x in _bookShopContext.MessageMemberDetails
                    join y in _bookShopContext.Messages on x.MessageId equals y.MessageId
                    join z in _bookShopContext.MessageTypes on y.MessageTypeId equals z.MessageTypeId
                    where x.MessageMemberDetailId == id
                    select new
                    {
                         time = x.UpdateTime.Value.ToShortDateString(),
                         read_a = (x.ReadStatu==1)?"已讀":"未讀",
                         content_a = y.MessageContent,
                         type_a = z.MessageTypeName
                    };

            return Json(q.FirstOrDefault());
        }
        [Authorize]
        public IActionResult myCoupons() //coupons頁面
        {
            IEnumerable<OrderDiscountDetail> q = _bookShopContext.OrderDiscountDetails.Where(x => x.MemberId == _userInforService.UserId).Include(x=>x.OrderDiscount);
            return View(q);
        }
        [Authorize]
        public IActionResult FollowCancle(int id) //取消追蹤
        {
            bool isSuccess = false;
            var tool =_bookShopContext.ActionDetials.Where(x => x.ActionToBookId == id).FirstOrDefault();
            if(tool != null)
            {
                _bookShopContext.ActionDetials.Remove(tool);
                _bookShopContext.SaveChanges();
                isSuccess = true;
            }
            return Content(isSuccess.ToString());
        }

        [Authorize]
        public IActionResult myPublisher() //關注的出版社方法
        {
            var q = (from cp in _bookShopContext.CollectedPublishers
                     join p in _bookShopContext.Publishers on cp.PublisherId equals p.PublisherId
                     where cp.MemberId == _userInforService.UserId
                     select new
                     {
                         publisherId = p.PublisherId,
                         publisherName = p.PublisherName,
                     }).ToJson();
            return Json(q);
        }
        [Authorize]
        public IActionResult myAuthor() //關注的作者方法
        {
            var q = (from ca in _bookShopContext.CollectedAuthors
                     join a in _bookShopContext.Authors on ca.AuthorId equals a.AuthorId
                     where ca.MemberId == _userInforService.UserId
                     select new
                     {
                         authorId = a.AuthorId,
                         authorName = a.AuthorName,
                     }).ToJson();
            return Json(q);
        }
        [Authorize]
        public IActionResult cancleAuthor(int id) //取消關注的作者方法
        {
            bool isSuccesse = false;
            var tool = _bookShopContext.CollectedAuthors.Where(x => x.MemberId == _userInforService.UserId && x.AuthorId == id).FirstOrDefault();
            if(tool != null)
            {
                _bookShopContext.CollectedAuthors.Remove(tool);
                _bookShopContext.SaveChanges();
                isSuccesse = true;
            }
            return RedirectToAction("myCollect");
        }
        [Authorize]
        public IActionResult canclePublisher(int id) //取消關注的作者方法
        {
            bool isSuccesse = false;
            var tool = _bookShopContext.CollectedPublishers.Where(x => x.MemberId == _userInforService.UserId && x.PublisherId == id).FirstOrDefault();
            if(tool != null)
            {
                _bookShopContext.CollectedPublishers.Remove(tool);
                _bookShopContext.SaveChanges();
                isSuccesse = true;
            }
            return RedirectToAction("myCollect");
        }


        [Authorize]
        public IActionResult myNotice() //可購買時通知我
        {
            var q = (from b in _bookShopContext.Books
                     join ac in _bookShopContext.ActionDetials on b.BookId equals ac.BookId
                     where ac.MemberId == _userInforService.UserId && ac.ActionId == 1
                     select new
                     {
                         actionDetailId = ac.ActionToBookId,
                         bookId = b.BookId,
                         bookStock = (b.UnitInStock>0)?"可購買":"缺貨中",
                         bookName = b.BookTitle
                     }).ToJson();

            return Json(q);
        }
        [Authorize]
        public IActionResult myCollect() //暫存清單頁面
        {
            var q = from b in _bookShopContext.Books.Include(x => x.BookDiscountDetails).ThenInclude(x => x.BookDiscount).Include(x => x.ActionDetials).Include(x=>x.Publisher)
                    join acd in _bookShopContext.ActionDetials on b.BookId equals acd.BookId
                    where (acd.MemberId == _userInforService.UserId && acd.ActionId == 4)
                    select b;

            return View(q);
        }
        [Authorize]
        public IActionResult myComment() //我的評論頁面
        {
            var q = _bookShopContext.Comments.Where(x => x.MemberId == _userInforService.UserId).Include(x => x.Book); 
            return View(q);
        }

        [Authorize]
        public IActionResult myOrders()  //訂單頁面
        {
            var q = _bookShopContext.Orders.Where(x => x.MemberId == _userInforService.UserId).
                Include(x => x.Payment).
                Include(x => x.Shipment).
                Include(x => x.PayStatus).
                Include(x => x.ShippingStatus).Include(x=>x.Member).ThenInclude(x=>x.Level).ToList();
            return View(q);
        }
        [Authorize]
        public IActionResult alretProflie() //修改資料頁面
        {
            Member member = _bookShopContext.Members.Where(x => x.MemberId == _userInforService.UserId).
                Include(x=>x.Level).
                Include(x=>x.Payment).
                FirstOrDefault()!;
            return View(member);
        } //
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult alretProflie(Member member) //修改資料方法
        {
            bool isExsit = false;
            Member memberupdate = _bookShopContext.Members.FirstOrDefault(x => x.MemberId == member.MemberId)!;
            if (memberupdate != null)
            {
                memberupdate.MemberName = member.MemberName;
                memberupdate.MemberEmail = member.MemberEmail;
                memberupdate.MemberBrithDate = member.MemberBrithDate;
                memberupdate.MemberAddress = member.MemberAddress;
                memberupdate.PaymentId = member.PaymentId;
                _bookShopContext.SaveChanges();
                isExsit = true;
            };

            return Content(isExsit.ToString());
        }
        
        public IActionResult PartailOrderDetail(int id)
        {
            var q = _bookShopContext.OrderDetails.Include(x => x.Book).ThenInclude(x => x.BookDiscountDetails).ThenInclude(x => x.BookDiscount).Where(x => x.OrderId == id);
            return PartialView("PartailOrderDetail", q);
        }

    }
}
