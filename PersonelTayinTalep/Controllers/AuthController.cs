using Business.Abstract;
using Core.DTOs;
using Core.Utilities.Security;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace PersonelTayinTalep.Controllers
{
    public class AuthController : Controller
    {
        private readonly IPersonelService _personelService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IPersonelService personelService, ILogger<AuthController> logger)
        {
            _personelService = personelService;
            _logger = logger;
        }

        //Adminsicil no:999999
        //şifre:Admin123


        //bir seferlik admin oluşturmak için kullandım
        //[HttpGet]
        //public async Task<IActionResult> CreateAdmin()
        //{
        //string sifre = "Admin123";
        //PasswordHelper.CreatePasswordHash(sifre, out byte[] hash, out byte[] salt);

        //var admin = new Personel
        //  {
        //SicilNo = "317740",
        //Adi = "Admin",
        //Soyadi = "Kullanıcı",
        //Unvan = "Yönetici",
        //SifreHash = hash,
        //SifreSalt = salt,
        //IsAdmin = true
        //};

        //        await _personelService.AddAsync(admin);

        //          return Content("Admin oluşturuldu: SicilNo=999999, Şifre=Admin123");
        //}

        [HttpGet]
        public IActionResult Login()
        {
            _logger.LogInformation("Giriş ekranı açıldı.");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Geçersiz giriş denemesi. ModelState geçersiz.");
                return View(model);
            }

            var personel = await _personelService.GetBySicilNoAsync(model.SicilNo);
            if (personel == null || !PasswordHelper.VerifyPasswordHash(model.Sifre, personel.SifreHash, personel.SifreSalt))
            {
                _logger.LogWarning("Geçersiz giriş denemesi. SicilNo: {SicilNo}", model.SicilNo);
                ModelState.AddModelError(string.Empty, "Geçersiz sicil numarası veya şifre.");
                return View(model);
            }

            HttpContext.Session.SetInt32("PersonelId", personel.Id);
            HttpContext.Session.SetString("IsAdmin", personel.IsAdmin ? "true" : "false");
            HttpContext.Session.SetString("KullaniciAdi", $"{personel.Adi} {personel.Soyadi}");

            _logger.LogInformation("Kullanıcı giriş yaptı: {AdSoyad} - Sicil: {SicilNo}", personel.Adi + " " + personel.Soyadi, personel.SicilNo);

            if (personel.IsAdmin)
                return RedirectToAction("AllAppointment", "Admin");

            return RedirectToAction("Index", "Personel");
        }

        public IActionResult Logout()
        {
            var adSoyad = HttpContext.Session.GetString("KullaniciAdi") ?? "Bilinmeyen Kullanıcı";
            _logger.LogInformation("Kullanıcı çıkış yaptı: {Kullanici}", adSoyad);

            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
