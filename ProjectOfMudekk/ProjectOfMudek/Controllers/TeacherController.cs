﻿using System.Data.Common;
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
            ViewBag.assessmentTools = _context.assessmentTools.ToList();
           

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file,string category)
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
                    Category = category,
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
            ViewBag.question = _context.questions.ToList();



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
            var userId = HttpContext.Session.GetInt32("Id");
            
            // Daha önce böyle bir kullanıcı var mı?
            if (_context.assessmentTools.Any(at => at.TeacherId == userId))
            {
                int totalPercentage = _context.assessmentTools
                                        .Where(at => at.TeacherId == userId )
                                        .Sum(at => at.Percentage);
                totalPercentage += assessmentTool.Percentage;
                if (totalPercentage <= 100)
                {
                    _context.assessmentTools.Add(assessmentTool);
                    _context.SaveChanges();
                }
                else
                {
                    TempData["ErrorMessage"] = "Değerler aralığı aşıldı.";
                    return RedirectToAction("DegerlendirmeAraclari", "Teacher", new { id = assessmentTool.Id });
                }
            }
            else
            {
                _context.assessmentTools.Add(assessmentTool);
                _context.SaveChanges();
            }
            return RedirectToAction("DegerlendirmeAraclari", "Teacher", new { id = assessmentTool.Id });
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
            var userId = HttpContext.Session.GetInt32("Id");
            ViewBag.UserId = userId;
            var categories = _context.assessmentTools.Where( a => a.TeacherId == userId).ToList();
            ViewBag.Categories = new SelectList(categories, "Id", "Title");


            

    
            

            if (userId != null)
            {
                var assessmentTools = _context.assessmentTools.Where(k => k.TeacherId == userId).ToList();
                ViewBag.AssessmentTools = assessmentTools;
                var subAssessmentTool = _context.subAssessmentTools.Where(k => k.TeacherId == userId).ToList();
                ViewBag.subAssessmentTools = subAssessmentTool;
                var learningOutcome = _context.learningOutcomess.Where(k => k.TeacherId == userId).ToList();
                ViewBag.learningOutcomess = learningOutcome;
                var student = _context.students.Where(k => k.TeacherId == userId).ToList();
                ViewBag.students = student;
                var question = _context.questions.ToList();
                ViewBag.questions = question;
                var teachers = _context.Teachers.ToList();
                ViewBag.teach = teachers;
                



            }
            else
            {
                ViewBag.AssessmentTools = new List<AssessmentTool>();
            }

            // ViewBag.assessmentTools = a;
            // ViewBag.subAssessmentTools = b;
            // ViewBag.learningOutcomess = c;
            // ViewBag.students = d;
            // ViewBag.questions = e;
            
            return View();
        }


        [HttpPost]
        public IActionResult Hesaplamalar(int selectedCategoryId)
        {
            var userId = HttpContext.Session.GetInt32("Id");
            ViewBag.UserId = userId;

            var categories = _context.assessmentTools.Where( a => a.TeacherId == userId).ToList();
            ViewBag.Categories = new SelectList(categories, "Id", "Title");


            if (userId != null)
            {
                var assessmentTools = _context.assessmentTools.Where(k => k.TeacherId == userId).ToList();
                ViewBag.AssessmentTools = assessmentTools;
                var subAssessmentTool = _context.subAssessmentTools.Where(k => k.TeacherId == userId).ToList();
                ViewBag.subAssessmentTools = subAssessmentTool;
                var learningOutcome = _context.learningOutcomess.Where(k => k.TeacherId == userId).ToList();
                ViewBag.learningOutcomess = learningOutcome;
                var student = _context.students.Where(k => k.TeacherId == userId).ToList();
                ViewBag.students = student;
                var question = _context.questions.ToList();
                ViewBag.questions = question;
                var teachers = _context.Teachers.ToList();
                ViewBag.teach = teachers;
            }
            else
            {
                ViewBag.AssessmentTools = new List<AssessmentTool>();
            }

            // Seçilen kategoriyi ViewBag'e kaydedin
            ViewBag.SelectedCategory = categories.FirstOrDefault(c => c.Id == selectedCategoryId)?.Title;

            TempData["ShowModal"] = true;
            
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

        #region Deneme
            
        // public IActionResult Deneme()
        // {
        //     // var a = _context.assessmentTools.ToList();
        //     var b = _context.subAssessmentTools.ToList();
        //     var userId = HttpContext.Session.GetInt32("Id");
        //     ViewBag.UserId = userId;
        //     var categories = _context.assessmentTools.ToList();
        //     ViewBag.Categories = new SelectList(categories, "Id", "Title");

        //     if (userId != null)
        //     {
        //         var assessmentTools = _context.assessmentTools.Where(k => k.TeacherId == userId).ToList();
        //         ViewBag.AssessmentTools = assessmentTools;
        //     }
        //     else
        //     {
        //         ViewBag.AssessmentTools = new List<AssessmentTool>();
        //     }
        //     ViewBag.subAssessmentTools = b;

        //     return View();
        // }

        // [HttpPost]
        // public IActionResult Deneme(int selectedCategoryId)
        // {
        //     var categories = _context.assessmentTools.ToList();
        //     ViewBag.Categories = new SelectList(categories, "Id", "Title");

        //     // Seçilen kategoriyi ViewBag'e kaydedin
        //     ViewBag.SelectedCategory = categories.FirstOrDefault(c => c.Id == selectedCategoryId)?.Title;
            
        //     return View();

        // }

        #endregion
        

        public IActionResult Deneme()
        {
            var userId = HttpContext.Session.GetInt32("Id");
            ViewBag.UserId = userId;
            
            // Assessment Tools
            var categories = _context.assessmentTools.ToList();
            ViewBag.Categories = new SelectList(categories, "Id", "Title");

            if (userId != null)
            {
                var assessmentTools = _context.assessmentTools.Where(k => k.TeacherId == userId).ToList();
                ViewBag.AssessmentTools = assessmentTools;
            }
            else
            {
                ViewBag.AssessmentTools = new List<AssessmentTool>();
            }

            // Sub Assessment Tools
            var subAssessmentTools = _context.subAssessmentTools.ToList();
            ViewBag.SubAssessmentTools = subAssessmentTools;

            return View();
        }

    [HttpPost]
    public IActionResult Deneme(int selectedCategoryId)
    {
        var userId = HttpContext.Session.GetInt32("Id");
        ViewBag.UserId = userId;
        
        // Assessment Tools
        var categories = _context.assessmentTools.ToList();
        ViewBag.Categories = new SelectList(categories, "Id", "Title");

        if (userId != null)
        {
            var assessmentTools = _context.assessmentTools.Where(k => k.TeacherId == userId).ToList();
            ViewBag.AssessmentTools = assessmentTools;
        }
        else
        {
            ViewBag.AssessmentTools = new List<AssessmentTool>();
        }

        // Sub Assessment Tools
        var subAssessmentTools = _context.subAssessmentTools.ToList();
        ViewBag.SubAssessmentTools = subAssessmentTools;

        // Seçilen kategoriyi ViewBag'e kaydedin
        ViewBag.SelectedCategory = categories.FirstOrDefault(c => c.Id == selectedCategoryId)?.Title;
        
        return View();
    }


        [HttpPost]
        public IActionResult Islem2(SubAssessmentTool subAssessmentTool)
        {
            var userId = HttpContext.Session.GetInt32("Id");

            // Daha önce böyle bir kullanıcı var mı?
            if (_context.subAssessmentTools.Any(at => at.TeacherId == userId))
            {
                int totalPercentage = _context.subAssessmentTools
                                        .Where(at => at.TeacherId == userId && at.Title == subAssessmentTool.Title)
                                        .Sum(at => at.Point);
                totalPercentage += subAssessmentTool.Point;
                if (totalPercentage <= 100)
                {
                    _context.subAssessmentTools.Add(subAssessmentTool);
                    _context.SaveChanges();
                }
                else
                {
                    TempData["ErrorMessage"] = "Değerler aralığı aşıldı.";
                    return RedirectToAction("DegerlendirmeAraclari", "Teacher");
                }
                
            }
            else
            {
                _context.subAssessmentTools.Add(subAssessmentTool);
                _context.SaveChanges();
            }
            return RedirectToAction("DegerlendirmeAraclari", "Teacher", new { id = subAssessmentTool.Id });
        }


        // [HttpPost]
        // public IActionResult Islem2(SubAssessmentTool subAssessmentTool)
        // {
        //     if (!_context.assessmentTools.Any())
        //     {
        //         TempData["sumu2"] = 0;
        //     }

        //     int sumu2 = TempData.ContainsKey("sumu2") ? Convert.ToInt32(TempData["sumu2"]) : 0;

        //     sumu2 += subAssessmentTool.Point;

        //     if (sumu2 > 100)
        //     {
        //         TempData["ErrorMessage"] = "Değerler aralığı aşıldı.";
        //         return RedirectToAction("DegerlendirmeAraclari", "Teacher", new { id = subAssessmentTool.Id });
        //     }

        //     if (!ModelState.IsValid)
        //     {
        //         return View();
        //     }

        //     TempData["sumu2"] = sumu2;

        //     _context.subAssessmentTools.Add(subAssessmentTool);
        //     _context.SaveChanges();

        //     return RedirectToAction("DegerlendirmeAraclari", "Teacher", new { id = subAssessmentTool.Id });
        // }



        // Öğrencinin notlarını JSON formatında dönen metod
        [HttpGet]
        public async Task<IActionResult> GetStudentNotes(int studentId)
        {
            var notes = _context.questions
                        .Where(q => q.StudentId == studentId)
                        .Select(q => new
                        {
                            q.Id,
                            q.LowerRating,
                            q.Note
                        })
                        .ToList();
            return Json(notes);
        }


        // Güncelleme işlemini gerçekleştiren POST metodu
        [HttpPost]
        public async Task<IActionResult> Edit(int studentId, List<Question> updatedQuestions)
        {
            var question = await _context.questions
                                          .Where(q => q.StudentId == studentId)
                                          .ToListAsync();

            foreach (var questio in question)
            {
                var updatedQuestion = updatedQuestions.FirstOrDefault(uq => uq.Id == questio.Id);
                if (updatedQuestion != null)
                {
                    questio.Note = updatedQuestion.Note;
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Hesaplamalar","Teacher"); // Güncelleme sonrası yönlendirme
        }

        [HttpGet]
        public IActionResult GetQuestionsByStudentId(int studentId)
        {
            var questions = _context.questions
                                    .Where(q => q.StudentId == studentId)
                                    .Select(q => new { q.Id, q.Note })
                                    .ToList();
            
            return Json(questions);
        }


        [HttpPost]
        public IActionResult HesaplaGuncelle(List<Question> questionss)
        {
            if (questionss == null)
            {
                // Log veya hata mesajı yazdırın
                Console.WriteLine("questionss is null");
                // veya bir hata sayfasına yönlendirin
                return BadRequest("questionss is null");
            }
            
            if (!questionss.Any())
            {
                // Log veya hata mesajı yazdırın
                Console.WriteLine("questionss is empty");
                // veya bir hata sayfasına yönlendirin
                return BadRequest("questionss is empty");
            }

            foreach (var question in questionss)
            {
                var existingQuestion = _context.questions.FirstOrDefault(q => q.Id == question.Id);
                if (existingQuestion != null)
                {
                    existingQuestion.Note = question.Note;
                    _context.Update(existingQuestion);
                }
            }
            _context.SaveChanges();
            return RedirectToAction("Hesaplamalar");
        }

        [HttpPost]
        public IActionResult Sil(int studentId)
        {
            // Öğrencinin notlarını al ve sil
            var studentNotes = _context.questions.Where(n => n.StudentId == studentId).ToList();
            if (studentNotes.Any())
            {
                _context.questions.RemoveRange(studentNotes);
                _context.SaveChanges();
            }

            // Öğrenci bilgisini sil (opsiyonel)
            var student = _context.students.FirstOrDefault(s => s.Id == studentId);
            if (student != null)
            {
                _context.students.Remove(student);
                _context.SaveChanges();
            }

            return RedirectToAction("Hesaplamalar");
        }



        public IActionResult Sohbet([FromQuery] int senderId)
        {
            HttpContext.Session.SetInt32("SenderId", senderId); // Session'a kaydediyoruz
            ViewBag.SenderId = senderId;

            var userId = HttpContext.Session.GetInt32("Id");
            ViewBag.UserId = userId;

            var teach = _context.Teachers.ToList();
            ViewBag.teach = teach;

            // Alıcı ve gönderici arasındaki mesajları al
            var messages = _context.messages
                .Where(m => (m.TeacherId == userId && m.SenderId == senderId) || (m.TeacherId == senderId && m.SenderId == userId))
                .ToList();

            // Kullanıcı alıcıysa (TeacherId == userId), gelen yeni mesajları güncelleyin
            if (userId == senderId)
            {
                foreach (var message in messages.Where(m => m.TeacherId == userId && m.IsNew))
                {
                    message.IsNew = false;
                }
                _context.SaveChanges();
            }

            ViewBag.message = messages;
            return View();
        }

        

        [HttpPost]
        public IActionResult Sohbet(string chat,int teacherId)
        {
            var senderId = HttpContext.Session.GetInt32("SenderId"); // Session'dan çekiyoruz
            
            if (string.IsNullOrEmpty(chat))
            {
                return BadRequest("Message content cannot be empty.");
            }

            var message = new Message
            {
                Chat = chat,
                TeacherId = teacherId,
                SenderId = senderId ?? 0,
                SentDate = DateTime.Now,
                IsNew = true  // Yeni mesajlar
            };

            _context.messages.Add(message);
            _context.SaveChanges();

            return RedirectToAction("Sohbet","Teacher",new { senderId = senderId });

        }


        public IActionResult Kisiler()
        {
            var userId = HttpContext.Session.GetInt32("Id");
            ViewBag.UserId = userId;
            var teach = _context.Teachers.ToList();
            var messages = _context.messages.ToList();
            ViewBag.teach = teach;
            ViewBag.message = messages;
            return View();
        }

        public IActionResult AnaSayfa()
        {
            var userId = HttpContext.Session.GetInt32("Id");
            ViewBag.UserId = userId;
            ViewBag.teach = _context.Teachers.ToList();
            ViewBag.forms = _context.forms.ToList();
            ViewBag.lessons = _context.lessons.ToList();
            return View();
        }
    }
}
