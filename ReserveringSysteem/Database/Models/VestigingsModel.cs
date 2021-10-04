using System;
using System.Collections.Generic;

namespace ReserveringSysteem.Database.Models
{
    public class VestigingsModel
    {
        public int ID { get; set; }
        public string Naam { get; set; }
        public int MaxTafels { get; set; }
        public int MaxPersonen { get; set; }
        public TimeSpan OpeningsTijd { get; set; }
        public TimeSpan SluitingsTijd { get; set; }

        public ICollection<ReserveringsModel> Reservering { get; set; }
    }
}
