using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Security.Policy;

namespace prjBookMvcCore.Controllers
{
    public class OrderController : Controller
    {
        [Authorize]
        public IActionResult ListCart()
        {
            return View();
        }
        [Authorize]
        public IActionResult ShoppingCart()
        {
            return View();
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

