using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProjectOfMudek.Context;

namespace ProjectOfMudek.Controllers
{
    public class TeacherController : Controller
    {
        private readonly MudekContext _context;

        public TeacherController(MudekContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var a = _context.academicUnits.ToList();
            var b  = _context.faculties.ToList();
            var c  = _context.departments.ToList();
            ViewBag.AcademicUnit = new SelectList(a,"AcademicUnitId","UnitName" );
            ViewBag.Faculties = new SelectList(b,"FacultyId", "FacultyName");
            ViewBag.Department = new SelectList(c,"DepartmentId", "DepartmentName");
            return View();
        }
        public IActionResult Tablo()
        {
            var a = _context.learningOutcomess.ToList();
            return View();
        }
        
        [HttpPost]
        public IActionResult Tablo(LearningOutcomes learningOutcomes)
        {
            if (ModelState.IsValid)
            {
                _context.learningOutcomess.Add(learningOutcomes);
                _context.SaveChanges();
                return RedirectToAction("Tablo", "Teacher");
            }
            return View();
        }
        public IActionResult Upload()
        {
            return View();
        }
        public IActionResult DegerlendirmeAraclari()
        {
            return View();
        }
        public IActionResult Hesaplamalar()
        {
            return View();
        }
        public IActionResult ProfilAyarlari()
        {
            var a = _context.Teachers.ToList();
            return View(a);
        }
    }
}
