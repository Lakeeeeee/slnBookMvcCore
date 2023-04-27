using Microsoft.AspNetCore.Mvc;
using prjBookMvcCore.Models;
using System.Diagnostics;

namespace prjBookMvcCore.Controllers
{
    public class HomeController : Controller
    {
        
        


        public IActionResult Home()
        {
            return View();
        }

        public IActionResult commentList()
        {
            return View();
        }



        #region(系統預設的東西, 先留著, 之後不需要再刪掉)
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        #endregion

        public IActionResult Category()
        {
            return View();
        }
    

    }
}