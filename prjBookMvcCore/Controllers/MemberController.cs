using Microsoft.AspNetCore.Mvc;
using prjBookMvcCore.Models;

namespace prjBookMvcCore.Controllers
{
    public class MemberController : Controller
    {
        BookShopContext db = new BookShopContext();
        
        int testMemID = 66; //預設會員ID

        public IActionResult myMessage() //通知訊息
        {
            IEnumerable<CustomerService> q = db.CustomerServices.Where(x => x.MemberId == testMemID);
            return View(q);
        }

        public IActionResult myPublisher() //關注的出版社
        {
            IEnumerable<CollectedPublisher> q = db.CollectedPublishers.Where(x => x.MemberId == testMemID);
            return View(q);
        }


        public IActionResult myAuthor() //關注的作者
        {
            IEnumerable<CollectedAuthor> q = db.CollectedAuthors.Where(x => x.MemberId == testMemID);
            return View(q);
        }

        public IActionResult myNotice() //可購買時通知我
        {
            return View();
        }
        public IActionResult myCollect() //暫存清單
        {
            IEnumerable<Book> q = db.ActionDetials.Where(x => x.MemberId == testMemID && x.ActionId == 2).Select(x => x.Book);
            return View(q);
        }

        #region(訂單修改/取消)

        public IActionResult editOrders(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("myOrders");
            }
            Order order = db.Orders.FirstOrDefault(x => x.OrderID == id);
            if (order == null)
            {
                return RedirectToAction("myOrders");
            }
            return View(order);
        }

        //[HttpPost]
        //public ActionResult editOrders()
        //{
        //    var q = db.Orders.Where(x => x.Member.MemberID == testMemID).ToList();
        //    return View(q);
        //}

        #endregion //to do

        public IActionResult myOrders()  //訂單查詢
        {
            var q = db.Orders.Where(x => x.MemberId == testMemID).ToList();
            return View(q);
        }

        public IActionResult alretProflie()
        {
            return View();
        }


        public IActionResult alretPassword()
        {
            return View();
        }

    }
}
