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
using System.Net.Mail;
using System.Text;

namespace prjBookMvcCore.Controllers
{
    public class OrderController : Controller
    {
        public UserInforService _user { get; set; }
        BookShopContext _db = new BookShopContext();
        private readonly IConfiguration _config;

        public OrderController(UserInforService user, IConfiguration config, BookShopContext db)
        {
            _user = user;
            _db = db;
            _config = config;
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

            _db.SaveChanges();
            theorderID = order.OrderId;
            return Content(theorderID.ToString());
        }

        public bool updatePoint(int memberid)
        {
            bool isSuccess = false;

            Order order = _db.Orders.OrderByDescending(o => o.OrderId).FirstOrDefault(x => x.MemberId == memberid);
            Member member = _db.Members.Find(memberid);
            if (order != null)
            {
                member.Points = (int)(member.Points - order.PointAmount);
                switch (member.LevelId)
                {
                    case 3:
                        member.Points = (int)((member.Points) + (int)Math.Ceiling((double)order.FinalPay) * 0.01);
                        break;
                    case 4:
                        member.Points = (int)((member.Points) + (int)Math.Ceiling((double)order.FinalPay) * 0.05);
                        break;
                    case 5:
                        member.Points = (int)((member.Points) + (int)Math.Ceiling((double)order.FinalPay) * 0.05);
                        break;
                };
                _db.SaveChanges();
                isSuccess = true;
            }
            return isSuccess;

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
                decimal 折扣 = _db.BookDiscountDetails.Include(x => x.BookDiscount).Where(x => x.BookId == bookid & x.BookDiscountStartDate < DateTime.Now & x.BookDiscountEndDate > DateTime.Now).Select(x => x.BookDiscount).Select(x => x.BookDiscountAmount).FirstOrDefault();
                decimal bookPrice = _db.Books.Find(bookid).UnitPrice;
                OrderDetail orderDetail = new OrderDetail();
                orderDetail.BookId = bookid;
                orderDetail.OrderId = order.OrderId;
                orderDetail.UnitPrice = Math.Ceiling(bookPrice * 折扣);
                list.Add(orderDetail);
            };

			for (int n = 0; n < list.Count(); n++)
            {
                list[n].Quantity = quantity[n];
                var thebook = _db.Books.Where(x => x.BookId == list[n].BookId).FirstOrDefault();
                thebook.UnitInStock = thebook.UnitInStock - quantity[n];
            };
            _db.OrderDetails.AddRange(list);

            //刪除購物車
            foreach (int item in acids)
            {
                ActionDetial tool = _db.ActionDetials.Find(item);
                _db.ActionDetials.Remove(tool);
            }
            _db.SaveChanges();
            //writeOrderMs(order, _config, _db); 寫訂單的信

            Member member = _db.Members.Find(order.MemberId);
            _db.SaveChanges();
            isSuccess = true;
            return Content(isSuccess.ToString());
        }
        
        [HttpGet]
        public IActionResult CollectList(int id) //暫存清單頁面
        {
            var q2 = from ad in _db.ActionDetials
                     join bk in _db.Books on ad.BookId equals bk.BookId
                     join bde in _db.BookDiscountDetails on bk.BookId equals bde.BookId
                     join bd in _db.BookDiscounts on bde.BookDiscountId equals bd.BookDiscountId 
                     where ad.MemberId == id && ad.ActionId == 4 && bde.BookDiscountStartDate < DateTime.Now & bde.BookDiscountEndDate > DateTime.Now
                     select new
                     {
                         acid = ad.ActionToBookId,
                         book = bk,
                         Dprice = Math.Ceiling(bk.UnitPrice * bd.BookDiscountAmount),
                     };
            return Json(q2);
        }


        public string moveToList(int bookID, int memberID, int actionID)
        {
            var query = _db.ActionDetials.
                        Where(x => x.ActionId == actionID && x.MemberId == memberID && x.BookId == bookID).
                        FirstOrDefault();
            if (query != null)
            {
                string jsonData = JsonConvert.SerializeObject(Json(new { success = false, message = "暫存清單已有此品項" }));
                return jsonData;
            }
            else
            {
                var q2 = _db.ActionDetials.
                            Where(x => x.ActionId == 7 && x.MemberId == memberID && x.BookId == bookID).
                            FirstOrDefault();
                if (_db.Books.Find(bookID).UnitInStock > 0)
                {
                    q2.ActionId = 4;
                    _db.SaveChanges();
                    string jsonData2 = JsonConvert.SerializeObject(Json(new { success = true, message = "已加入暫存!" }));
                    return jsonData2;
                }
                else
                {
                    string jsonData3 = JsonConvert.SerializeObject(Json(new { success = false, message = "這本書暫時沒有庫存了, 敬請期待" }));
                    return jsonData3;
                }
            }
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

        public void writeOrderMs(Order order, IConfiguration config, BookShopContext db)
        {
            string mailContent = $"親愛的讀者您好：<br>" +
                $"感謝您對讀本的支持，您於讀本的訂購已經完成！" +
                $"為保護您的個資安全，即日起將不在信函中顯示完整訂單 / 出貨明細，<br>" +
                $"也不提供發送手機簡訊服務，僅以電子郵件進行通知，請務必留意相關訊息！<br>" +
                $"建議您，可至讀本會員中心 > 訂單查詢，謝謝您! " +
                $"您本次訂購的商品將由 {order.Shipment.ShipmentName} 運送。" +
                $"<p>讀本購書平台</p>";
            string mailSubject = "[讀本] 訂單完成通知信";
            string SmtpServer = "smtp.gmail.com";
            string GoogleMailUserID = config["GoogleMailUserID"];
            string GoogleMailUserPwd = config["GoogleMailUserPwd"];
            int port = 587;
            Member a = db.Members.Find(order.MemberId);
            MailMessage mms = new MailMessage();
            mms.From = new MailAddress(GoogleMailUserID);
            mms.Subject = mailSubject;
            mms.Body = mailContent;
            mms.IsBodyHtml = true;
            mms.SubjectEncoding = Encoding.UTF8;
            mms.To.Add(new MailAddress(a.MemberEmail));
            using (SmtpClient client = new SmtpClient(SmtpServer, port))
            {
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential(GoogleMailUserID, GoogleMailUserPwd);
                client.Send(mms);
            }
        }

        public string editPayment(int id)
        {
            bool isSuccess = false;
            var order = _db.Orders.Find(id);
            if(order != null)
            {
				order.PayStatusId = 5;
				_db.SaveChanges();
                isSuccess = true;
			}
            return isSuccess.ToString();

        }


        //----------------------------------------------------------------
        public IActionResult checkOutConfirm()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult checkOutConfirm(OrderViewModel model)
        {

            return View(model);
        }
        public IActionResult checkOutFinal(int id)
        {
            Order order = _db.Orders.Include(x => x.Member).Include(x => x.OrderDetails).ThenInclude(x => x.Book).Where(x => x.OrderId == id).FirstOrDefault();
            return View(order);
        }

		public IActionResult FFPage(int id)
		{
			Order order = _db.Orders.Include(x => x.Member).Include(x => x.OrderDetails).ThenInclude(x => x.Book).Where(x => x.OrderId == id).FirstOrDefault();
			return View(order);
		}

	}
}

