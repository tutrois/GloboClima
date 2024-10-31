using Amazon.DynamoDBv2.DataModel;
using System.ComponentModel.DataAnnotations;

namespace GloboClima.API.Data.Models
{
    [DynamoDBTable("GloboClima")]
    public class UserFavorite
    {
        public UserFavorite()
        {
        }

        public UserFavorite(Guid userId)
        {
            UserId = userId;
            PartitionKey = $"USER#{UserId}"; // Gera o valor da chave de partição
        }
        /// <summary>
        /// Chave de partição no DynamoDB.
        /// </summary>
        [Key]
        [DynamoDBHashKey("pk")]
        public string PartitionKey { get; set; }

        /// <summary>
        /// Chave de ordenação no DynamoDB.
        /// </summary>
        [DynamoDBRangeKey("sk")]
        public string SortKey { get; set; } = "FAVORITE";

        /// <summary>
        /// ID único do usuário.
        /// </summary>
        [DynamoDBProperty]
        public Guid UserId { get; set; }

        [DynamoDBProperty]
        [StringLength(100, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 3)]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string NomeCidade { get; set; }

    }
}
