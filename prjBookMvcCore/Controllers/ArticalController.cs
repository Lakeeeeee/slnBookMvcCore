using Microsoft.AspNetCore.Mvc;
using prjBookMvcCore.Models;

namespace prjBookMvcCore.Controllers
{
    public class ArticalController : Controller
    {
        public IActionResult List全部文章() 
        {
            BookShopContext db=new BookShopContext();
            var datas = from a in db.Articals select a;
            return View(datas); 
        }

        public IActionResult Detail文章(int? id)
        {
            if (id == null) { return RedirectToAction("List全部文章"); }
            ViewBag.ArticalId = id;
            return View();
            //BookShopContext db = new BookShopContext();
            //var datas=db.ArticalToBookDetails.Where(d=>d.ArticalId==id).Select(d=>d);
            //return View(datas);
        }
        
        //public IActionResult Index() { return View(); }
    }
}
