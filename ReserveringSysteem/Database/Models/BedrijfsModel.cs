namespace ReserveringSysteem.Database.Models
{
    public class BedrijfsModel
    {
        public int ID { get; set; }
        public string Naam { get; set; }
        public string Adress { get; set; }
        public string BTWNummer { get; set; }
        public string KVKNummer { get; set; }

        public ReserveringsModel Reservering { get; set; }
    }
}
