using Microsoft.AspNetCore.Mvc;
using ReserveringSysteem.Database;
using ReserveringSysteem.Database.Models;
using ReserveringSysteem.Models;
using System;
using System.Threading.Tasks;

namespace ReserveringSysteem.Controllers
{
    public class VestigingController : Controller
    {
        public async Task<IActionResult> Index(int id)
        {
            var vestiging = await DatabaseManager.ReserveringDatabase.GetVestiging(id);
            if (vestiging == null)
                return RedirectToAction("Index", "Home");

            ViewData["Vestiging"] = vestiging;

            return View();
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> CreateVestiging(VestigingModel model)
        {
            if (!ModelState.IsValid)
                ViewData["Error"] += "Er is een error! :( Contacteer de developer! ";

            if (string.IsNullOrWhiteSpace(model.Naam))
                ViewData["Error"] += "Voer een naam in! Erg belangrijk ";

            if (string.IsNullOrWhiteSpace(model.MaxPersonen) || string.IsNullOrWhiteSpace(model.MaxTafels))
                ViewData["Error"] += "Max aantal personen of max aantal tafels is leeg! ";

            if (string.IsNullOrWhiteSpace(model.OpeningsTijd) || string.IsNullOrWhiteSpace(model.SluitingsTijd))
                ViewData["Error"] += "Openings tijd of sluitings tijd is leeg! ";

            if (!int.TryParse(model.MaxPersonen, out var maxPersonen))
                ViewData["Error"] += "Max Personen is geen nummer! ";

            if (!int.TryParse(model.MaxTafels, out var maxTafels))
                ViewData["Error"] += "Max Tafels is geen nummer! ";

            if (!TimeSpan.TryParse(model.OpeningsTijd, out var openingsTijd))
                ViewData["Error"] += "Incorrect Openings tijd formaat, moet zijn (xx:xx) ";

            if (!TimeSpan.TryParse(model.SluitingsTijd, out var sluitingsTijd))
                ViewData["Error"] += "Incorrect Sluitings tijd formaat, moet zijn (xx:xx) ";

            if (ViewData["Error"] != null)
                return View("Create");

            await DatabaseManager.ReserveringDatabase.AddVestiging(new()
            {
                Naam            = model.Naam,
                MaxPersonen     = maxPersonen,
                MaxTafels       = maxTafels,
                OpeningsTijd    = openingsTijd,
                SluitingsTijd   = sluitingsTijd,
            });

            return RedirectToAction("Index", "Home");
        }

        [HttpGet("reservering/toevoegen")]
        public async Task<IActionResult> CreateReservering(int? id)
        {
            if (id == null)
                return NotFound();

            var vestiging = await DatabaseManager.ReserveringDatabase.GetVestiging(id ?? 0);
            if (vestiging == null)
                return RedirectToAction("Index", "Home");

            ViewData["VestigingID"] = id;

            return View();
        }

        [HttpPost("/api/reservering/toevoegen")]
        public async Task<IActionResult> CreateReserveringAPI(ReserveringModel model)
        {
            if (!ModelState.IsValid)
                ViewData["Error"] += "Er is een error! :( Contacteer de developer! ";

            if (string.IsNullOrWhiteSpace(model.NaamReserverende))
                ViewData["Error"] += "Naam Reserverende is leeg! ";

            if (string.IsNullOrWhiteSpace(model.TelefoonNummer))
                ViewData["Error"] += "Telefoon Nummer is leeg! ";

            if (string.IsNullOrWhiteSpace(model.AantalPersonen))
                ViewData["Error"] += "Aantal Personen is leeg! ";

            if (string.IsNullOrWhiteSpace(model.Tafel))
                ViewData["Error"] += "Tafel is leeg! ";

            if (string.IsNullOrWhiteSpace(model.Datum))
                ViewData["Error"] += "Datum is leeg! ";

            if (string.IsNullOrWhiteSpace(model.Tijd))
                ViewData["Error"] += "Tijd is leeg! ";

            if (!int.TryParse(model.AantalPersonen, out var aantalPersonen))
                ViewData["Error"] += "Aantal Personen is geen nummer!";

            if (!int.TryParse(model.Tafel, out var tafel))
                ViewData["Error"] += "Tafel is geen nummer!";

            if (!DateTime.TryParse(model.Datum, out var datum))
                ViewData["Error"] += "Datum is in de verkeerde formaat (moet zijn xx-xx-xx)";

            if (!TimeSpan.TryParse(model.Tijd, out var tijd))
                ViewData["Error"] += "Tijd is in de verkeerde formaat (moet zijn xx:xx)";

            // Add the time to the date
            datum = datum.Add(tijd);

            var vestiging = await DatabaseManager.ReserveringDatabase.GetVestiging(int.Parse(model.VestigingID));
            if (vestiging == null)
                return RedirectToAction("Index", "Home");

            var reservering = new ReserveringsModel()
            {
                ID                  = vestiging.ID,
                NaamReserverende    = model.NaamReserverende,
                TelefoonNummer      = model.TelefoonNummer,
                AantalPersonen      = aantalPersonen,
                Tafel               = tafel,
                Tijd                = datum,
            };

            if (vestiging.OpeningsTijd > tijd || vestiging.SluitingsTijd < tijd)
                ViewData["Error"] += $"De tijd is verkeerd, openings tijd en sluitings tijd is van {vestiging.OpeningsTijd} tot {vestiging.SluitingsTijd}";

            if (DatabaseManager.ReserveringDatabase.HasMaxReservations(vestiging, reservering))
                ViewData["Error"] += $"Vestiging is aan max capaciteit van {vestiging.MaxPersonen} personen";

            if (DatabaseManager.ReserveringDatabase.HasReservationOnDateAndTable(datum, tafel))
                ViewData["Error"] += $"Er is al een afspraak gepland op {datum} bij tafel {tafel}";

            if (ViewData["Error"] != null)
            {
                ViewData["VestigingID"] = model.VestigingID;
                return View("CreateReservering");
            }

            await DatabaseManager.ReserveringDatabase.AddReservering(reservering);

            return RedirectToAction("Index", new { id = vestiging.ID });
        }
    }
}
