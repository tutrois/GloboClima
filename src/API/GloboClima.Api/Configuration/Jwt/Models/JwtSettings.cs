namespace GloboClima.API.Configuration.Jwt.Models
{
    /// <summary>
    /// Representa as configurações para o JWT (JSON Web Token).
    /// </summary>
    public class JwtSettings
    {
        /// <summary>
        /// Obtém ou define o segredo usado para a assinatura do token.
        /// </summary>
        public string Secret { get; set; }
        /// <summary>
        /// Obtém ou define a duração de expiração do token em horas.
        /// </summary>
        public int ExpirationHours { get; set; }
        /// <summary>
        /// Obtém ou define o emissor do token.
        /// </summary>
        public string Issuer { get; set; }
        /// <summary>
        /// Obtém ou define o público (audience) para o qual o token é válido.
        /// </summary>
        public string ValidTo { get; set; }
    }
}
