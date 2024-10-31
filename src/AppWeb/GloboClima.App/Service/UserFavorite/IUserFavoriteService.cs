using GloboClima.App.Configuration.Authentication.ViewModels;
using GloboClima.App.Models;

namespace GloboClima.App.Service
{
    public interface IUserFavoriteService
    {
        Task<ApiResponse<List<UserFavorite>>> GetListFavoritesAsync(Guid userId);
        Task<ApiResponse<UserFavorite>> AddToFavoriteAsync(UserFavorite userFavorite);
    }
}
