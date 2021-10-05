using System;

namespace ReserveringSysteem.Models
{
    public class ReserveringModel
    {
        public TimeSpan OpeningsTijd { get; set; }
        public TimeSpan SluitingsTijd { get; set; }
        public int MaxTafels { get; set; }
        // public List<Reservering> Reserveringen { get; set; }
    }
}
