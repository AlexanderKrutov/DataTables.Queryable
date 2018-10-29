using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AspNetCore.MVC.Sample.Models;

namespace AspNetCore.MVC.Sample.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction(nameof(Sample1));
        }

        public IActionResult Sample1()
        {
            return View();
        }

        public IActionResult Sample2()
        {
            return View();
        }

        public IActionResult Sample3()
        {
            return View();
        }

        public IActionResult Sample4()
        {
            return View();
        }

        public IActionResult Sample5()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
