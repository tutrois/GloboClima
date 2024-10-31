using Amazon.DynamoDBv2.DataModel;

namespace GloboClima.API.Configuration.Identity.Models 
{
    /// <summary>
    /// Representa um papel (role) da aplicação armazenado no DynamoDB.
    /// </summary>
    [DynamoDBTable("GloboClima")]
    public class ApplicationRole 
    {
        /// <summary>
        /// Chave de partição no DynamoDB.
        /// </summary>
        [DynamoDBHashKey("pk")]
        public string PartitionKey { get; set; }

        /// <summary>
        /// Chave de ordenação no DynamoDB.
        /// </summary>
        [DynamoDBRangeKey("sk")]
        public string SortKey { get; set; } = "ROLE";

        /// <summary>
        /// Nome do papel.
        /// </summary>
        [DynamoDBProperty]
        public string Name { get; set; }

        /// <summary>
        /// Nome normalizado do papel.
        /// </summary>
        [DynamoDBProperty]
        public string NormalizedName { get; set; }
    }
}
