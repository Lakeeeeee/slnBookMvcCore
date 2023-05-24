﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using prjBookMvcCore.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using GoogleReCaptcha.V3.Interface;

namespace prjBookMvcCore.Controllers
{
    public class ActionController : Controller
    {
        BookShopContext db = new();

        public string ActionTo(int bookID, int memberID, int actionID)
        {
            bool isSuccess = true;

            var query = from ad in db.ActionDetials
                        where ad.ActionId == actionID && ad.MemberId == memberID && ad.BookId == bookID
                        select ad;
            if (query.Count() != 0)
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

        public string memAction(int bookID, int memberID, int actionID)
        {
           string isSuccess ;
            var q = db.ActionDetials.Where(a => a.ActionId == actionID & a.MemberId == memberID & a.BookId == bookID).Select(a => a);
            if (q.Count() != 0)
            {
                isSuccess = "false";
                return isSuccess;
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
                isSuccess = "true";
                return isSuccess;
            }
        }

        public string viewcart刪除(int ActionToBookId)
        {
            string isSuccess = " false";
            var q = db.ActionDetials.Where(a => a.ActionToBookId == ActionToBookId).FirstOrDefault();
            if (q != null)
            {
                db.ActionDetials.Remove(q);
                db.SaveChanges();
                isSuccess = "true";
            }
            return isSuccess;
        }
    }
}
