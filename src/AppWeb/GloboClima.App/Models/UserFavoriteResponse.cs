using System.ComponentModel.DataAnnotations;

namespace GloboClima.App.Models
{
    public class UserFavorite
    {
        public Guid UserId { get; set; }

        public string NomeCidade { get; set; }
        public ResponseResult ?ReponseResult { get; set; }
    }
}
