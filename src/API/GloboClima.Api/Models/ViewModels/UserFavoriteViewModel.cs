using Amazon.DynamoDBv2.DataModel;
using System.ComponentModel.DataAnnotations;

namespace GloboClima.API.Models.ViewModels
{
    public class UserFavoriteViewModel
    {
        public Guid UserId { get; set; }

        public string NomeCidade { get; set; }
    }
}
