using GloboClima.App.Configuration.Authentication.ViewModels;
using GloboClima.App.Extensions;
using GloboClima.App.Models;
using Microsoft.Extensions.Options;
using NuGet.Configuration;
using System.Net;
using System.Net.Http.Headers;
using System.Runtime;

namespace GloboClima.App.Service
{

    public class UserFavoriteService : BaseService, IUserFavoriteService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUser _appUser;


        public UserFavoriteService(HttpClient httpClient, IOptions<AppSettings> _settings, IHttpContextAccessor httpContextAccessor, IUser appUser)
        {
           

            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
            _appUser = appUser;
            httpClient.BaseAddress = new Uri(_settings.Value.AutenticacaoUrl);
        }

        public async Task<ApiResponse<UserFavorite>> AddToFavoriteAsync(UserFavorite userFavorite)
        {
            var content = ObterConteudo(userFavorite);

            var response = await _httpClient.PostAsync("/api/UserFavorite", content);
            var teste = await response.Content.ReadAsStringAsync();

            if (!TratarErrosResponse(response))
            {
                return new ApiResponse<UserFavorite>
                {
                    Data = new UserFavorite
                    {

                        ReponseResult = await DeserializarObjetoResponse<ResponseResult>(response)
                    }
                };
            }

            return await DeserializarObjetoResponse<ApiResponse<UserFavorite>>(response);
        }

        public async Task<ApiResponse<List<UserFavorite>>> GetListFavoritesAsync(Guid userId)
        {
            var response = await _httpClient.GetAsync($"/api/UserFavorite/ListUserFavorite/{userId}");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            if (!TratarErrosResponse(response))
            {
                return new ApiResponse<List<UserFavorite>>
                {
                    Success = false,
                    Data = new List<UserFavorite>() // Retorna uma lista vazia em caso de erro
                };
            }

            return await DeserializarObjetoResponse<ApiResponse<List<UserFavorite>>>(response);
        }
    }
}
