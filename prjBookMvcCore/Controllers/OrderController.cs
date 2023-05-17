﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using prjBookMvcCore.Models;

namespace prjBookMvcCore.Controllers
{
    public class OrderController : Controller
    {
        BookShopContext db = new();
        public IActionResult ListCart()
        {
            return View();
        }
        public string StoreShoppingCart(int bookID, int memberID, int actionID)
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
        public IActionResult ShoppingCart(int memberID)
        {
            List<CInformation> cartItems = new List<CInformation>();
            var query = from b in db.Books
                        join ad in db.ActionDetials
                        on b.BookId equals ad.BookId
                        where memberID == memberID && ad.ActionId == 3
                        orderby ad.ActionToBookId descending
                        select new
                        {
                            書名 = b.BookTitle,
                            書本ID = b.BookId,
                            定價 = b.UnitPrice,
                            出版社 = b.Publisher.PublisherName,
                            折扣名 = b.BookDiscountDetails.Select(x => x.BookDiscount.BookDiscountName).FirstOrDefault(),
                            折扣 = b.BookDiscountDetails.Select(x => x.BookDiscount.BookDiscountAmount).FirstOrDefault(),
                            庫存 = b.UnitInStock,
                        };
            foreach (var item in query)
            {
                Book b = new Book()
                {
                    BookId = item.書本ID,
                    BookTitle = item.書名,
                    UnitInStock = item.庫存,
                    UnitPrice = item.定價,
                };
                Publisher p = new Publisher { PublisherName = item.出版社};
                BookDiscount bd = new BookDiscount { BookDiscountAmount = item.折扣, BookDiscountName = item.折扣名 };
                CInformation tmp = new CInformation(){
                    book = b,
                    bookDiscount = bd,
                    publisher = p,
                };
                cartItems.Add(tmp);
            }
            return View(cartItems);
        }

        public IActionResult checkOutInfo()
        {
            return View();
        }

        public IActionResult checkOutConfirm()
        {
            return View();
        }
        public IActionResult checkOutFinal()
        {
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }

        //API 測試中=====

   //     const request = Require('request');
   //     const cheerio = require('cheerio');
   //     const async = require('async');

   //     [HttpGet]
   //     [Route("api/ibon-stores")]
   //     public IActionResult GetIbonStores()
   //     {
   //         var result = new List<Dictionary<string, string>>();

   //         getCities((cities) => {
   //             async.map(cities, (city, callback) => {
   //                 getStories(city, (stores) => {
   //                     callback(null, stores);
   //                 })
   //             }, (err, results) => {result = [].concat.apply([], results);
   //         });
   //     });

   // return Ok(result);
   // }

   //async function getCities(callback)
   // {
   //     request('http://www.ibon.com.tw/retail_inquiry.aspx#gsc.tab=0', (err, res, body) => {
   //         var $ = cheerio.load(body)
   //         var cities = $('#Class1 option').map((index, obj) => {
   //         return $(obj).text()
   //     }).get()
   //         callback(cities)
   //     })
   // }

   // function getStories(city, callback)
   // {
   //     var options = {
   //     url: 'http://www.ibon.com.tw/retail_inquiry_ajax.aspx',
   //     method: 'POST',
   //     form:
   //     {
   //     strTargetField: 'COUNTY',
   //         strKeyWords: city,
   //     }
   // }
   // request(options, (err, res, body) => {
   //     var $ = cheerio.load(body)
   //     var stores = $('tr').map((index, obj) => {
   //         return new Dictionary<string, string>{
   //             {"id", $(obj).find('td').eq(0).text().trim()},
   //             {"store", $(obj).find('td').eq(1).text().trim()},
   //             {"address", $(obj).find('td').eq(2).text().trim()}
   //         };
   //     }).get()
   //     stores.shift()
   //     callback(stores)
   // })
}

    }

