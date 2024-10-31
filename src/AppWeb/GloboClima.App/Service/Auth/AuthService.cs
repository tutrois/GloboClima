using GloboClima.App.Configuration.Authentication.ViewModels;
using GloboClima.App.Extensions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using GloboClima.App.ViewModels;

namespace GloboClima.App.Service
{
    public class AuthService : BaseService, IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AppSettings _settings;
        private readonly IAuthenticationService _authenticationService;
        private readonly IUser _user;


        public AuthService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor, IOptions<AppSettings> _settings, IAuthenticationService authenticationService, IUser user)
        {
            httpClient.BaseAddress = new Uri(_settings.Value.AutenticacaoUrl);

            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
            _authenticationService = authenticationService;
            _user = user;
        }

        public async Task<ApiResponse<UserLoginResponse>> Login(LoginUserViewModel viewModel)
        {
            var loginContent = ObterConteudo(viewModel);

            var response = await _httpClient.PostAsync("/api/login/Entrar", loginContent);

            // Verifique se a resposta foi bem-sucedida antes de tentar ler o conteúdo
            if (!TratarErrosResponse(response))
            {
                var teste = await response.Content.ReadAsStringAsync();

                return new ApiResponse<UserLoginResponse>
                {
                    Data = new UserLoginResponse
                    {
                        ReponseResult = await DeserializarObjetoResponse<ResponseResult>(response)
                    }
                };
            }

            return await DeserializarObjetoResponse<ApiResponse<UserLoginResponse>>(response);
        }

        public async Task<ApiResponse<UserLoginResponse>> Register(RegisterUserViewModel viewModel)
        {
            var registerContent = ObterConteudo(viewModel);

            var response = await _httpClient.PostAsync("/api/login/Registrar", registerContent);

            // Verifique se a resposta foi bem-sucedida antes de tentar ler o conteúdo
            if (!TratarErrosResponse(response))
            {
                var teste = await DeserializarObjetoResponse<ResponseResult>(response);
                return new ApiResponse<UserLoginResponse>
                {
                    Data = new UserLoginResponse
                    {

                        ReponseResult = await DeserializarObjetoResponse<ResponseResult>(response)
                    }
                };
            }

            return await DeserializarObjetoResponse<ApiResponse<UserLoginResponse>>(response);

        }

        public async Task DoLogin(ApiResponse<UserLoginResponse> resposta)
        {
            var token = FormatToken(resposta.Data.AccessToken);

            var claims = new List<Claim>();
            claims.Add(new Claim("JWT", resposta.Data.AccessToken));
            claims.AddRange(token.Claims);

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8),
                IsPersistent = true
            };

            await _authenticationService.SignInAsync(
                _user.ObterHttpContext(),
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }
        public static JwtSecurityToken FormatToken(string jwtToken)
        {
            return new JwtSecurityTokenHandler().ReadToken(jwtToken) as JwtSecurityToken;
        }
    }
}
