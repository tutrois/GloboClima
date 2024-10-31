using GloboClima.API.Configuration;
using GloboClima.API.Configuration.Notificacoes;
using GloboClima.API.Data.Models;
using GloboClima.API.Data.Repository;
using GloboClima.API.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GloboClima.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserFavoriteController : BaseController
    {
        private readonly HttpClient _httpClient;
        private readonly IUserFavoriteRepository _userFavoriteRepository;

        public UserFavoriteController(INotificador notificador, HttpClient httpClient, IUserFavoriteRepository userFavoriteRepository) : base(notificador)
        {
            _httpClient = httpClient;
            _userFavoriteRepository = userFavoriteRepository;
        }

        [HttpGet("ListUserFavorite")]
        [ProducesResponseType(typeof(List<UserFavorite>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<UserFavorite>>> ListUserFavorites(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                NotificarErro("O ID do usuário não pode ser vazio ou nulo.");
                return CustomResponse();
            }

            var result = _userFavoriteRepository.Buscar(userId);

            return CustomResponse(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(UserFavorite), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddToUserFavorites([FromBody] UserFavoriteViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return CustomResponse();

            if (viewModel.UserId == Guid.Empty)
            {
                NotificarErro("O ID do usuário não foi fornecido.");
            }
            if(string.IsNullOrWhiteSpace(viewModel.NomeCidade)){
                NotificarErro("O Nome da Cidade não foi fornecido.");
            }

            if (!OperacaoValida())
                return CustomResponse();

            var favorite = new UserFavorite(viewModel.UserId)
            {
                NomeCidade = viewModel.NomeCidade
            };

            var result = await _userFavoriteRepository.Adicionar(favorite);

            if (result != System.Net.HttpStatusCode.OK)
                return CustomResponse(result);

            return CustomResponse(result);
        }
    }
}
