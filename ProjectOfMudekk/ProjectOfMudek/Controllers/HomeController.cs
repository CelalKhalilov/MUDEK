using Entities.Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProjectOfMudek.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;

namespace ProjectOfMudek.Controllers
{
    public class HomeController : Controller
    {
        private readonly MudekContext _context;
        private static Dictionary<string, string> verificationCodes = new Dictionary<string, string>();

        public HomeController(MudekContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var academicUnits = _context.academicUnits.ToList();
            return View(academicUnits);
        }

        public IActionResult Ogretmen()
        {
            return View();
        }

        public IActionResult Dogrulama(string email)
        {
            ViewBag.Email = email;
            return View();
        }

        [HttpPost]
        public IActionResult OgretmenRegister(Teacher teacher)
        {
            if (ModelState.IsValid)
            {
                // Rastgele 6 basamaklı kod oluştur
                string randomCode = new Random().Next(100000, 999999).ToString();

                // Kodu kullanıcıya e-posta ile gönder
                SendVerificationCode(teacher.Gmail, randomCode);

                // Doğrulama kodunu dictionary'ye ekle
                verificationCodes[teacher.Gmail] = randomCode;

                // Öğretmen bilgilerini geçici olarak sakla
                TempData["Teacher"] = JsonConvert.SerializeObject(teacher);
                TempData["VerificationEmail"] = teacher.Gmail;

                // Doğrulama sayfasına yönlendir
                return RedirectToAction("Dogrulama", new { email = teacher.Gmail });
            }
            return View("Ogretmen");
        }

        [HttpPost]
        public IActionResult OgretmenLogin(Teacher teacher)
        {
            var user = _context.Teachers.FirstOrDefault(a => a.Gmail == teacher.Gmail && a.Password == teacher.Password);
            if (user != null)
            {
                int userId = user.Id;
                HttpContext.Session.SetInt32("Id", userId);
                HttpContext.Session.SetString("IsLoggedIn", "true");
                return RedirectToAction("Index", "Teacher");
            }
            else
            {
                TempData["ErrorMessage"] = "Invalid username or password.";
                return RedirectToAction("Ogretmen");
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Ogretmen", "Home");
        }

        [HttpPost]
        public IActionResult Dogrulama(string email, string code)
        {
            if (VerifyCode(email, code))
            {
                if (TempData["Teacher"] != null)
                {
                    var teacher = JsonConvert.DeserializeObject<Teacher>(TempData["Teacher"].ToString());
                    _context.Teachers.Add(teacher);
                    _context.SaveChanges();

                    HttpContext.Session.SetInt32("Id", teacher.Id);
                    HttpContext.Session.SetString("IsLoggedIn", "true");

                    return RedirectToAction("Ogretmen", "Home");
                }
                return RedirectToAction("Success");
            }
            else
            {
                ViewBag.Email = email;
                // TempData["ErrorMessage"] = "Kod yanlış, tekrar deneyin.";
                ViewBag.Error = "Kod yanlış, tekrar deneyin.";
                return View();
            }
        }

        private void SendVerificationCode(string email, string code)
        {
            string senderEmail = "9221118072@samsun.edu.tr";
            string senderPassword = "Şifre Gir";

            using (MailMessage message = new MailMessage(senderEmail, email))
            {
                message.Subject = "Doğrulama Kodu";
                message.Body = $"Oturum açma için doğrulama kodunuz: {code}";

                using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtpClient.EnableSsl = true;
                    smtpClient.Credentials = new NetworkCredential(senderEmail, senderPassword);
                    smtpClient.Send(message);
                }
            }
        }

        private bool VerifyCode(string email, string userCode)
        {
            return verificationCodes.TryGetValue(email, out string code) && userCode == code;
        }
    }
}
