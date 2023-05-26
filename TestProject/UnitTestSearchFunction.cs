using Microsoft.AspNetCore.Mvc;
using prjBookMvcCore.Controllers;
using prjBookMvcCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject
{
    internal class UnitTestSearchFunction
    {
        private HomeController homeController;

        [SetUp]
        public void Setup()
        {
            
        }
        

        [Test]

        public void TestSearch()
        {
            // 定義測試所需的輸入參數
            string txtKeyword = "sa";
            int frontprice = 200;
            int backprice = 400;
            decimal frontdiscount = (decimal)0.95;
            decimal backdiscount = (decimal)0.88;
            DateTime frontdate = new DateTime(2023, 5, 25);
            DateTime backdate = new DateTime(2005, 1, 1);

            // 執行搜尋功能
            IActionResult result = homeController.searchList(txtKeyword, frontprice, backprice, frontdiscount, backdiscount, frontdate, backdate);

            Assert.IsNotNull(result);
            // 驗證結果類型
            Assert.IsInstanceOf<ViewResult>(result);

            // 取得搜尋結果的Model
            ViewResult viewResult = (ViewResult)result;
            var searchInformation = viewResult.Model as CForHomePage;

            // 驗證searchInformation的相關屬性或方法結果
            // ...
        }






    }

}
