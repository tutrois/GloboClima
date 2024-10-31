using GloboClima.App.Configuration.Authentication.ViewModels;
using GloboClima.App.Service;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;

namespace GloboClima.App.Controllers
{
    public class AuthController : BaseController
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        [Route("Registro")]

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [Route("Registro")]
        public async Task<IActionResult> Register(RegisterUserViewModel usuarioRegistro)
        {
            if(!ModelState.IsValid) return View(usuarioRegistro);

            //API - Registro
            var resposta = await _authService.Register(usuarioRegistro);
            
            if(ResponsePossuiErros(resposta.Data.ReponseResult)) return View(usuarioRegistro);

            await _authService.DoLogin((ApiResponse<UserLoginResponse>)resposta);

            // Realizar Login na App
            return RedirectToAction("Index", "HomeDash");
        }

        [HttpGet]
        [Route("Login")]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginUserViewModel usuarioLogin, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (!ModelState.IsValid) return View(usuarioLogin);

            //API - Login
            var resposta = await _authService.Login(usuarioLogin);

            if (ResponsePossuiErros(resposta.Data.ReponseResult)) return View(usuarioLogin);

            await _authService.DoLogin((ApiResponse<UserLoginResponse>)resposta);

            if (string.IsNullOrEmpty(returnUrl)) return RedirectToAction("Index", "HomeDash");
            return View(returnUrl);
        }

        [HttpGet]
        [Route("Sair")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");

        }
    }
}
