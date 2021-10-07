using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ReserveringSysteem.Database;
using ReserveringSysteem.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ReserveringSysteem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var result = new List<ReserveringModel>();
            var vestigingen = await DatabaseManager.ReserveringDatabase.GetVestigingen();
            foreach (var vestiging in vestigingen)
            {
                result.Add(new()
                {
                    ID = vestiging.ID,
                    Naam = vestiging.Naam
                });
            }

            return View(result);
        }

        public IActionResult Reserveringen()
        {
            var timeList = new List<string>();

            var openingsTijd = new TimeSpan(11, 00, 00);
            var sluitingsTijd = new TimeSpan(23, 00, 00);

            while (openingsTijd <= sluitingsTijd)
            {
                timeList.Add($"{openingsTijd.Hours}:{openingsTijd.Minutes:00}");
                openingsTijd += new TimeSpan(00, 30, 00);
            }

            return View(timeList);
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
