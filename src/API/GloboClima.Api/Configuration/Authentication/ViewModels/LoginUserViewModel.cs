using System.ComponentModel.DataAnnotations;

namespace GloboClima.API.Configuration.Authentication.ViewModels
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
}
