using GloboClima.App.Data.Reponses;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace GloboClima.App.ViewModels
{
    public class HomeDashViewModel
    {
        [JsonPropertyName("userId")]
        public Guid UserId { get; set; }

        [JsonPropertyName("nomeCidade")]
        public string Nome { get; set; }

        [IgnoreDataMember]
        public List<WeatherResponse> ?ListWeatherResponse { get; set; }
    }
}
