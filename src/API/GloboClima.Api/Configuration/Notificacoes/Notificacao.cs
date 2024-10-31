namespace GloboClima.API.Configuration.Notificacoes
{
    /// <summary>
    /// Representa uma notificação que pode ser registrada na aplicação.
    /// </summary>
    public class Notificacao
    {
        /// <summary>
        /// Inicializa uma nova instância da classe <see cref="Notificacao"/> com a mensagem especificada.
        /// </summary>
        /// <param name="mensagem">A mensagem da notificação.</param>
        public Notificacao(string mensagem)
        {
            Mensagem = mensagem;
        }

        /// <summary>
        /// Obtém a mensagem da notificação.
        /// </summary>
        public string Mensagem { get; }
    }
}
