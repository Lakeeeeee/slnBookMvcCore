using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using prjBookMvcCore.Models;
namespace prjBookMvcCore.Controllers
{
    public class ActionController : Controller
    {
        BookShopContext db = new();
        public IActionResult ActionTo(int bookID, int memberID,int actionID)
        {
            ActionDetial ad = new ActionDetial
            {
                ActionId = actionID,
                BookId = bookID,
                MemberId = memberID,
            };

            db.ActionDetials.Add(ad);
            db.SaveChanges();

            return View();
        }
    }
}
