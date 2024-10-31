using System.Text.Json.Serialization;

namespace GloboClima.App.Models
{
    public class WeatherResponse
    {

        [JsonPropertyName("coord")]
        public Coord Coordenadas { get; set; }

        [JsonPropertyName("weather")]
        public List<Weather> Meteorologia { get; set; }

        [JsonPropertyName("base")]
        public string Base { get; set; }

        [JsonPropertyName("main")]
        public Main Main { get; set; }

        [JsonPropertyName("visibility")]
        public int Visibility { get; set; }

        [JsonPropertyName("wind")]
        public Wind Wind { get; set; }

        [JsonPropertyName("rain")]
        public Rain Rain { get; set; }

        [JsonPropertyName("clouds")]
        public Clouds Clouds { get; set; }

        [JsonPropertyName("dt")]
        public long Dt { get; set; }

        [JsonPropertyName("sys")]
        public Sys Sys { get; set; }

        [JsonPropertyName("timezone")]
        public int Timezone { get; set; }

        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("cod")]
        public int Cod { get; set; }
        public ResponseResult ReponseResult { get; set; }

    }
    public class Coord
    {
          
        public double Lon { get; set; }

           
        public double Lat { get; set; }
    }

    public class Weather
    {
         
        public int Id { get; set; }

          
        public string Main { get; set; }

           
        public string Description { get; set; }

            
        public string Icon { get; set; }
    }

    public class Main
    {
            
        public double Temp { get; set; }

           
        public double FeelsLike { get; set; }

          
        public double TempMin { get; set; }

        public double TempMax { get; set; }

          
        public int Pressure { get; set; }

            
        public int Humidity { get; set; }

           
        public int SeaLevel { get; set; }

            
        public int GrndLevel { get; set; }
    }

    public class Wind
    {
           
        public double Speed { get; set; }
            
        public int Deg { get; set; }

        public double Gust { get; set; }
    }

    public class Rain
    {
          
        public double OneHour { get; set; }
    }

    public class Clouds
    {
            
        public int All { get; set; }
    }

    public class Sys
    {
           
        public int Type { get; set; }

            
        public int Id { get; set; }

            
        public string Country { get; set; }

           
        public long Sunrise { get; set; }

            
        public long Sunset { get; set; }
    }
}


