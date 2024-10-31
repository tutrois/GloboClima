using GloboClima.App.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GloboClima.App.Controllers
{
    public class BaseController : Controller
    {
        protected bool ResponsePossuiErros(ResponseResult resposta)
        {
            if (resposta != null && resposta.Errors.Any())
            {
                foreach (var mensagem in resposta.Errors)
                {
                    ModelState.AddModelError(string.Empty, mensagem);
                }

                return true;
            }

            return false;
        }

        protected bool ResponsePossuiErros(IEnumerable<ResponseResult> respostas)
        {
            foreach (var resposta in respostas)
            {
                var result = ResponsePossuiErros(resposta);

                if (result)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
