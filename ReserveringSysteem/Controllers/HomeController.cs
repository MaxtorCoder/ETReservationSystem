﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ReserveringSysteem.Database;
using ReserveringSysteem.Models;
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
            ViewData["VestigingList"] = await DatabaseManager.ReserveringDatabase.GetVestigingen();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SelectVestiging(SelectVestigingModel model)
        {
            var vestiging = await DatabaseManager.ReserveringDatabase.GetVestiging(model.ID);
            if (vestiging == null)
                return View("Index");

            return RedirectToAction("Index", "Vestiging", new { id = model.ID });
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
