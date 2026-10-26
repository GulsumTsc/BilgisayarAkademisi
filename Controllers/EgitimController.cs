using BilgisayarAkademisi.Services;
using Microsoft.AspNetCore.Mvc;

namespace BilgisayarAkademisi.Controllers;

// İçerik dosyada, işlem kuralları serviste, HTML gösterimi View'dadır.
[ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
public class EgitimController : Controller
{
    private readonly EgitimAkisi _akis;
    public EgitimController(EgitimAkisi akis) => _akis = akis;

    [HttpGet]
    public IActionResult Index(int? etap)
    {
        var model = _akis.Ekran(HttpContext.Session, etap);
        if (model.Can <= 0) return RedirectToAction(nameof(Oyun));
        return View(model);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public IActionResult Cevapla(int etapNo, int soruId, string denemeId, int? secilenSecenek)
    {
        string? hata = _akis.Cevapla(HttpContext.Session, etapNo, soruId, denemeId, secilenSecenek);
        if (hata != null) TempData["Hata"] = hata;
        return RedirectToAction(nameof(Index));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public IActionResult Sonraki(int etapNo, int soruId, string denemeId)
    {
        _akis.Sonraki(HttpContext.Session, etapNo, soruId, denemeId);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Oyun()
    {
        var model = _akis.Ekran(HttpContext.Session);
        if (model.Can > 0) return RedirectToAction(nameof(Index));
        return View(model);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public IActionResult YenidenBaslat(int etapNo, string denemeId)
    {
        _akis.YenidenBaslat(HttpContext.Session, etapNo, denemeId);
        return RedirectToAction(nameof(Index));
    }
}
