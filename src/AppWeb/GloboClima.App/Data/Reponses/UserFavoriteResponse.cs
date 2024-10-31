using GloboClima.App.ViewModels;
using System.Text.Json.Serialization;

namespace GloboClima.App.Data.Reponses
{
    public class UserFavorite
    {
        public Guid UserId { get; set; }

        public string NomeCidade { get; set; }

        [JsonIgnore]
        public ResponseResult? ReponseResult { get; set; }
    }
}
