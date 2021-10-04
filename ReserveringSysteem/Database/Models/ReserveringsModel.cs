using System;

namespace ReserveringSysteem.Database.Models
{
    public class ReserveringsModel
    {
        public int ID { get; set; }
        public int ReserveringID { get; set; }
        public int Tafel { get; set; }
        public int AantalPersonen { get; set; }
        public string NaamReserverende { get; set; }
        public string TelefoonNummer { get; set; }
        public int BedrijfID { get; set; }  //< Linked to BedrijfsModel
        public DateTime Tijd { get; set; }

        public BedrijfsModel Bedrijf { get; set; }
        public VestigingsModel Vestiging { get; set; }
    }
}
