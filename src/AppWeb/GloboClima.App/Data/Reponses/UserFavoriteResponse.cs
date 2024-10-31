using GloboClima.App.ViewModels;

namespace GloboClima.App.Data.Reponses
{
    public class UserFavorite
    {
        public Guid UserId { get; set; }

        public string NomeCidade { get; set; }
        public ResponseResult? ReponseResult { get; set; }
    }
}
