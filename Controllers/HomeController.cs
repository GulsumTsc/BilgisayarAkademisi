using System.Diagnostics;
using BilgisayarAkademisi.Models;
using Microsoft.AspNetCore.Mvc;
using BilgisayarAkademisi.Services;

namespace BilgisayarAkademisi.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly EgitimAkisi _akis;

        public HomeController(ILogger<HomeController> logger, EgitimAkisi akis)
        {
            _logger = logger;
            _akis = akis;
        }

        public IActionResult Index()
        {
            return View(_akis.Ekran(HttpContext.Session));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
