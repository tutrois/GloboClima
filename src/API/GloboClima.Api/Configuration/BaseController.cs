using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;
using GloboClima.API.Configuration.Notificacoes;
using System.Text.Json;

namespace GloboClima.API.Configuration
{
    /// <summary>
    /// Classe base para controladores, fornecendo funcionalidades de notificação e resposta personalizada.
    /// </summary>
    public class BaseController : ControllerBase
    {
        private readonly INotificador _notificador;

        /// <summary>
        /// Inicializa uma nova instância da classe <see cref="BaseController"/>.
        /// </summary>
        /// <param name="notificador">Instância de <see cref="INotificador"/> para gerenciar notificações.</param>
        public BaseController(INotificador notificador)
        {
            _notificador = notificador;
        }

        /// <summary>
        /// Verifica se a operação é válida, ou seja, se não há notificações.
        /// </summary>
        /// <returns>Retorna <c>true</c> se a operação é válida; caso contrário, <c>false</c>.</returns>
        protected bool OperacaoValida()
        {
            return !_notificador.TemNotificacao();
        }

        /// <summary>
        /// Retorna uma resposta personalizada com os dados ou erros.
        /// </summary>
        /// <param name="result">Dados a serem retornados na resposta.</param>
        /// <returns>Um <see cref="ActionResult"/> representando a resposta da operação.</returns>
        protected ActionResult CustomResponse(object result = null)
        {
            if (OperacaoValida())
            {
                return Ok(new
                {
                    success = true,
                    data = result
                });
            }

            return BadRequest(new
            {
                success = false,
                errors = _notificador.ObterNotificacoes().Select(n => n.Mensagem)
            });
        }

        /// <summary>
        /// Retorna uma resposta personalizada baseada no estado do modelo.
        /// </summary>
        /// <param name="modelState">O estado do modelo a ser verificado.</param>
        /// <returns>Um <see cref="ActionResult"/> representando a resposta da operação.</returns>
        protected ActionResult CustomResponse(ModelStateDictionary modelState)
        {
            if (!modelState.IsValid) NotificarErroModelInvalida(modelState);
            return CustomResponse();
        }

        /// <summary>
        /// Notifica erros encontrados no estado do modelo inválido.
        /// </summary>
        /// <param name="modelState">O estado do modelo contendo os erros.</param>
        protected void NotificarErroModelInvalida(ModelStateDictionary modelState)
        {
            var erros = modelState.Values.SelectMany(e => e.Errors);
            foreach (var erro in erros)
            {
                var errorMsg = erro.Exception == null ? erro.ErrorMessage : erro.Exception.Message;
                NotificarErro(errorMsg);
            }
        }

        /// <summary>
        /// Notifica um erro com uma mensagem específica.
        /// </summary>
        /// <param name="mensagem">A mensagem de erro a ser notificada.</param>
        protected void NotificarErro(string mensagem)
        {
            _notificador.AdicionarNotificacao(new Notificacao(mensagem));
        }

        /// <summary>
        /// Limpa as notificações de erro atuais.
        /// </summary>
        protected void LimparErro()
        {
            _notificador.LimparNotificacoes();
        }

        protected async Task<T> DeserializarObjetoResponse<T>(HttpResponseMessage responseMessage)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            return JsonSerializer.Deserialize<T>(await responseMessage.Content.ReadAsStringAsync(), options);
        }
    }
}

