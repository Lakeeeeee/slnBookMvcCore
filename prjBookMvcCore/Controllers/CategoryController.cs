using Microsoft.AspNetCore.Mvc;
using prjBookMvcCore.Models;

namespace prjBookMvcCore.Controllers
{
    public class CategoryController : Controller
    {
        BookShopContext db = new BookShopContext();

        public IActionResult 中文書()
        {
            var datas = from c in db.Categories
                        select c;
            return View(datas);
        }
        public IActionResult 語言學習()
        {
            IEnumerable<SubCategory> datas =db.SubCategories.Where(x=>x.CategoryId==29) ;
            return View(datas);
        }

        public IActionResult 漫畫圖文書()
        {
            IEnumerable<SubCategory> datas = db.SubCategories.Where(x => x.CategoryId == 28);
            return View(datas);
        }

        public IActionResult 旅遊()
        {
            IEnumerable<SubCategory> datas = db.SubCategories.Where(x => x.CategoryId ==23);
            return View(datas);
        }

        //public IActionResult 年齡()
        //{
        //    IEnumerable<SubCategory> datas = db.SubCategories.Where(x=>x.CategoryId==);
        //}
    }
}
