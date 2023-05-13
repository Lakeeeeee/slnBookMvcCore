using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using prjBookMvcCore.Models;
using Newtonsoft.Json;
namespace prjBookMvcCore.Controllers
{
    public class ActionController : Controller
    {
        BookShopContext db = new();
        public string ActionTo(int bookID, int memberID,int actionID)
        {
            bool isSuccess = true;

            var query = from ad in db.ActionDetials
                        where ad.ActionId == actionID && ad.MemberId == memberID && ad.BookId == bookID
                        select ad;
            if(query.Count() != 0)
            {
                isSuccess = false;
            }
            else
            {
                ActionDetial newAd = new ActionDetial
                {
                    ActionId = actionID,
                    BookId = bookID,
                    MemberId = memberID,
                };
                db.ActionDetials.Add(newAd);
                db.SaveChanges();
            }
            string jsonData = JsonConvert.SerializeObject(isSuccess);
            return jsonData;
        }
    }
}
