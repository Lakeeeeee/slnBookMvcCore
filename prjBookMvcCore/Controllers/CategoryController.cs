using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prjBookMvcCore.Models;

namespace prjBookMvcCore.Controllers
{
    public class CategoryController : Controller
    {
        BookShopContext db = new();
        //TODO:(書玉)分頁controller發法改寫
        public IActionResult 中文書()
        {
            var category = db.Categories.Select(b => b);
            return View(category); 
        }

        public IActionResult 中文書分類(int ?id)
        {
            if (id == null)
            {
                return RedirectToAction("Menu");
            }

            var category = db.Categories.FirstOrDefault(c => c.CategoryId == id);
            if (category == null)
            {
                return NotFound();
            }

            var subCategory = db.SubCategories.FirstOrDefault(c => c.CategoryId == id);

            var subviewModel = new ViewModel.CategoryViewModel
            {
                CategoryId = category.CategoryId,
                CategoryName = category.CategoryName,
                SubCategoryId = subCategory?.SubCategoryId??0,
                SubCategoryName = subCategory?.SubCategoryName,
            };
            return View(subviewModel);
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
