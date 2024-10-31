namespace GloboClima.API.Configuration.Notificacoes
{
    /// <summary>
    /// Implementa a interface <see cref="INotificador"/> para gerenciar notificações na aplicação.
    /// </summary>
    public class Notificador : INotificador
    {
        private List<Notificacao> _notificacoes;

        /// <summary>
        /// Inicializa uma nova instância da classe <see cref="Notificador"/> e cria uma lista vazia de notificações.
        /// </summary>
        public Notificador()
        {
            _notificacoes = new List<Notificacao>();
        }

        /// <summary>
        /// Adiciona uma nova notificação à lista de notificações.
        /// </summary>
        /// <param name="notificacao">A notificação a ser adicionada.</param>
        public void AdicionarNotificacao(Notificacao notificacao)
        {
            _notificacoes.Add(notificacao);
        }

        /// <summary>
        /// Obtém a lista de notificações registradas.
        /// </summary>
        /// <returns>Uma lista de objetos <see cref="Notificacao"/>.</returns>
        public List<Notificacao> ObterNotificacoes()
        {
            return _notificacoes;
        }

        /// <summary>
        /// Verifica se existem notificações pendentes.
        /// </summary>
        /// <returns><c>true</c> se houver notificações; caso contrário, <c>false</c>.</returns>
        public bool TemNotificacao()
        {
            return _notificacoes.Any();
        }

        /// <summary>
        /// Limpa todas as notificações registradas.
        /// </summary>
        public void LimparNotificacoes()
        {
            _notificacoes.Clear();
        }
    }
}
