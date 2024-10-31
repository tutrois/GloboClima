using GloboClima.App.Extensions;
using GloboClima.App.Service;
using GloboClima.App.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GloboClima.App.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IWeatherService _weatherService;
        private readonly IUser _appUser;

        public HomeController(IWeatherService weatherService, IUser appUser)
        {
            _weatherService = weatherService;
            _appUser = appUser;
        }

        public async Task<IActionResult> Index()
        {
            if (_appUser.EstaAutenticado())
            {
                return RedirectToAction("Index", "HomeDash");
            }

            var resposta = await _weatherService.GetListFiveCurrentWeatherDataAsync();

            return View(resposta.Data);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Route("erro/{id:length(3,3)}")]
        public IActionResult Error(int id)
        {
            var modelErro = new ErrorViewModel();

            if (id == 500)
            {
                modelErro.Mensagem = "Ocorreu um erro! Tente novamente mais tarde ou contate nosso suporte.";
                modelErro.Titulo = "Ocorreu um erro!";
                modelErro.ErroCode = id;
            }
            else if (id == 404)
            {
                modelErro.Mensagem =
                    "A página que está procurando não existe! <br />Em caso de dúvidas entre em contato com nosso suporte";
                modelErro.Titulo = "Ops! Página não encontrada.";
                modelErro.ErroCode = id;
            }
            else if (id == 403)
            {
                modelErro.Mensagem = "Você não tem permissão para fazer isto.";
                modelErro.Titulo = "Acesso Negado";
                modelErro.ErroCode = id;
            }
            else
            {
                return StatusCode(404);
            }

            return View("Error", modelErro);
        }
    }
}
