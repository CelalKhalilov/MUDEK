using Entities.Entities.Models;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProjectOfMudek.Context;
using ProjectOfMudek.Models;
using System.Diagnostics;

namespace ProjectOfMudek.Controllers
{
    public class HomeController : Controller
    {
        private readonly MudekContext _context;

        public HomeController(MudekContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var a = _context.academicUnits.ToList();
            return View(a);
        }

        public IActionResult Ogretmen()
        {
            return View();
        }

        public IActionResult Dogrulama()
        {
            return View();
        }
        [HttpPost]
        public IActionResult OgretmenLogin(Teacher teacher)
        {
            var user = _context.Teachers.FirstOrDefault(a => a.Gmail == teacher.Gmail && a.Password == teacher.Password);
            if (user != null)
            {
                // Başarılı giriş, "/Admin/Index" sayfasına yönlendir
                HttpContext.Session.SetString("IsLoggedIn", "true");
                return RedirectToAction("Index", "Teacher");
            }
            else
            {
                // Hatalı giriş, sayfada bir hata mesajı gösterilebilir veya farklı bir işlem yapılabilir
                ViewBag.Error = "Invalid username or password.";
                return View();
            }
        }

        public IActionResult Logout()
        {
            // Oturumu temizle
            HttpContext.Session.Clear();

            // Login sayfasına yönlendir
            return RedirectToAction("Ogretmen", "Home");
        }


        [HttpPost]
        public IActionResult Ogretmen(Teacher teacher)
        {
            if (ModelState.IsValid)
            {
                _context.Teachers.Add(teacher);
                _context.SaveChanges();
                return RedirectToAction("Dogrulama", "Home");
            }
            return View();
        }
        
    }
}
