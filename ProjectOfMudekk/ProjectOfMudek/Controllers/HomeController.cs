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

        
    }
}
