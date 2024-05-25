using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using ProjectOfMudek.Context;

namespace ProjectOfMudek.Controllers
{
    public class AdminController : Controller
    {
        private readonly MudekContext _context;

        public AdminController(MudekContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var a = _context.academicUnits.ToList();
            var b = _context.faculties.ToList();
            var c = _context.departments.ToList();
            var d = _context.periods.ToList();
            var e = _context.lessons.ToList();
            // var userId = HttpContext.Session.GetInt32("Id");
            // ViewBag.UserId = userId;
            ViewBag.academicUnits = a;
            ViewBag.faculties = b;
            ViewBag.departments = c;
            ViewBag.periods = d;
            ViewBag.lessons = e;
            return View();
        }

        [HttpPost]
        public IActionResult Index(AcademicUnit academicUnit)
        {
            if (ModelState.IsValid)
            {
                _context.academicUnits.Add(academicUnit);
                _context.SaveChanges();
                return RedirectToAction("Index", "Admin", new { id = academicUnit.Id });
            }
            return View();
        }

        [HttpGet]
        public IActionResult AcademicUnitUpdate(int id)
        {

            var bul = _context.academicUnits.FirstOrDefault(a => a.Id == id);
            return View(bul);

        }

        [HttpPost]
        public IActionResult AcademicUnitUpdate(AcademicUnit uptdaeAcademicUnit)
        {
            var academicUnit = _context.academicUnits.SingleOrDefault(g => g.Id == uptdaeAcademicUnit.Id);

            if (academicUnit != null)
            {
                academicUnit.UnitName = uptdaeAcademicUnit.UnitName;
                
                _context.SaveChanges();

                return RedirectToAction("Index","Admin"); // veya başka bir yönlendirme
            }
            else
            {
                return NotFound(); // veya uygun bir hata işleme yöntemi
            }
        }

        [HttpPost]
        public IActionResult AcademicUnitDelete(int academicUnitId)
        {
            var academicUnit = _context.academicUnits.Find(academicUnitId);
            System.Console.WriteLine($"HATA HATA HATA HAAAAAAAATAAAAAAAAA: >{academicUnit}<");
            if (academicUnit == null)
            {
                return NotFound(); // veya uygun bir hata mesajı döndürün
            }
            else
            {
                _context.Remove(academicUnit);
                _context.SaveChanges();
                return RedirectToAction("Index", "Admin");
            }

        }


        // =================================================================================================

        [HttpGet]
        public IActionResult FacultyAdd(int id)
        {
            var academicUnit = _context.academicUnits.FirstOrDefault(g => g.Id == id);

            // Gida örneği null değilse ve Id değeri istediğiniz değere eşitse ViewBag'e ata
            if (academicUnit != null)
            {
                ViewBag.academicUnitId = academicUnit.Id;
            }

            return View(new AcademicUnit());
        }

        [HttpGet]
        public IActionResult FacultyUpdate(int id)
        {

            var bul = _context.faculties.FirstOrDefault(a => a.Id == id);
            return View(bul);

        }

        [HttpPost]
        public IActionResult FacultyUpdate(Faculty uptadeFaculty )
        {
            var faculty = _context.faculties.SingleOrDefault(g => g.Id == uptadeFaculty.Id);

            if (faculty != null)
            {
                faculty.FacultyName = uptadeFaculty.FacultyName;
                
                _context.SaveChanges();

                return RedirectToAction("Index","Admin"); // veya başka bir yönlendirme
            }
            else
            {
                return NotFound(); // veya uygun bir hata işleme yöntemi
            }
        }

        [HttpPost]
        public IActionResult FacultyAdd(Faculty p)
        {
            if (ModelState.IsValid)
            {
                _context.faculties.Add(p);
                _context.SaveChanges();
                return RedirectToAction("Index", "Admin");
            }

            return View(p);
        }

        [HttpPost]
        public IActionResult FacultyDelete(int facultyId)
        {
            var faculty = _context.faculties.Find(facultyId);
            if (faculty == null)
            {
                return NotFound(); // veya uygun bir hata mesajı döndürün
            }
            else
            {
                _context.Remove(faculty);
                _context.SaveChanges();
                return RedirectToAction("Index", "Admin");
            }

        }

        // =================================================================================================

        [HttpGet]
        public IActionResult DepartmentAdd(int id)
        {
            var facultie = _context.faculties.FirstOrDefault(g => g.Id == id);

            // Gida örneği null değilse ve Id değeri istediğiniz değere eşitse ViewBag'e ata
            if (facultie != null)
            {
                ViewBag.facultyId = facultie.Id;
            }

            return View(new Faculty());
        }

        [HttpPost]
        public IActionResult DepartmentAdd(Department p)
        {
            if (ModelState.IsValid)
            {
                _context.departments.Add(p);
                _context.SaveChanges();
                return RedirectToAction("Index", "Admin");
            }

            return View(p);
        }

        [HttpPost]
        public IActionResult DepartmentUpdate(Department updateDepartment)
        {
            var department = _context.departments.SingleOrDefault(g => g.Id == updateDepartment.Id);

            if (department != null)
            {
                department.DepartmentName = updateDepartment.DepartmentName;
                
                _context.SaveChanges();

                return RedirectToAction("Index","Admin"); // veya başka bir yönlendirme
            }
            else
            {
                return NotFound(); // veya uygun bir hata işleme yöntemi
            }
        }

        [HttpPost]
        public IActionResult DepartmentDelete(int departmentId)
        {
            var department = _context.departments.Find(departmentId);
            if (department == null)
            {
                return NotFound(); // veya uygun bir hata mesajı döndürün
            }
            else
            {
                _context.Remove(department);
                _context.SaveChanges();
                return RedirectToAction("Index", "Admin");
            }

        }

        // =================================================================================================

        [HttpGet]
        public IActionResult PeriodAdd(int id)
        {
            var department = _context.departments.FirstOrDefault(g => g.Id == id);

            // Gida örneği null değilse ve Id değeri istediğiniz değere eşitse ViewBag'e ata
            if (department != null)
            {
                ViewBag.departmentId = department.Id;
            }

            return View(new Department());
        }

        [HttpPost]
        public IActionResult PeriodAdd(Period p)
        {
            if (ModelState.IsValid)
            {
                _context.periods.Add(p);
                _context.SaveChanges();
                return RedirectToAction("Index", "Admin");
            }

            return View(p);
        }

        [HttpPost]
        public IActionResult PeriodUpdate(Period updatePeriod)
        {
            var period = _context.periods.SingleOrDefault(g => g.Id == updatePeriod.Id);

            if (period != null)
            {
                period.PeriodName = updatePeriod.PeriodName;
                
                _context.SaveChanges();

                return RedirectToAction("Index","Admin"); // veya başka bir yönlendirme
            }
            else
            {
                return NotFound(); // veya uygun bir hata işleme yöntemi
            }
        }

        [HttpPost]
        public IActionResult PeriodDelete(int periodId)
        {
            var period = _context.periods.Find(periodId);
            if (period == null)
            {
                return NotFound(); // veya uygun bir hata mesajı döndürün
            }
            else
            {
                _context.Remove(period);
                _context.SaveChanges();
                return RedirectToAction("Index", "Admin");
            }

        }

        // =================================================================================================

        [HttpGet]
        public IActionResult LessonAdd(int id)
        {
            var period = _context.periods.FirstOrDefault(g => g.Id == id);

            // Gida örneği null değilse ve Id değeri istediğiniz değere eşitse ViewBag'e ata
            if (period != null)
            {
                ViewBag.periodId = period.Id;
            }

            return View(new Period());
        }

        [HttpPost]
        public IActionResult LessonAdd(Lesson p)
        {
            if (ModelState.IsValid)
            {
                _context.lessons.Add(p);
                _context.SaveChanges();
                return RedirectToAction("Index", "Admin");
            }

            return View(p);
        }

        [HttpPost]
        public IActionResult LessonUpdate(Lesson updateLesson)
        {
            var lesson = _context.lessons.SingleOrDefault(g => g.Id == updateLesson.Id);

            if (lesson != null)
            {
                lesson.LessonName = updateLesson.LessonName;
                
                _context.SaveChanges();

                return RedirectToAction("Index","Admin"); // veya başka bir yönlendirme
            }
            else
            {
                return NotFound(); // veya uygun bir hata işleme yöntemi
            }
        }

        [HttpPost]
        public IActionResult LessonDelete(int lessonId)
        {
            var lesson = _context.lessons.Find(lessonId);
            if (lesson == null)
            {
                return NotFound(); // veya uygun bir hata mesajı döndürün
            }
            else
            {
                _context.Remove(lesson);
                _context.SaveChanges();
                return RedirectToAction("Index", "Admin");
            }

        }

        // =================================================================================================

        public IActionResult Kullanicilar()
        {
            var a = _context.Teachers.ToList();
            ViewBag.teachers = a;
            var userId = HttpContext.Session.GetInt32("Id");
            ViewBag.UserId = userId;
            var teach = _context.Teachers.ToList();
            ViewBag.teach = teach;
            return View();
        }

    }
}