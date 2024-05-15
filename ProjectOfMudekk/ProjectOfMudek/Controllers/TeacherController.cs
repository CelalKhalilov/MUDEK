using System.Data.Common;
using Entities.Entities.Models;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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
            var isLoggedIn = HttpContext.Session.GetString("IsLoggedIn");
            if (isLoggedIn != "true")
            {
                return RedirectToAction("Ogretmen", "Home");
            }
            var userId = HttpContext.Session.GetInt32("Id");
            ViewBag.UserId = userId;
            var teach = _context.Teachers.ToList();
            ViewBag.teach = teach;
            var academicUnit = _context.academicUnits.ToList();
            ViewBag.academicUnit = academicUnit;
            // new SelectList(academicUnit, "Id", "UnitName");
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetFaculties(int academicUnitId)
        {
            var faculties = await _context.faculties
                .Where(f => f.AcademicUnitId == academicUnitId)
                .ToListAsync();

            return Json(faculties);
        }
        [HttpGet]
        public async Task<IActionResult> GetDepartments(int facultyId)
        {
            var departments = await _context.departments
                .Where(d => d.FacultyId == facultyId)
                .ToListAsync();

            return Json(departments);
        }
        [HttpGet]
        public async Task<IActionResult> GetPeriod(int periodId)
        {
            var periods = await _context.periods
                .Where(d => d.DepartmentId == periodId)
                .ToListAsync();

            return Json(periods);
        }
        [HttpGet]
        public async Task<IActionResult> GetLesson(int lessonId)
        {
            var lessons = await _context.lessons
                .Where(d => d.PeriodId == lessonId)
                .ToListAsync();

            return Json(lessons);
        }

        [HttpPost]
        public IActionResult Index(Form form)
        {
            if (ModelState.IsValid)
            {
                _context.forms.Add(form);
                _context.SaveChanges();
                return RedirectToAction("Tablo", "Teacher");
            }
            return View();
        }


        // [HttpPost]
        // public IActionResult GetDistricts(int cityId)
        // {
        //     var districts = _context.faculties.Where(d => d.AcademicUnitId == cityId).ToList();
        //     return Json(districts);
        // }

        

        public IActionResult Tablo()
        {
            var isLoggedIn = HttpContext.Session.GetString("IsLoggedIn");
            if (isLoggedIn != "true")
            {
                return RedirectToAction("Ogretmen", "Home");
            }
            var a = _context.learningOutcomess.ToList();
            var b = _context.lessons.ToList();
            var c = _context.forms.ToList();
            var d = _context.Teachers.ToList();
            var userId = HttpContext.Session.GetInt32("Id");
            ViewBag.UserId = userId;
            var teach = _context.Teachers.ToList();
            ViewBag.teach = teach;
            ViewBag.OutcomesList = a;
            ViewBag.lessons = b;
            ViewBag.forms = c;
            ViewBag.teachers = d;

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
            var isLoggedIn = HttpContext.Session.GetString("IsLoggedIn");
            if (isLoggedIn != "true")
            {
                return RedirectToAction("Ogretmen", "Home");
            }
            var userId = HttpContext.Session.GetInt32("Id");
            ViewBag.UserId = userId;
            var teach = _context.Teachers.ToList();
            ViewBag.teach = teach;
            var uploadedFiles = _context.uploadedFiles.ToList();
            ViewBag.UploadedFiles = uploadedFiles;

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError("file", "Please select a file to upload.");
                return View();
            }

            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                var uploadedFile = new UploadedFile
                {
                    FileName = file.FileName,
                    ContentType = file.ContentType,
                    Data = memoryStream.ToArray()
                };

                _context.uploadedFiles.Add(uploadedFile);
                await _context.SaveChangesAsync();
                return RedirectToAction("Upload", "Teacher");
            }
        }


        [HttpGet]
        public async Task<IActionResult> Download(int fileId)
        {
            var file = await _context.uploadedFiles.FindAsync(fileId);
            if (file == null)
            {
                return NotFound();
            }

            return File(file.Data, file.ContentType, file.FileName);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int fileId)
        {
            var file = await _context.uploadedFiles.FindAsync(fileId);
            if (file != null)
            {
                _context.uploadedFiles.Remove(file);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Upload");
        }

        


        public IActionResult DegerlendirmeAraclari()
        {
            var isLoggedIn = HttpContext.Session.GetString("IsLoggedIn");
            if (isLoggedIn != "true")
            {
                return RedirectToAction("Ogretmen", "Home");
            }
            var userId = HttpContext.Session.GetInt32("Id");
            ViewBag.UserId = userId;
            var teach = _context.Teachers.ToList();
            ViewBag.teach = teach;
            var a = _context.assessmentTools.ToList();
            var b = _context.subAssessmentTools.ToList();
            ViewBag.assessmentTools = a;
            ViewBag.subAssessmentTools = b;

            return View();
        }

        #region Islem1
            
        // [HttpPost]
        // public IActionResult Iselm1(AssessmentTool assessmentTool)
        // {
        //     int sum =+ assessmentTool.Percentage;
        //     HttpContext.Session.SetInt32("Percentage", sum);
        //     var tut = HttpContext.Session.GetInt32("Percentage");
        //     if (tut > 100)
        //     {
        //         throw new Exception();
        //     }
        //     else
        //     {
        //         if (ModelState.IsValid)
        //         {
        //             _context.assessmentTools.Add(assessmentTool);
        //             _context.SaveChanges();
        //             return RedirectToAction("DegerlendirmeAraclari", "Teacher", new { id = assessmentTool.Id });
        //         }
        //         return View();
        //     }
        // }

        #endregion

        [HttpPost]
        public IActionResult Iselm1(AssessmentTool assessmentTool)
        {
            if (!_context.assessmentTools.Any())
            {
                TempData["sumu"] = 0;
            }

            int sumu = TempData.ContainsKey("sumu") ? Convert.ToInt32(TempData["sumu"]) : 0;

            sumu += assessmentTool.Percentage;

            if (sumu > 100)
            {
                TempData["ErrorMessage"] = "Değerler aralığı aşıldı.";
                return RedirectToAction("DegerlendirmeAraclari", "Teacher", new { id = assessmentTool.Id });
            }

            if (!ModelState.IsValid)
            {
                return View();
            }

            TempData["sumu"] = sumu;

            _context.assessmentTools.Add(assessmentTool);
            _context.SaveChanges();

            return RedirectToAction("DegerlendirmeAraclari", "Teacher", new { id = assessmentTool.Id });
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

            var a = _context.assessmentTools.ToList();
            var b = _context.subAssessmentTools.ToList();
            var c = _context.learningOutcomess.ToList();
            var d = _context.students.ToList();
            var e = _context.questions.ToList();
            var categories = _context.assessmentTools.ToList();
            ViewBag.Categories = new SelectList(categories, "Id", "Title");
            var userId = HttpContext.Session.GetInt32("Id");
            ViewBag.UserId = userId;
            var teach = _context.Teachers.ToList();
            ViewBag.teach = teach;
            ViewBag.assessmentTools = a;
            ViewBag.subAssessmentTools = b;
            ViewBag.learningOutcomess = c;
            ViewBag.students = d;
            ViewBag.questions = e;
            return View();
        }

        


        [HttpPost]
        public IActionResult HesaplamalarStudentAdd(Student student)
        {
            if (ModelState.IsValid)
            {
                _context.students.Add(student);
                _context.SaveChanges();

                return RedirectToAction("Hesaplamalar", "Teacher");
            }
            return View();
        }

        [HttpPost]
        public IActionResult HesaplamalarQuestionAdd(Question question)
        {
            if (ModelState.IsValid)
            {
                _context.questions.Add(question);
                _context.SaveChanges();

                return RedirectToAction("Hesaplamalar", "Teacher");
            }
            return View();
        }

        [HttpPost]
        public IActionResult HesaplamalarQuestionAdd2(List<Question> question)
        {
            // null olmayan inputları filtreleyin
            var validEntries = question.Where(m => m.Note.HasValue).ToList();
            if (validEntries.Any())
            {

                _context.questions.AddRange(validEntries);
                _context.SaveChanges();

                return RedirectToAction("Hesaplamalar", "Teacher");
            }
            return View();
        }

        #region profil ayarlari
            
        
        // public IActionResult ProfilAyarlari()
        // {
        //     var isLoggedIn = HttpContext.Session.GetString("IsLoggedIn");
        //     if (isLoggedIn != "true")
        //     {
        //         return RedirectToAction("Ogretmen", "Home");
        //     }
        //     var userId = HttpContext.Session.GetInt32("Id");
        //     ViewBag.UserId = userId;
            
        //     var a = _context.Teachers.ToList();
        //     ViewBag.teachers = a;
        //     return View();
        // }

        // [HttpPost]
        // public IActionResult ProfilAyarlariGuncelle(Teacher teacher)
        // {

        //     var teachers = _context.Teachers.SingleOrDefault(g => g.Id == teacher.Id);

        //     if (teachers != null)
        //     {
        //         teachers.FirstName = teacher.FirstName;
        //         teachers.LastName = teacher.LastName;
        //         teachers.Gmail = teacher.Gmail;
        //         teachers.ProfileImage = teacher.ProfileImage;

        //         _context.SaveChanges();

        //         return RedirectToAction("ProfilAyarlari", "Teacher", new { id = teacher.Id }); // veya başka bir yönlendirme
        //     }
        //     else
        //     {
        //         return NotFound(); // veya uygun bir hata işleme yöntemi
        //     }
        // }

        #endregion

        public IActionResult ProfilAyarlari()
        {
            var isLoggedIn = HttpContext.Session.GetString("IsLoggedIn");
            if (isLoggedIn != "true")
            {
                return RedirectToAction("Ogretmen", "Home");
            }
            var userId = HttpContext.Session.GetInt32("Id");
            ViewBag.UserId = userId;
            var teach = _context.Teachers.ToList();
            ViewBag.teach = teach;
            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ProfilAyarlariGuncelle(Teacher teach, IFormFile file)
        {
            var existingTeach = _context.Teachers.SingleOrDefault(g => g.Id == teach.Id);

            if (existingTeach != null)
            {
                existingTeach.FirstName = teach.FirstName;
                existingTeach.LastName = teach.LastName;
                existingTeach.Gmail = teach.Gmail;

                if (file != null && file.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await file.CopyToAsync(memoryStream);
                        existingTeach.Image = memoryStream.ToArray();
                    }
                }

                _context.SaveChanges();

                return RedirectToAction("ProfilAyarlari", "Teacher");
            }
            else
            {
                return NotFound();
            }
        }


        public IActionResult Deneme()
        {
            var a = _context.assessmentTools.ToList();
            var b = _context.subAssessmentTools.ToList();
            var userId = HttpContext.Session.GetInt32("Id");
            ViewBag.UserId = userId;
            // var tut = HttpContext.Session.GetString("Title");
            // ViewBag.tutuyor = tut;
            var categories = _context.assessmentTools.ToList();
            ViewBag.Categorie = new SelectList(categories, "Id", "Title");
            ViewBag.assessmentTools = a;
            ViewBag.subAssessmentTools = b;

            return View();
        }

        [HttpPost]
        public IActionResult Deneme(int selectedCategoryId)
        {
            var categories = _context.assessmentTools.ToList();
            ViewBag.Categories = new SelectList(categories, "Id", "Title");

            // Seçilen kategoriyi ViewBag'e kaydedin
            ViewBag.SelectedCategory = categories.FirstOrDefault(c => c.Id == selectedCategoryId)?.Title;
            
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
