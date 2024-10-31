namespace GloboClima.API.Configuration.Authentication.ViewModels
{
    /// <summary> 
    /// Representa um token de usuário contendo informações relevantes. 
    /// </summary>
    public class UserTokenViewModel
    {
        /// <summary>Obtém ou define o ID do usuário.</summary>
        public string Id { get; set; }

        /// <summary>Obtém ou define o e-mail do usuário.</summary>
        public string Email { get; set; }

        /// <summary>Obtém ou define as claims associadas ao usuário.</summary>
        public IEnumerable<ClaimViewModel> Claims { get; set; }
    }
    /// <summary>
    /// Representa a resposta de login contendo o token de acesso e informações do usuário.
    /// </summary>
    public class LoginResponseViewModel
    {
        /// <summary>Obtém ou define o token de acesso.</summary>
        public string AccessToken { get; set; }

        /// <summary>Obtém ou define a duração da validade do token em segundos.</summary>
        public double ExpiresIn { get; set; }

        /// <summary>Obtém ou define o token do usuário associado à resposta de login.</summary>
        public UserTokenViewModel UserToken { get; set; }
    }

    /// <summary>
    /// Representa uma claim associada ao usuário, com tipo e valor.
    /// </summary>
    public class ClaimViewModel
    {
        /// <summary>Obtém ou define o valor da claim. </summary>
        public string Value { get; set; }
        /// <summary>Obtém ou define o tipo da claim.</summary>
        public string Type { get; set; }
    }
}
