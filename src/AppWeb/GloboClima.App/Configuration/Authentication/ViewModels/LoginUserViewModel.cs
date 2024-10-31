using GloboClima.App.Models;
using System.ComponentModel.DataAnnotations;

namespace GloboClima.App.Configuration.Authentication.ViewModels
{
    /// <summary>
    /// Representa o modelo de dados para login de um usuário.
    /// </summary>
    public class LoginUserViewModel
    {    /// <summary>Obtém ou define o e-mail do usuário.</summary>
         /// <remarks>Este campo é obrigatório e deve estar em um formato válido de e-mail.</remarks>
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [EmailAddress(ErrorMessage = "O campo {0} está em um formato inválido")]
        public string Email { get; set; }

        /// <summary>Obtém ou define a senha do usuário.</summary>
        /// <remarks>Este campo é obrigatório e deve ter entre 6 e 100 caracteres.</remarks>
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(100, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 6)]
        public string Password { get; set; }
    }

    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public T Data { get; set; }
    }

    public class UserLoginResponse
    {
        public string AccessToken { get; set; }
        public double ExpiresIn { get; set; }
        public UserToken UserToken { get; set; }
        public ResponseResult ReponseResult { get; set; }
    }

    public class UserToken
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public IEnumerable<UserClaim> Claims { get; set; }
    }

    public class UserClaim
    {
        public string Value { get; set; }
        public string Type { get; set; }
    }
}
