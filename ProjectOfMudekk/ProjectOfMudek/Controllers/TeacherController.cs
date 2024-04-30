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
            return View();
        }
    }
}
