using GloboClima.App.Configuration.Authentication.ViewModels;
using GloboClima.App.Models;

namespace GloboClima.App.Service
{
    public interface IWeatherService
    {
        Task<ApiResponse<List<CityResponse>>> GetListFiveWeatherAsync();
        Task<ApiResponse<List<WeatherResponse>>> GetListFiveCurrentWeatherDataAsync();
        Task<ApiResponse<List<WeatherResponse>>> GetListCurrentWeatherDataAsync(List<string> citys);
    }
}
