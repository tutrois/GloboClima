namespace GloboClima.App.ViewModels
{
    public class ErrorViewModel
    {
        public int ErroCode { get; set; }
        public string Titulo { get; set; }
        public string Mensagem { get; set; }
    }

    public class ResponseResult
    {
        public bool success { get; set; }
        public List<string> Errors { get; set; }
    }
}
