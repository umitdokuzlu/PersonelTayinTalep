using Business.Abstract;
using ClosedXML.Excel;
using Core.DTOs;
using Core.Utilities.Security;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PersonelTayinTalep.Filters;
using PersonelTayinTalep.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PersonelTayinTalep.Controllers
{
    [AdminAuthorize]
    public class AdminController : Controller
    {
        private readonly IPersonelService _personelService;
        private readonly ITayinTalepService _tayinTalepService;
        private readonly ITayinTalepTercihService _tercihService;
        private readonly IAdliyeService _adliyeService;
        private readonly ILogger<AdminController> _logger;

        public AdminController(IPersonelService personelService,
            ITayinTalepService tayinTalepService,
            ITayinTalepTercihService tercihService,
            IAdliyeService adliyeService,
            ILogger<AdminController> logger)
        {
            _personelService = personelService;
            _tayinTalepService = tayinTalepService;
            _tercihService = tercihService;
            _adliyeService = adliyeService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Admin paneli - personel listesi görüntüleniyor.");
            try
            {
                var tumPersoneller = await _personelService.GetAllAsync();
                var adminId = HttpContext.Session.GetInt32("PersonelId");

                var personeller = tumPersoneller
                    .Where(p => !p.IsAdmin && p.Id != adminId)
                    .ToList();

                var viewModel = new List<AdminPersonelViewModel>();

                foreach (var p in personeller)
                {
                    var talepler = await _tayinTalepService.GetByPersonelIdAsync(p.Id);
                    var sonTalep = talepler.OrderByDescending(x => x.BasvuruTarihi).FirstOrDefault();

                    var tercihler = new List<string> { "Yapmadı", "Yapmadı", "Yapmadı" };

                    if (sonTalep != null)
                    {
                        var talepTercihler = await _tercihService.GetByTayinTalepIdAsync(sonTalep.Id);
                        foreach (var tercih in talepTercihler)
                        {
                            var adliye = await _adliyeService.GetByIdAsync(tercih.AdliyeId);
                            if (tercih.Sira >= 1 && tercih.Sira <= 3)
                                tercihler[tercih.Sira - 1] = adliye.Ad;
                        }
                    }

                    viewModel.Add(new AdminPersonelViewModel
                    {
                        Id = p.Id,
                        SicilNo = p.SicilNo,
                        AdSoyad = $"{p.Adi} {p.Soyadi}",
                        Unvan = p.Unvan,
                        Tercih1 = tercihler[0],
                        Tercih2 = tercihler[1],
                        Tercih3 = tercihler[2]
                    });
                }

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Admin paneli - personel listesi yüklenirken hata oluştu.");
                return View("Hata");
            }
        }

        public async Task<IActionResult> AllAppointment()
        {
            _logger.LogInformation("Tüm tayin talepleri görüntüleniyor.");
            try
            {
                var talepler = await _tayinTalepService.GetAllAsync();
                var viewModelList = new List<AdminTayinTalepViewModel>();

                foreach (var talep in talepler)
                {
                    var personel = await _personelService.GetByIdAsync(talep.PersonelId);
                    var tercihler = await _tercihService.GetByTayinTalepIdAsync(talep.Id);

                    var tercihList = new List<string>();
                    foreach (var tercih in tercihler.OrderBy(t => t.Sira))
                    {
                        var adliye = await _adliyeService.GetByIdAsync(tercih.AdliyeId);
                        tercihList.Add(adliye?.Ad ?? "Bilinmiyor");
                    }

                    viewModelList.Add(new AdminTayinTalepViewModel
                    {
                        TalepId = talep.Id,
                        AdSoyad = $"{personel.Adi} {personel.Soyadi}",
                        SicilNo = personel.SicilNo,
                        Unvan = personel.Unvan,
                        TalepTuru = talep.TalepTuru,
                        Aciklama = talep.Aciklama,
                        BasvuruTarihi = talep.BasvuruTarihi,
                        Tercihler = tercihList
                    });
                }

                return View("TayinListe", viewModelList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Tayin talepleri listesi yüklenirken hata oluştu.");
                return View("Hata");
            }
        }

        [HttpGet]
        public IActionResult CreatePersonel()
        {
            _logger.LogInformation("Yeni personel oluşturma sayfası açıldı.");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreatePersonel(CreatePersonelViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                PasswordHelper.CreatePasswordHash(model.Sifre, out byte[] sifreHash, out byte[] sifreSalt);

                var yeniPersonel = new Personel
                {
                    SicilNo = model.SicilNo,
                    Adi = model.Adi,
                    Soyadi = model.Soyadi,
                    Unvan = model.Unvan,
                    SifreHash = sifreHash,
                    SifreSalt = sifreSalt,
                    IsAdmin = model.IsAdmin
                };

                await _personelService.AddAsync(yeniPersonel);

                _logger.LogInformation("Yeni personel oluşturuldu: {SicilNo}", model.SicilNo);
                TempData["BasariMesaji"] = "Personel başarılı bir şekilde kaydedildi.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Yeni personel oluşturulurken hata oluştu.");
                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _personelService.DeleteAsync(id);
                _logger.LogInformation("Personel silindi. ID: {Id}", id);
                TempData["BasariMesaji"] = "Personel başarılı bir şekilde Silindi.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Personel silinirken hata oluştu. ID: {Id}", id);
                return View("Hata");
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteTayinTalep(int id)
        {
            try
            {
                await _tayinTalepService.DeleteAsync(id);
                _logger.LogInformation("Tayin talebi silindi. ID: {Id}", id);
                TempData["BasariMesaji"] = "Tayin talebi başarılı bir şekilde silindi";
                return RedirectToAction("AllAppointment", "Admin");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Tayin talebi silinirken hata oluştu. ID: {Id}", id);
                return View("Hata");
            }
        }

        [HttpGet]
        public async Task<IActionResult> ExportTayinTalepleriToExcel()
        {
            _logger.LogInformation("Tayin talepleri Excel dışa aktarımı başlatıldı.");
            try
            {
                var talepler = await _tayinTalepService.GetAllWithDetailsAsync();

                using var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add("Tayin Talepleri");

                worksheet.Cell(1, 1).Value = "ADI SOYADI";
                worksheet.Cell(1, 2).Value = "SİCİL NO";
                worksheet.Cell(1, 3).Value = "UNVAN";
                worksheet.Cell(1, 4).Value = "TALEP TÜRÜ";
                worksheet.Cell(1, 5).Value = "AÇIKLAMA";
                worksheet.Cell(1, 6).Value = "BAŞVURU TARİHİ";
                worksheet.Cell(1, 7).Value = "TERCİH 1";
                worksheet.Cell(1, 8).Value = "TERCİH 2";
                worksheet.Cell(1, 9).Value = "TERCİH 3";

                int row = 2;
                foreach (var t in talepler)
                {
                    var tercihler = t.Tercihler.OrderBy(x => x.Sira).ToList();

                    worksheet.Cell(row, 1).Value = $"{t.Personel.Adi} {t.Personel.Soyadi}";
                    worksheet.Cell(row, 2).Value = t.Personel.SicilNo;
                    worksheet.Cell(row, 3).Value = t.Personel.Unvan;
                    worksheet.Cell(row, 4).Value = t.TalepTuru;
                    worksheet.Cell(row, 5).Value = t.Aciklama;
                    worksheet.Cell(row, 6).Value = t.BasvuruTarihi.ToShortDateString();
                    worksheet.Cell(row, 7).Value = tercihler.ElementAtOrDefault(0)?.Adliye?.Ad ?? "";
                    worksheet.Cell(row, 8).Value = tercihler.ElementAtOrDefault(1)?.Adliye?.Ad ?? "";
                    worksheet.Cell(row, 9).Value = tercihler.ElementAtOrDefault(2)?.Adliye?.Ad ?? "";

                    row++;
                }

                using var stream = new MemoryStream();
                workbook.SaveAs(stream);
                stream.Position = 0;

                _logger.LogInformation("Tayin talepleri Excel olarak dışa aktarıldı.");
                return File(stream.ToArray(),
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    "TayinTalepleri.xlsx");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Excel dışa aktarımı sırasında hata oluştu.");
                return View("Hata");
            }
        }
    }
}
