using GloboClima.API.Models;
using System.Net;

namespace GloboClima.API.Repository
{
    public interface IUserFavoriteRepository
    {
        Task<HttpStatusCode> Adicionar(UserFavorite userFavorite);
        Task<HttpStatusCode> Atualizar(UserFavorite userFavorite);
        Task<IEnumerable<UserFavorite>> Buscar(string userId);
        Task<UserFavorite?> Buscar(string userId, string nome);
        Task<HttpStatusCode> Deletar(UserFavorite userFavorite);
    }
}
