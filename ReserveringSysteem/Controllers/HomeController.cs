using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ReserveringSysteem.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ReserveringSysteem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var timeList = new List<string>();

            var openingsTijd = new TimeSpan(17, 00, 00);
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
