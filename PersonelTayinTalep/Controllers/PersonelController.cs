using Business.Abstract;
using Core.DTOs;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using PersonelTayinTalep.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PersonelTayinTalep.Filters;

namespace PersonelTayinTalep.Controllers
{
    [PersonelAuthorize]
    public class PersonelController : Controller
    {
        private readonly IPersonelService _personelService;
        private readonly ITayinTalepService _tayinTalepService;
        private readonly ITayinTalepTercihService _tercihService;
        private readonly IAdliyeService _adliyeService;
        private readonly ILogger<PersonelController> _logger;

        public PersonelController(
            IPersonelService personelService,
            ITayinTalepService tayinTalepService,
            IAdliyeService adliyeService,
            ITayinTalepTercihService tercihService,
            ILogger<PersonelController> logger)
        {
            _personelService = personelService;
            _tayinTalepService = tayinTalepService;
            _adliyeService = adliyeService;
            _tercihService = tercihService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var personelId = HttpContext.Session.GetInt32("PersonelId");

            var personel = await _personelService.GetByIdAsync(personelId.Value);
            var talepler = await _tayinTalepService.GetByPersonelIdAsync(personelId.Value);

            _logger.LogInformation("Personel ID {Id} ana ekranı görüntüledi.", personelId);

            var viewModel = new PersonelTayinTalepViewModel
            {
                Personel = personel,
                Talepler = talepler
            };

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var personelId = HttpContext.Session.GetInt32("PersonelId");

            var mevcutTalepler = await _tayinTalepService.GetByPersonelIdAsync(personelId.Value);
            bool ayniGunTalepVar = mevcutTalepler.Any(t => t.BasvuruTarihi.Date == DateTime.Today);
            if (ayniGunTalepVar)
            {
                _logger.LogWarning("Personel ID {Id} aynı gün içinde ikinci kez talep oluşturmak istedi.", personelId);
                TempData["TayinUyari"] = "Aynı dönem içinde sadece bir tayin talebi oluşturabilirsiniz.";
                return RedirectToAction("Index");
            }

            _logger.LogInformation("Personel ID {Id} yeni tayin talebi oluşturma sayfasını açtı.", personelId);

            ViewBag.TalepTurleri = new SelectList(new List<string> { "Eş Durumu", "Sağlık", "Hizmet Gereği", "Diğer" });
            ViewBag.AdliyeList1 = new SelectList(await _adliyeService.GetAllAsync(), "Id", "Ad");
            ViewBag.AdliyeList2 = new SelectList(await _adliyeService.GetAllAsync(), "Id", "Ad");
            ViewBag.AdliyeList3 = new SelectList(await _adliyeService.GetAllAsync(), "Id", "Ad");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(TayinTalep talep, int?[] adliyeIdleri)
        {
            var personelId = HttpContext.Session.GetInt32("PersonelId");

            var mevcutTalepler = await _tayinTalepService.GetByPersonelIdAsync(personelId.Value);
            var ayniGunTalepVarMi = mevcutTalepler.Any(t => t.BasvuruTarihi.Date == DateTime.Today);

            if (ayniGunTalepVarMi)
            {
                _logger.LogWarning("Personel ID {Id} aynı gün içinde tekrar talep oluşturmaya çalıştı.", personelId);
                TempData["TayinUyari"] = "Aynı dönem içinde sadece bir tayin talebi oluşturabilirsiniz.";
                return RedirectToAction("Index");
            }

            var doluTercihler = adliyeIdleri
                .Where(id => id.HasValue && id.Value != 0)
                .Select(id => id.Value)
                .ToList();

            if (!doluTercihler.Any())
                ModelState.AddModelError("", "En az bir tercih seçmelisiniz.");
            else if (doluTercihler.Count != doluTercihler.Distinct().Count())
                ModelState.AddModelError("", "Aynı adliyeyi birden fazla kez tercih edemezsiniz.");

            if (!ModelState.IsValid)
            {
                ViewBag.TalepTurleri = new SelectList(new List<string> { "Eş Durumu", "Sağlık", "Hizmet Gereği", "Diğer" }, talep.TalepTuru);
                ViewBag.AdliyeList1 = new SelectList(await _adliyeService.GetAllAsync(), "Id", "Ad", adliyeIdleri.ElementAtOrDefault(0));
                ViewBag.AdliyeList2 = new SelectList(await _adliyeService.GetAllAsync(), "Id", "Ad", adliyeIdleri.ElementAtOrDefault(1));
                ViewBag.AdliyeList3 = new SelectList(await _adliyeService.GetAllAsync(), "Id", "Ad", adliyeIdleri.ElementAtOrDefault(2));
                return View(talep);
            }

            talep.PersonelId = personelId.Value;
            talep.BasvuruTarihi = DateTime.Now;

            await _tayinTalepService.AddAsync(talep);

            for (int i = 0; i < doluTercihler.Count && i < 3; i++)
            {
                await _tercihService.AddAsync(new TayinTalepTercih
                {
                    TayinTalepId = talep.Id,
                    AdliyeId = doluTercihler[i],
                    Sira = i + 1
                });
            }

            _logger.LogInformation("Personel ID {Id} tayin talebi oluşturdu. Talep ID: {TalepId}", personelId, talep.Id);

            TempData["BasariMesaji"] = "Tayin talebiniz başarıyla kaydedildi.";
            return RedirectToAction("Index", "Personel");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var personelId = HttpContext.Session.GetInt32("PersonelId");

            var mevcutTalepler = await _tayinTalepService.GetByPersonelIdAsync(personelId.Value);
            var sonTalep = mevcutTalepler.OrderByDescending(t => t.BasvuruTarihi).FirstOrDefault();

            if (sonTalep == null || sonTalep.Id != id)
            {
                _logger.LogWarning("Personel ID {Id} geçersiz düzenleme girişimi yaptı. Talep ID: {TalepId}", personelId, id);
                return BadRequest("Sadece en son başvurunuzu düzenleyebilirsiniz.");
            }

            if (sonTalep.BasvuruTarihi.Date != DateTime.Today)
            {
                _logger.LogWarning("Personel ID {Id} geçmiş tarihteki talebi düzenlemeye çalıştı. Talep ID: {TalepId}", personelId, id);
                TempData["TayinUyari"] = "Sadece başvuru yaptığınız gün düzenleme yapabilirsiniz.";
                return RedirectToAction("Index");
            }

            var seciliTercihler = await _tercihService.GetByTayinTalepIdAsync(id);
            var adliyeler = await _adliyeService.GetAllAsync();

            ViewBag.TalepTurleri = new SelectList(
                new List<string> { "Eş Durumu", "Sağlık", "Hizmet Gereği", "Diğer" },
                sonTalep.TalepTuru
            );

            ViewBag.AdliyeList1 = new SelectList(adliyeler, "Id", "Ad", seciliTercihler.ElementAtOrDefault(0)?.AdliyeId);
            ViewBag.AdliyeList2 = new SelectList(adliyeler, "Id", "Ad", seciliTercihler.ElementAtOrDefault(1)?.AdliyeId);
            ViewBag.AdliyeList3 = new SelectList(adliyeler, "Id", "Ad", seciliTercihler.ElementAtOrDefault(2)?.AdliyeId);

            _logger.LogInformation("Personel ID {Id} tayin talebi düzenleme sayfasını açtı. Talep ID: {TalepId}", personelId, id);

            return View(sonTalep);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(TayinTalep talep, int?[] adliyeIdleri)
        {
            var personelId = HttpContext.Session.GetInt32("PersonelId");

            var mevcutTalepler = await _tayinTalepService.GetByPersonelIdAsync(personelId.Value);
            var sonTalep = mevcutTalepler.OrderByDescending(t => t.BasvuruTarihi).FirstOrDefault();

            if (sonTalep == null || sonTalep.Id != talep.Id)
            {
                _logger.LogWarning("Personel ID {Id} izinsiz güncelleme girişiminde bulundu. Talep ID: {TalepId}", personelId, talep.Id);
                return BadRequest("Sadece en son başvurunuzu güncelleyebilirsiniz.");
            }

            if (sonTalep.BasvuruTarihi.Date != DateTime.Today)
            {
                _logger.LogWarning("Personel ID {Id} geçmiş tarihteki talebi güncellemeye çalıştı. Talep ID: {TalepId}", personelId, talep.Id);
                TempData["TayinUyari"] = "Sadece başvuru yaptığınız gün güncelleme yapabilirsiniz.";
                return RedirectToAction("Index");
            }

            var doluTercihler = adliyeIdleri.Where(x => x.HasValue && x.Value != 0).Select(x => x.Value).ToList();

            if (!doluTercihler.Any())
                ModelState.AddModelError("", "En az bir tercih seçmelisiniz.");
            else if (doluTercihler.Count != doluTercihler.Distinct().Count())
                ModelState.AddModelError("", "Aynı adliyeyi birden fazla kez tercih edemezsiniz.");

            if (!ModelState.IsValid)
            {
                var adliyeler = await _adliyeService.GetAllAsync();

                ViewBag.TalepTurleri = new SelectList(
                    new List<string> { "Eş Durumu", "Sağlık", "Hizmet Gereği", "Diğer" },
                    talep.TalepTuru
                );

                ViewBag.AdliyeList1 = new SelectList(adliyeler, "Id", "Ad", adliyeIdleri.ElementAtOrDefault(0));
                ViewBag.AdliyeList2 = new SelectList(adliyeler, "Id", "Ad", adliyeIdleri.ElementAtOrDefault(1));
                ViewBag.AdliyeList3 = new SelectList(adliyeler, "Id", "Ad", adliyeIdleri.ElementAtOrDefault(2));

                return View(talep);
            }

            sonTalep.TalepTuru = talep.TalepTuru;
            sonTalep.Aciklama = talep.Aciklama;
            await _tayinTalepService.UpdateAsync(sonTalep);

            var eskiTercihler = await _tercihService.GetByTayinTalepIdAsync(talep.Id);
            foreach (var tercih in eskiTercihler)
            {
                await _tercihService.DeleteAsync(tercih.Id);
            }

            for (int i = 0; i < doluTercihler.Count && i < 3; i++)
            {
                await _tercihService.AddAsync(new TayinTalepTercih
                {
                    TayinTalepId = talep.Id,
                    AdliyeId = doluTercihler[i],
                    Sira = i + 1
                });
            }

            _logger.LogInformation("Personel ID {Id} tayin talebini güncelledi. Talep ID: {TalepId}", personelId, talep.Id);

            TempData["BasariMesaji"] = "Tayin talebiniz başarıyla güncellendi.";
            return RedirectToAction("Index", "Personel");
        }


        //[HttpPost]
        //public async Task<IActionResult> Delete(int id)
        //{
        //    var personelId = HttpContext.Session.GetInt32("PersonelId");

        //    var mevcutTalepler = await _tayinTalepService.GetByPersonelIdAsync(personelId.Value);
        //    var sonTalep = mevcutTalepler.OrderByDescending(t => t.BasvuruTarihi).FirstOrDefault();

        //    if (sonTalep == null || sonTalep.Id != id)
        //        return BadRequest("Sadece en son başvurunuzu silebilirsiniz.");

        //    await _tayinTalepService.DeleteAsync(id);
        //    return RedirectToAction("Index");
        //}
    }
}
