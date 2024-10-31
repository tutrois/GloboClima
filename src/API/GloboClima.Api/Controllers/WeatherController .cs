using GloboClima.API.Configuration;
using GloboClima.API.Configuration.ApiExternal.OpenWeatherMap.Models;
using GloboClima.API.Configuration.Notificacoes;
using GloboClima.API.Models;
using GloboClima.API.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Runtime;
using System.Text.Json;

namespace GloboClima.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherController : BaseController
    {
        private readonly HttpClient _httpClient;
        private readonly ApiOpenWeatherSettings _openWeatherSettings;

        public WeatherController(INotificador notificador, HttpClient httpClient, 
             IOptions<ApiOpenWeatherSettings> _settings) : base(notificador)
        {
            httpClient.BaseAddress = new Uri(_settings.Value.BaseUrl);
           

            _httpClient = httpClient;
            _openWeatherSettings = _settings.Value;
        }

        [HttpGet("getCity/{cityName}")]
        [ProducesResponseType(typeof(List<City>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<City>>> GetCityByName(string cityName)
        {
            if (string.IsNullOrWhiteSpace(cityName))
            {
                NotificarErro("O nome da cidade não pode ser vazio ou nulo.");
                return CustomResponse();
            }

            var result = await GetCitiesByNames(new List<string> { cityName });

            if (!OperacaoValida())
                return CustomResponse();

            return CustomResponse(result);
        }

        [HttpGet("getCitys")]
        [ProducesResponseType(typeof(List<City>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<City>>> GetCitysByName([FromQuery] List<string> cityNames)
        {
            if (cityNames == null || cityNames.Count == 0 || cityNames.Any(string.IsNullOrWhiteSpace))
            {
                NotificarErro("Os nomes das cidades não podem ser vazios ou nulos.");
                return CustomResponse();
            }

            var result = await GetCitiesByNames(cityNames);

            if (!OperacaoValida())
                return CustomResponse();

            return CustomResponse(result);
        }

        private async Task<List<City>> GetCitiesByNames(List<string> cityNames)
        {
            var cities = new List<City>();

            foreach (var cityName in cityNames)
            {
                var response = await _httpClient.GetAsync($"/geo/1.0/direct?q={cityName}&appid={_openWeatherSettings.ApiKey}&units=metric&lang=pt_br");

                if (response.IsSuccessStatusCode)
                {
                    var cityList = await DeserializarObjetoResponse<List<City>>(response);
                    cities.AddRange(cityList);
                }
                else
                {
                    NotificarErro($"Não foi possível encontrar a cidade: {cityName}");
                }
            }

            return cities;
        }

        [HttpGet("Current")]
        [ProducesResponseType(typeof(WeatherResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<WeatherResponse>> GetCurrentWeatherDataAsync([FromQuery] LocationViewModel location)
        {
            if (location == null || string.IsNullOrWhiteSpace(location.Latitude) || string.IsNullOrWhiteSpace(location.Longitude))
            {
                NotificarErro("A Latitude e a Longitude da cidade não pode ser vazio ou nulo.");
                return CustomResponse();
            }

            var response = await _httpClient.GetAsync($"/data/2.5/weather?lat={location.Latitude}&lon={location.Longitude}&appid={_openWeatherSettings.ApiKey}&units=metric&lang=pt_br");

            if (!response.IsSuccessStatusCode)
                return CustomResponse(response);

            return CustomResponse(await DeserializarObjetoResponse<WeatherResponse>(response));
        }

        [HttpGet("listCurrentDataByName")]
        [ProducesResponseType(typeof(WeatherResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<WeatherResponse>> GetListCurrentWeatherDataByNameAsync([FromQuery] List<string> cityNames)
        {
            if (cityNames == null || cityNames.Count == 0 || cityNames.Any(string.IsNullOrWhiteSpace))
            {
                NotificarErro("Os nomes das cidades não podem ser vazios ou nulos.");
                return CustomResponse();
            }
            var listCities = await GetCitiesByNames(cityNames);

            var listCurrentData = new List<WeatherResponse>();

            foreach (var city in listCities)
            {
                var response = await _httpClient.GetAsync($"/data/2.5/weather?lat={city.Lat}&lon={city.Lon}&appid={_openWeatherSettings.ApiKey}&units=metric&lang=pt_br");

                if (response.IsSuccessStatusCode)
                {
                    var weatherResponse = await DeserializarObjetoResponse<WeatherResponse>(response);
                    listCurrentData.Add(weatherResponse);
                }
                else
                {
                    NotificarErro($"Não foi possível encontrar a cidade: {city.Name}");
                }
            }

            if (!OperacaoValida())
                return CustomResponse();

            return CustomResponse(listCurrentData);
        }
    }
}
