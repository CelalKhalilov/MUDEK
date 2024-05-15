using Entities.Entities.Models;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProjectOfMudek.Context;
using ProjectOfMudek.Models;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;

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
                int userId = user.Id;
                HttpContext.Session.SetInt32("Id", userId);
                // Başarılı giriş, "/Admin/Index" sayfasına yönlendir
                HttpContext.Session.SetString("IsLoggedIn", "true");
                return RedirectToAction("Index", "Teacher");

                
            }
            else
            {
                // Hatalı giriş, sayfada bir hata mesajı gösterilebilir veya farklı bir işlem yapılabilir
                // ViewBag.Error = "Invalid username or password.";
                TempData["ErrorMessage"] = "Invalid username or password..";
                return RedirectToAction("Ogretmen");
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

        // =================================================================

        // private static Dictionary<string, string> verificationCodes = new Dictionary<string, string>();

        // Ana sayfa - Gmail giriş formu
        // [HttpGet]
        // public ActionResult Index()
        // {
        //     return View();
        // }

        // [HttpPost]
        // public ActionResult Index(string email)
        // {
        //     // Rastgele 6 basamaklı kod oluştur
        //     string randomCode = new Random().Next(100000, 999999).ToString();

        //     // Kodu kullanıcıya e-posta ile gönder
        //     SendVerificationCode(email, randomCode);

        //     // Store the verification code in the dictionary
        //     verificationCodes[email] = randomCode;

        //     // Oturum açma sayfasına yönlendir
        //     return RedirectToAction("Login", new { email = email });
        // }

        // // Oturum açma sayfası - Kodu girme formu
        // [HttpGet]
        // public ActionResult Login(string email)
        // {
        //     ViewBag.Email = email;
        //     return View();
        // }

        // public IActionResult Success()
        // {
        //     return View();
        // }
        // [HttpPost]
        // public ActionResult Login(string email, string code)
        // {
        //     // Girilen kodu doğrula
        //     if (VerifyCode(email, code))
        //     {
        //         // return Content("Başarıyla oturum açıldı!");
        //         return RedirectToAction("Success");
        //     }
        //     else
        //     {
        //         return Content("Kod yanlış, tekrar deneyin.");
        //     }
        // }

        // private void SendVerificationCode(string email, string code)
        // {
        //     string senderEmail = "9221118072@samsun.edu.tr"; // Gönderen Gmail adresi
        //     string senderPassword = "Cll353180?"; // Gönderen Gmail şifresi

        //     using (MailMessage message = new MailMessage(senderEmail, email))
        //     {
        //         message.Subject = "Doğrulama Kodu";
        //         message.Body = $"Oturum açma için doğrulama kodunuz: {code}";

        //         using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587))
        //         {
        //             smtpClient.EnableSsl = true;
        //             smtpClient.Credentials = new NetworkCredential(senderEmail, senderPassword);
        //             smtpClient.Send(message);
        //         }
        //     }
        // }

        // private bool VerifyCode(string email, string userCode)
        // {
        //     // Dictionary'den eşleşen kodu al
        //     string code;
        //     if (verificationCodes.TryGetValue(email, out code))
        //     {
        //         return userCode == code;
        //     }
        //     else
        //     {
        //         return false;
        //     }
        // }
        
    }
}
