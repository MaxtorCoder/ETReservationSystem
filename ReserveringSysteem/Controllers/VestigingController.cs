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
            if (id == 0)
                ViewData["Vestigingen"] = await DatabaseManager.ReserveringDatabase.GetAllVestigingen();
            else
            {
                var vestiging = await DatabaseManager.ReserveringDatabase.GetVestiging(id);
                if (vestiging == null)
                    return RedirectToAction("Index", "Home");

                ViewData["Vestiging"] = vestiging;
            }

            return View();
        }

        [HttpGet("/create")]
        public IActionResult Create() => View();

        [HttpPost("/api/create-vestiging")]
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

        [HttpGet("/create-reservering")]
        public async Task<IActionResult> CreateReservering()
        {
            ViewData["Bedrijven"] = await DatabaseManager.ReserveringDatabase.GetAllBedrijven();
            ViewData["Vestigingen"] = await DatabaseManager.ReserveringDatabase.GetAllVestigingen();

            return View();
        }

        [HttpPost("/api/create-reservering")]
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

            var vestiging = await DatabaseManager.ReserveringDatabase.GetVestiging(model.VestigingID);
            if (vestiging == null)
                return RedirectToAction("Index", "Home");

            if (model.BedrijfID == 0)
                ViewData["Error"] += "Geen bedrijf geselecteerd! ";

            if (await DatabaseManager.ReserveringDatabase.GetBedrijf(model.BedrijfID) == null)
                ViewData["Error"] += "Bedrijf bestaat niet (dev error)! ";

            var reservering = new ReserveringsModel()
            {
                ID                  = vestiging.ID,
                NaamReserverende    = model.NaamReserverende,
                TelefoonNummer      = model.TelefoonNummer,
                AantalPersonen      = aantalPersonen,
                Tafel               = tafel,
                Tijd                = datum,
                BedrijfID           = model.BedrijfID,
            };

            if (vestiging.OpeningsTijd > tijd || vestiging.SluitingsTijd < tijd)
                ViewData["Error"] += $"De tijd is verkeerd, openings tijd en sluitings tijd is van {vestiging.OpeningsTijd} tot {vestiging.SluitingsTijd}";

            if (DatabaseManager.ReserveringDatabase.HasMaxReservations(vestiging, reservering))
                ViewData["Error"] += $"Vestiging is aan max capaciteit van {vestiging.MaxPersonen} personen";

            if (DatabaseManager.ReserveringDatabase.HasReservationOnDateAndTable(datum, tafel))
                ViewData["Error"] += $"Er is al een afspraak gepland op {datum} bij tafel {tafel}";

            if (ViewData["Error"] != null)
            {
                ViewData["Bedrijven"] = await DatabaseManager.ReserveringDatabase.GetAllBedrijven();
                ViewData["Vestigingen"] = await DatabaseManager.ReserveringDatabase.GetAllVestigingen();

                return View("CreateReservering");
            }

            await DatabaseManager.ReserveringDatabase.AddReservering(reservering);

            return RedirectToAction("Index", new { id = vestiging.ID });
        }

        [HttpGet("/remove-reservering")]
        public async Task<IActionResult> RemoveReservering(int? vestigingID, int? reserveringID)
        {
            var vestiging = await DatabaseManager.ReserveringDatabase.GetVestiging(vestigingID ?? 0);
            if (vestiging == null)
                return RedirectToAction("Index", new { id = vestigingID ?? 0 });

            var reservering = await DatabaseManager.ReserveringDatabase.GetReservering(reserveringID ?? 0);
            if (reservering == null)
                return RedirectToAction("Index", new { id = vestigingID ?? 0 });

            if (reservering.ID != vestiging.ID)
                return RedirectToAction("Index", new { id = vestigingID ?? 0 });

            await DatabaseManager.ReserveringDatabase.RemoveReservering(reservering);

            return RedirectToAction("Index", new { id = vestiging.ID });
        }

        [HttpGet("/bedrijven")]
        public async Task<IActionResult> ViewBedrijven()
        {
            ViewData["Bedrijven"] = await DatabaseManager.ReserveringDatabase.GetAllBedrijven();

            return View();
        }

        [HttpGet("/create-bedrijf")]
        public IActionResult CreateBedrijf() => View();

        [HttpPost("/api/create-bedrijf")]
        public async Task<IActionResult> CreateBedrijfAPI(BedrijfModel model)
        {
            if (!ModelState.IsValid)
                ViewData["Error"] += "Er is een error! :( Contacteer de developer! ";

            if (string.IsNullOrWhiteSpace(model.Naam))
                ViewData["Error"] += "Naam van bedrijf is leeg! ";

            if (string.IsNullOrWhiteSpace(model.Adress))
                ViewData["Error"] += "Address is leeg! ";

            if (string.IsNullOrWhiteSpace(model.PostCode))
                ViewData["Error"] += "Post Code is leeg! ";

            if (string.IsNullOrWhiteSpace(model.Afdeling))
                ViewData["Error"] += "Afdeling is leeg! ";

            if (string.IsNullOrWhiteSpace(model.BTWNummer))
                ViewData["Error"] += "BTW-Nummer is leeg! ";

            if (string.IsNullOrWhiteSpace(model.KVKNummer))
                ViewData["Error"] += "KVK-Nummer is leeg! ";

            if (await DatabaseManager.ReserveringDatabase.KvkNummerExists(model.KVKNummer))
                ViewData["Error"] += "KVK-Nummer bestaat al! ";

            if (string.IsNullOrWhiteSpace(model.TelefoonNummer))
                ViewData["Error"] += "Telefoon-Nummer is leeg! ";

            if (ViewData["Error"] != null)
                return View("Create");

            var bedrijf = new BedrijfsModel
            {
                Naam            = model.Naam,
                Adress          = model.Adress,
                PostCode        = model.PostCode,
                Afdeling        = model.Afdeling,
                BTWNummer       = model.BTWNummer,
                KVKNummer       = model.KVKNummer,
                TelefoonNummer  = model.TelefoonNummer,
            };

            await DatabaseManager.ReserveringDatabase.AddBedrijf(bedrijf);

            return RedirectToAction("ViewBedrijven");
        }

        [HttpGet("/remove-bedrijf")]
        public async Task<IActionResult> RemoveBedrijf(int? id)
        {
            var bedrijf = await DatabaseManager.ReserveringDatabase.GetBedrijf(id ?? 0);
            if (bedrijf == null)
                return RedirectToAction("ViewBedrijven");

            await DatabaseManager.ReserveringDatabase.RemoveBedrijf(bedrijf);

            return RedirectToAction("ViewBedrijven");
        }

        [HttpGet("/edit-bedrijf")]
        public async Task<IActionResult> EditBedrijf(int? id)
        {
            if (id == null)
                return RedirectToAction("ViewBedrijven");

            var bedrijf = await DatabaseManager.ReserveringDatabase.GetBedrijf(id ?? 0);
            if (bedrijf == null)
                return RedirectToAction("ViewBedrijven");

            ViewData["Bedrijf"] = bedrijf;

            return View();
        }

        [HttpPost("/api/edit-bedrijf")]
        public async Task<IActionResult> EditBedrijfAPI(BedrijfModel model)
        {
            if (!ModelState.IsValid)
                ViewData["Error"] += "Er is een error! :( Contacteer de developer! ";

            if (ViewData["Error"] != null)
                return View("Create");

            var bedrijf = new BedrijfsModel
            {
                ID              = model.ID,
                Naam            = model.Naam,
                Adress          = model.Adress,
                PostCode        = model.PostCode,
                Afdeling        = model.Afdeling,
                BTWNummer       = model.BTWNummer,
                KVKNummer       = model.KVKNummer,
                TelefoonNummer  = model.TelefoonNummer,
            };

            await DatabaseManager.ReserveringDatabase.UpdateBedrijf(bedrijf);

            return RedirectToAction("ViewBedrijven");
        }
    }
}
