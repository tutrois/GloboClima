using GloboClima.App.Configuration.Authentication.ViewModels;
using GloboClima.App.Extensions;
using GloboClima.App.Models;
using Microsoft.Extensions.Options;

namespace GloboClima.App.Service
{
    public class WeatherService : BaseService, IWeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public WeatherService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor, IOptions<AppSettings> _settings)
        {
            httpClient.BaseAddress = new Uri(_settings.Value.AutenticacaoUrl);

            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ApiResponse<List<CityResponse>>> GetListFiveWeatherAsync()
        {
            var capitais = CapitalGenerator.GerarCapitaisAleatorias();

            // Converte a lista de capitais em uma string de consulta
            var queryString = string.Join("&", capitais.Select(city => $"cityNames={Uri.EscapeDataString(city)}"));

            var response = await _httpClient.GetAsync($"/api/Weather/Current?{queryString}");

            if (!TratarErrosResponse(response))
            {
                return new ApiResponse<List<CityResponse>>
                {
                    Success = false,
                    Data = new List<CityResponse>() // Retorna uma lista vazia em caso de erro
                };
            }

            var teste = await response.Content.ReadAsStringAsync();

            return await DeserializarObjetoResponse<ApiResponse<List<CityResponse>>>(response);
        }

        public async Task<ApiResponse<List<WeatherResponse>>> GetListFiveCurrentWeatherDataAsync()
        {
            var capitais = CapitalGenerator.GerarCapitaisAleatorias();

            // Converte a lista de capitais em uma string de consulta
            var queryString = string.Join("&", capitais.Select(city => $"cityNames={Uri.EscapeDataString(city)}"));

            var response = await _httpClient.GetAsync($"/api/Weather/listCurrentDataByName?{queryString}");

            if (!TratarErrosResponse(response))
            {
                return new ApiResponse<List<WeatherResponse>>
                {
                    Success = false,
                    Data = new List<WeatherResponse>() // Retorna uma lista vazia em caso de erro
                };
            }


            return await DeserializarObjetoResponse<ApiResponse<List<WeatherResponse>>>(response);
        }

        public async Task<ApiResponse<List<WeatherResponse>>> GetListCurrentWeatherDataAsync(List<string> citys)
        {
            // Converte a lista de capitais em uma string de consulta
            var queryString = string.Join("&", citys.Select(city => $"cityNames={Uri.EscapeDataString(city)}"));

            var response = await _httpClient.GetAsync($"/api/Weather/listCurrentDataByName?{queryString}");

            if (!TratarErrosResponse(response))
            {
                return new ApiResponse<List<WeatherResponse>>
                {
                    Success = false,
                    Data = new List<WeatherResponse>() // Retorna uma lista vazia em caso de erro
                };
            }


            return await DeserializarObjetoResponse<ApiResponse<List<WeatherResponse>>>(response);
        }
    }
}
