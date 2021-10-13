namespace ReserveringSysteem.Models
{
    public class ReserveringModel
    {
        public string VestigingID { get; set; }
        public string NaamReserverende { get; set; }
        public string TelefoonNummer { get; set; }
        public string AantalPersonen { get; set; }
        public string Tafel { get; set; }
        public string Datum { get; set; }
        public string Tijd { get; set; }
        public int BedrijfID { get; set; }
    }
}
