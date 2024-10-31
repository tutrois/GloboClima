namespace GloboClima.API.Models
{
    public class City
    {
        public string Name { get; set; }
        public Dictionary<string, string> Local_Names { get; set; }
        public double Lat { get; set; }
        public double Lon { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
    }
}
