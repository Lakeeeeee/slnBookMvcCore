using Microsoft.AspNetCore.Mvc;
using prjBookMvcCore.Models;

namespace prjBookMvcCore.Controllers
{
    public class CategoryController : Controller
    {
        BookShopContext db = new();
        //TODO:(書玉)分頁controller發法改寫
        public IActionResult 中文書(int? id)
        {
            if (id == null) { return RedirectToAction("中文書"); }
            var datas = db.Books.Where(b => b.LanguageId == id).Select(b=>b.BookId);
            ViewBag.BookId = datas;
            return View(); 
        }

        public IActionResult 人文社科()
        {
            IEnumerable<SubCategory> datas = db.SubCategories.Where(x => x.CategoryId == 29);
            return View(datas);
        }

        public IActionResult 心理勵志()
        {
            IEnumerable<SubCategory> datas = db.SubCategories.Where(x => x.CategoryId == 28);
            return View(datas);
        }

        public IActionResult 文學小說()
        {
            IEnumerable<SubCategory> datas = db.SubCategories.Where(x => x.CategoryId ==23);
            return View(datas);
        }

        public IActionResult 日中對照()
        {
            IEnumerable<SubCategory> datas = db.SubCategories.Where(x => x.CategoryId ==1);
            return View(datas);
        }

        public IActionResult 生活風格()
        {
            IEnumerable<SubCategory> datas = db.SubCategories.Where(x => x.CategoryId == 1);
            return View(datas);
        }
        public IActionResult 考試用書()
        {
            IEnumerable<SubCategory> datas = db.SubCategories.Where(x => x.CategoryId == 1);
            return View(datas);
        }
        public IActionResult 自然科普()
        {
            IEnumerable<SubCategory> datas = db.SubCategories.Where(x => x.CategoryId == 1);
            return View(datas);
        }
        public IActionResult 宗教命理()
        {
            IEnumerable<SubCategory> datas = db.SubCategories.Where(x => x.CategoryId == 1);
            return View(datas);
        }
        public IActionResult 旅遊()
        {
            IEnumerable<SubCategory> datas = db.SubCategories.Where(x => x.CategoryId == 1);
            return View(datas);
        }
        public IActionResult 商業理財()
        {
            IEnumerable<SubCategory> datas = db.SubCategories.Where(x => x.CategoryId == 1);
            return View(datas);
        }
        public IActionResult 童書青少年文學()
        {
            IEnumerable<SubCategory> datas = db.SubCategories.Where(x => x.CategoryId == 1);
            return View(datas);
        }
        public IActionResult 飲食()
        {
            IEnumerable<SubCategory> datas = db.SubCategories.Where(x => x.CategoryId == 1);
            return View(datas);
        }
        public IActionResult 電腦資訊()
        {
            IEnumerable<SubCategory> datas = db.SubCategories.Where(x => x.CategoryId == 1);
            return View(datas);
        }
        public IActionResult 漫畫圖文書()
        {
            IEnumerable<SubCategory> datas = db.SubCategories.Where(x => x.CategoryId == 1);
            return View(datas);
        }
        public IActionResult 語言學習()
        {
            IEnumerable<SubCategory> datas = db.SubCategories.Where(x => x.CategoryId == 1);
            return View(datas);
        }
        public IActionResult 親子教養()
        {
            IEnumerable<SubCategory> datas = db.SubCategories.Where(x => x.CategoryId == 1);
            return View(datas);
        }
        public IActionResult 醫療保健()
        {
            IEnumerable<SubCategory> datas = db.SubCategories.Where(x => x.CategoryId == 1);
            return View(datas);
        }
        public IActionResult 藝術設計()
        {
            IEnumerable<SubCategory> datas = db.SubCategories.Where(x => x.CategoryId == 1);
            return View(datas);
        }
    }
}
