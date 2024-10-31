using GloboClima.App.Configuration.Authentication.ViewModels;
using GloboClima.App.Data.Reponses;
using GloboClima.App.Extensions;
using GloboClima.App.Service;
using GloboClima.App.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GloboClima.App.Controllers
{
    [Authorize]
    public class HomeDashController : BaseController
    {
        private readonly IWeatherService _weatherService;
        private readonly IUserFavoriteService _userFavoriteService;
        private readonly IUser _appUser;


        public HomeDashController(IWeatherService weatherService, IUserFavoriteService userFavoriteService, IUser appUser)
        {
            _weatherService = weatherService;
            _userFavoriteService = userFavoriteService;
            _appUser = appUser;
        }

        [HttpGet]
        [Route("HomeDash")]
        public async Task<IActionResult> Index(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            var resultListFavorite = await _userFavoriteService.GetListFavoritesAsync(_appUser.ObterUserId());

            if (resultListFavorite == null)
                return View();

            var result = await _weatherService.GetListCurrentWeatherDataAsync(resultListFavorite.Data.Select(e=>e.NomeCidade).ToList());

            var vireModel = new HomeDashViewModel
            {
                ListWeatherResponse = result.Data
            };

            return View(vireModel);
        }

        [HttpPost]
        [Route("HomeDash")]
        public async Task<IActionResult> Index(HomeDashViewModel viewModel, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (!ModelState.IsValid) return View();

            var userFavorite = new UserFavorite()
            {
                UserId = _appUser.ObterUserId(),
                NomeCidade = viewModel.Nome,
            };

            var resposta = await _userFavoriteService.AddToFavoriteAsync(userFavorite);

            if (ResponsePossuiErros(resposta.Data.ReponseResult)) return View();

            if (string.IsNullOrEmpty(returnUrl)) return RedirectToAction("Index", "HomeDash");
            return View(returnUrl);
        }
    }
}
