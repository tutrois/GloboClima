using GloboClima.App.Data.Reponses;

namespace GloboClima.App.ViewModels
{
    public class HomeDashViewModel
    {
        public string Nome { get; set; }

        public List<WeatherResponse> ?WhaterListResponse { get; set; }
    }
}
