using System.ComponentModel.DataAnnotations;

namespace GloboClima.API.Configuration.Authentication.ViewModels
{
    /// <summary>
    /// Representa o modelo de dados para o registro de um novo usuário.
    /// </summary>
    public class RegisterUserViewModel
    {
        /// <summary> Obtém ou define o e-mail do usuário.</summary>
        /// <remarks>Este campo é obrigatório e deve estar em um formato válido de e-mail.</remarks>
        [EmailAddress(ErrorMessage = "O campo {0} está em um formato inválido")]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string Email { get; set; }

        /// <summary>Obtém ou define a senha do usuário. </summary>
        /// <remarks> Este campo é obrigatório e deve ter entre 6 e 100 caracteres. </remarks>
        [StringLength(100, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 6)]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string Password { get; set; }

        /// <summary>Obtém ou define a confirmação da senha do usuário.</summary>
        /// <remarks>Este campo deve ser igual à senha fornecida.</remarks>
        [Compare("Password", ErrorMessage = "As senhas não conferem.")]
        public string ConfirmPassword { get; set; }
    }
}
