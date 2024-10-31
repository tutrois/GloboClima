using Amazon.DynamoDBv2.DataModel;
using System.ComponentModel.DataAnnotations;

namespace GloboClima.API.Configuration.Identity.Models
{
    /// <summary>
    /// Representa um usuário da aplicação armazenado no DynamoDB.
    /// </summary>
    [DynamoDBTable("GloboClima")]
    public class ApplicationUser
    {
        /// <summary>
        /// Construtor padrão.
        /// </summary>
        public ApplicationUser() { }

        /// <summary>
        /// Construtor com ID do usuário.
        /// </summary>
        /// <param name="userId">O ID do usuário.</param>
        public ApplicationUser(Guid userId)
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
        public string SortKey { get; set; } = "METADATA";

        /// <summary>
        /// ID único do usuário.
        /// </summary>
        [DynamoDBProperty]
        public Guid UserId { get; set; }

        /// <summary>
        /// Nome de usuário.
        /// </summary>
        [DynamoDBProperty]
        public string UserName { get; set; }
        /// <summary>
        /// Nome de usuário normalizado.
        /// </summary>
        [DynamoDBProperty]
        public string NormalizedUserName { get; set; }

        /// <summary>
        /// Email do usuário.
        /// </summary>
        [DynamoDBProperty]
        public string Email { get; set; }

        /// <summary>
        /// Email normalizado do usuário.
        /// </summary>
        [DynamoDBProperty]
        public string NormalizedEmail { get; set; }

        /// <summary>
        /// Indica se o email foi confirmado.
        /// </summary>
        [DynamoDBProperty]
        public bool EmailConfirmed { get; set; }

        /// <summary>
        /// Hash da senha do usuário.
        /// </summary>
        [DynamoDBProperty]
        public string PasswordHash { get; set; }

        /// <summary>
        /// Número de telefone do usuário.
        /// </summary>
        [DynamoDBProperty]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Indica se o número de telefone foi confirmado.
        /// </summary>
        [DynamoDBProperty]
        public bool PhoneNumberConfirmed { get; set; }

        /// <summary>
        /// Indica se a autenticação de dois fatores está habilitada.
        /// </summary>
        [DynamoDBProperty]
        public bool TwoFactorEnabled { get; set; }
    }
}
