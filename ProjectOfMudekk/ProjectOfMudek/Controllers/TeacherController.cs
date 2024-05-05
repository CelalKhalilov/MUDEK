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
            var b = _context.faculties.ToList();
            var c = _context.departments.ToList();
            ViewBag.AcademicUnit = new SelectList(a, "AcademicUnitId", "UnitName");
            ViewBag.Faculties = new SelectList(b, "FacultyId", "FacultyName");
            ViewBag.Department = new SelectList(c, "DepartmentId", "DepartmentName");
            return View();
        }
        public IActionResult Tablo()
        {
            var a = _context.learningOutcomess.ToList();

            ViewBag.OutcomesList = a;

            return View();
        }

        [HttpPost]
        public IActionResult TabloDelete(int TabloId)
        {
            var OgrenimCiktilari = _context.learningOutcomess.Find(TabloId);
            if (OgrenimCiktilari == null)
            {
                return NotFound(); // veya uygun bir hata mesajı döndürün
            }
            else
            {
                _context.Remove(OgrenimCiktilari);
                _context.SaveChanges();
                return RedirectToAction("Tablo", "Teacher");
            }

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


        [HttpPost]
        public IActionResult TabloGuncelle(LearningOutcomes learningOutcomes)
        {

            var learningOutcome = _context.learningOutcomess.SingleOrDefault(g => g.Id == learningOutcomes.Id);

            if (learningOutcome != null)
            {
                learningOutcome.Keywords = learningOutcomes.Keywords;
                learningOutcome.Definition = learningOutcomes.Definition;

                _context.SaveChanges();

                return RedirectToAction("Tablo", "Teacher"); // veya başka bir yönlendirme
            }
            else
            {
                return NotFound(); // veya uygun bir hata işleme yöntemi
            }
        }


        public IActionResult Upload()
        {
            TumClass model = new TumClass();
            model.AssessmentToolList = _context.assessmentTools.ToList();
            model.SubAssessmentToolList = _context.subAssessmentTools.ToList();
            return View(model);
        }


        public IActionResult DegerlendirmeAraclari()
        {
            var a = _context.assessmentTools.ToList();
            var b = _context.subAssessmentTools.ToList();
            ViewBag.assessmentTools = a;
            ViewBag.subAssessmentTools = b;
            
            return View();
        }

        [HttpPost]
        public IActionResult Iselm1(AssessmentTool assessmentTool)
        {
            if (ModelState.IsValid)
            {
                _context.assessmentTools.Add(assessmentTool);
                _context.SaveChanges();
                return RedirectToAction("DegerlendirmeAraclari", "Teacher");
            }
            return View();
        }

        [HttpPost]
        public IActionResult DegerlendirmeAraclariDelete(int DegerlendirmeAraclariId)
        {
            var DegerlendirmeAraclari = _context.assessmentTools.Find(DegerlendirmeAraclariId);
            if (DegerlendirmeAraclari == null)
            {
                return NotFound(); // veya uygun bir hata mesajı döndürün
            }
            else
            {
                _context.Remove(DegerlendirmeAraclari);
                _context.SaveChanges();
                return RedirectToAction("DegerlendirmeAraclari", "Teacher");
            }

        }
        [HttpPost]
        public IActionResult AltDegerlendirmeAraclariDelete(int AltDegerlendirmeAraclariId)
        {
            var AltDegerlendirmeAraclari = _context.subAssessmentTools.Find(AltDegerlendirmeAraclariId);
            if (AltDegerlendirmeAraclari == null)
            {
                return NotFound(); // veya uygun bir hata mesajı döndürün
            }
            else
            {
                _context.Remove(AltDegerlendirmeAraclari);
                _context.SaveChanges();
                return RedirectToAction("DegerlendirmeAraclari", "Teacher");
            }

        }


        // [HttpGet]
        // public IActionResult DegerlendirmeAraclariGuncelle(int id)
        // {

        //     var bul = _context.assessmentTools.FirstOrDefault(a => a.Id == id);
        //     return View(bul);

        // }

        [HttpPost]
        public IActionResult DegerlendirmeAraclariGuncelle(AssessmentTool assessmentTool)
        {

            var existingGida = _context.assessmentTools.SingleOrDefault(g => g.Id == assessmentTool.Id);

            if (existingGida != null)
            {
                existingGida.Title = assessmentTool.Title;
                existingGida.Percentage = assessmentTool.Percentage;

                _context.SaveChanges();

                return RedirectToAction("DegerlendirmeAraclari", "Teacher"); // veya başka bir yönlendirme
            }
            else
            {
                return NotFound(); // veya uygun bir hata işleme yöntemi
            }
        }
        [HttpPost]
        public IActionResult AltDegerlendirmeAraclariGuncelle(SubAssessmentTool subAssessmentTool)
        {

            var subAssessmentTools = _context.subAssessmentTools.SingleOrDefault(g => g.Id == subAssessmentTool.Id);

            if (subAssessmentTools != null)
            {
                subAssessmentTools.LowerRating = subAssessmentTool.LowerRating;
                subAssessmentTools.Point = subAssessmentTool.Point;

                _context.SaveChanges();

                return RedirectToAction("DegerlendirmeAraclari", "Teacher"); // veya başka bir yönlendirme
            }
            else
            {
                return NotFound(); // veya uygun bir hata işleme yöntemi
            }
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



        public IActionResult Deneme()
        {

            return View();
        }

        

        [HttpPost]
        public IActionResult Islem2(SubAssessmentTool learningOutcomes)
        {
            if (ModelState.IsValid)
            {
                _context.subAssessmentTools.Add(learningOutcomes);
                _context.SaveChanges();
                return RedirectToAction("DegerlendirmeAraclari", "Teacher");
            }
            return View();
        }


    }
}
