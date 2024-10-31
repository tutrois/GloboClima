using System.ComponentModel.DataAnnotations;

namespace GloboClima.API.Configuration.Authentication.ViewModels
{
    /// <summary>
    /// Representa o modelo de dados para o registro de uma nova função (role) para um usuário.
    /// </summary>
    public class RegisterRoleViewModel
    {   
        /// <summary>Obtém ou define o ID do usuário ao qual a função será atribuída.</summary>
        /// <remarks>Este campo é obrigatório./// </remarks>
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string UserId { get; set; }

        /// <summary>Obtém ou define o nome da função a ser registrada.</summary>
        /// <remarks>Este campo é obrigatório.</remarks>
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string Name { get; set; }
    }
}
