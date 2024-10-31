namespace GloboClima.API.Configuration.Notificacoes
{
    /// <summary>
    /// Define os métodos para a notificação de eventos na aplicação.
    /// </summary>
    public interface INotificador
    {
        /// <summary>
        /// Verifica se existem notificações pendentes.
        /// </summary>
        /// <returns><c>true</c> se houver notificações; caso contrário, <c>false</c>.</returns>
        bool TemNotificacao();

        /// <summary>
        /// Obtém a lista de notificações registradas.
        /// </summary>
        /// <returns>Uma lista de objetos <see cref="Notificacao"/>.</returns>
        List<Notificacao> ObterNotificacoes();

        /// <summary>
        /// Adiciona uma nova notificação à lista.
        /// </summary>
        /// <param name="notificacao">A notificação a ser adicionada.</param>
        void AdicionarNotificacao(Notificacao notificacao);

        /// <summary>
        /// Limpa todas as notificações registradas.
        /// </summary>
        void LimparNotificacoes();
    }
}
