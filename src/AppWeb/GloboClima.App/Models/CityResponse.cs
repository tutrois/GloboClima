namespace GloboClima.App.Models
{
    public class CityResponse
    {
        public string Name { get; set; }
        public Dictionary<string, string> LocalNames { get; set; }
        public double Lat { get; set; }
        public double Lon { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public ResponseResult ReponseResult { get; set; }

    }
}
