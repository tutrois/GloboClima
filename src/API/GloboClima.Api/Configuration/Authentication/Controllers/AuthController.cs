using GloboClima.API.Configuration.Authentication.ViewModels;
using GloboClima.API.Configuration.Identity.Models;
using GloboClima.API.Configuration.Jwt.Models;
using GloboClima.API.Configuration.Notificacoes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GloboClima.API.Configuration.Authentication.Controllers
{
    [ApiController]
    [Route("api/login")]
    public class AuthController : BaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly JwtSettings _jwtSettings;

        public AuthController(INotificador notificador, 
            UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager, 
            IOptions<JwtSettings> jwtSettings) : base(notificador)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtSettings = jwtSettings.Value;
        }
        /// <summary>Registra um novo usuário no sistema. </summary>
        /// <param name="registerUser">O modelo contendo as informações de registro do usuário.</param>
        /// <returns> Um <see cref="ActionResult"/> que representa o resultado do processo de registro. Se bem-sucedido, retorna um token JWT; caso contrário, retorna erros de validação ou dados de registro. </returns>
        /// <remarks> Este método verifica primeiro se o modelo de registro fornecido é válido. Se for válido, cria um novo usuário e tenta registrá-lo no sistema. 
        /// Após o registro bem-sucedido, o usuário é autenticado automaticamente e um token JWT é gerado.</remarks>
        [HttpPost("Registrar")]
        public async Task<ActionResult> Register(RegisterUserViewModel registerUser)
        {
            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            var user = new ApplicationUser(Guid.NewGuid())
            {
                UserName = registerUser.Email,
                NormalizedUserName = registerUser.Email.ToUpper(),
                Email = registerUser.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, registerUser.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                return CustomResponse(await GerarJwt(user.Email));
            }

            foreach (var error in result.Errors)
            {
                NotificarErro(error.Description);
            }

            return CustomResponse(registerUser);
        }

        /// <summary>Faz login de um usuário existente no sistema.</summary>
        /// <param name="loginUser">O modelo contendo as informações de login do usuário.</param>
        /// <returns> Um <see cref="ActionResult"/> que representa o resultado do processo de login. Se bem-sucedido, retorna um token JWT; caso contrário, retorna erros de validação ou dados de login.</returns>
        /// <remarks> Este método verifica se o modelo de login fornecido é válido.Se for válido, tenta autenticar o usuário usando seu e-mail e senha.
        /// Após o login bem-sucedido, um token JWT é gerado. Se o usuário estiver bloqueado devido a várias tentativas de login falhadas, uma mensagem de erro apropriada é retornada.
        /// Se o login falhar por qualquer outro motivo, uma mensagem de erro é fornecida indicando o problema. </remarks>
        [HttpPost("Entrar")]
        public async Task<ActionResult> Login(LoginUserViewModel loginUser)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _signInManager.PasswordSignInAsync(loginUser.Email, loginUser.Password, false, false);

            if (result.Succeeded)
            {
                return CustomResponse(await GerarJwt(loginUser.Email));
            }
            if (result.IsLockedOut)
            {
                NotificarErro("Usuário temporariamente bloqueado por tentativas inválidas");
                return CustomResponse(loginUser);
            }

            NotificarErro("Usuário ou Senha incorretos");
            return CustomResponse(loginUser);
        }

        /// <summary>Registra uma nova role para um usuário específico.</summary>
        /// <param name="registerRole">O modelo contendo as informações da role a ser registrada.</param>
        /// <returns> Um <see cref="ActionResult"/> que representa o resultado do processo de registro da role.
        /// Se bem-sucedido, retorna os resultados da operação; caso contrário, retorna erros de validação ou informações sobre a falha. </returns>
        /// <remarks> Este método verifica se o modelo fornecido para registro da role é válido.
        /// Se for válido, busca o usuário pelo ID e tenta adicionar a nova role ao usuário. Se a operação for bem-sucedida, os resultados são retornados.
        /// Caso contrário, mensagens de erro são coletadas e retornadas. Apenas usuários com a role "ADMIN" podem executar este método.</remarks>
        [Authorize(Roles = "ADMIN")]
        [HttpPost("RegistrarRole")]
        public async Task<ActionResult> RegisterRole(RegisterRoleViewModel registerRole)
        {
            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            var user = await _userManager.FindByIdAsync(registerRole.UserId);

            var result = await _userManager.AddToRoleAsync(user, registerRole.Name);

            if (result.Succeeded)
            {
                return CustomResponse(result);
            }

            foreach (var error in result.Errors)
            {
                NotificarErro(error.Description);
            }

            return CustomResponse(result);
        }

        /// <summary>Gera um token JWT para o usuário especificado.</summary>
        /// <param name="email">O e-mail do usuário para o qual o token JWT será gerado.</param>
        /// <returns> Uma <see cref="Task{LoginResponseViewModel}"/> representando o resultado da geração do token.
        /// Retorna um objeto <see cref="LoginResponseViewModel"/> contendo o token e informações do usuário. </returns>
        private async Task<LoginResponseViewModel> GerarJwt(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var claims = new List<Claim>();

            var identityClaims = await GenerateUserClaims(claims, user);
            var encodedToken = EncodeToken(identityClaims);

            return ResponseToken(encodedToken, user, claims);
        }

        /// <summary> Gera as claims do usuário a partir de suas roles e informações.</summary>
        /// <param name="claims">Uma coleção de claims a serem adicionadas.</param>
        /// <param name="user">O usuário para o qual as claims serão geradas.</param>
        /// <returns>Uma <see cref="Task{ClaimsIdentity}"/> representando a identidade do usuário com suas claims.</returns>
        private async Task<ClaimsIdentity> GenerateUserClaims(ICollection<Claim> claims, ApplicationUser user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);

            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, ToUnixEpochDate(DateTime.UtcNow).ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64));

            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var identityClaims = new ClaimsIdentity();
            identityClaims.AddClaims(claims);

            return identityClaims;
        }

        /// <summary> Codifica as claims do usuário em um token JWT. </summary>
        /// <param name="identityClaims">A identidade do usuário contendo suas claims.</param>
        /// <returns> Uma string representando o token JWT codificado.</returns>
        private string EncodeToken(ClaimsIdentity identityClaims)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);

            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.ValidTo,
                Subject = identityClaims,
                Expires = DateTime.UtcNow.AddHours(_jwtSettings.ExpirationHours),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            });

            return tokenHandler.WriteToken(token);
        }

        /// <summary> Cria um modelo de resposta de login contendo o token e informações do usuário. </summary>
        /// <param name="encodedToken">O token JWT codificado.</param>
        /// <param name="user">O usuário cujas informações serão incluídas na resposta.</param>
        /// <param name="claims">As claims associadas ao usuário.</param>
        /// <returns> Um objeto <see cref="LoginResponseViewModel"/> contendo o token e informações do usuário. </returns>
        private LoginResponseViewModel ResponseToken(string encodedToken, ApplicationUser user, IEnumerable<Claim> claims)
        {
            return new LoginResponseViewModel
            {
                AccessToken = encodedToken,
                ExpiresIn = TimeSpan.FromHours(_jwtSettings.ExpirationHours).TotalSeconds,
                UserToken = new UserTokenViewModel
                {
                    Id = user.UserId.ToString(),
                    Email = user.Email,
                    Claims = claims.Select(c => new ClaimViewModel { Type = c.Type, Value = c.Value })
                }
            };
        }

        /// <summary> Converte uma data para o formato de data Unix Epoch. </summary>
        /// <param name="date">A data a ser convertida.</param>
        /// <returns> Um long representando a data no formato Unix Epoch. </returns>
        private static long ToUnixEpochDate(DateTime date)
          => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);
    }
}
