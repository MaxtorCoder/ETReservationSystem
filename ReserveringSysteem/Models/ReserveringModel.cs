using System;

namespace ReserveringSysteem.Models
{
    public class ReserveringModel
    {
        public int ID { get; set; }
        public string Naam { get; set; }
        public TimeSpan OpeningsTijd { get; set; }
        public TimeSpan SluitingsTijd { get; set; }
        public int MaxTafels { get; set; }
        // public List<Reservering> Reserveringen { get; set; }
    }
}
