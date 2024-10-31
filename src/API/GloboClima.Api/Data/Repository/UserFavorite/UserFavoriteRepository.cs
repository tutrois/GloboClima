using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using GloboClima.API.Data.Models;
using System.Net;

namespace GloboClima.API.Data.Repository
{
    public class UserFavoriteRepository : IUserFavoriteRepository
    {
        private readonly IDynamoDBContext _context;

        public UserFavoriteRepository(IDynamoDBContext context)
        {
            _context = context;
        }

        public async Task<HttpStatusCode> Adicionar(UserFavorite userFavorite)
        {
            try
            {
                await _context.SaveAsync(userFavorite);
                return HttpStatusCode.OK; // Indica que a inserção foi bem-sucedida
            }
            catch (Exception ex)
            {
                // Verifica o tipo de exceção para definir o status code adequado
                if (ex is ConditionalCheckFailedException)
                    return HttpStatusCode.Conflict;
                if (ex is ProvisionedThroughputExceededException)
                    return HttpStatusCode.ServiceUnavailable;
                if (ex is InvalidOperationException)
                    return HttpStatusCode.BadRequest; // Retorna erro 400 se houver problema de configuração

                return HttpStatusCode.InternalServerError; // Retorna status 500 para outros erros
            }
        }

        public async Task<HttpStatusCode> Atualizar(UserFavorite userFavorite)
        {
            try
            {
                await _context.SaveAsync(userFavorite);
                return HttpStatusCode.OK; // Indica que a inserção foi bem-sucedida
            }
            catch (Exception ex)
            {
                // Verifica o tipo de exceção para definir o status code adequado
                if (ex is ConditionalCheckFailedException)
                    return HttpStatusCode.Conflict;
                if (ex is ProvisionedThroughputExceededException)
                    return HttpStatusCode.ServiceUnavailable;
                if (ex is InvalidOperationException)
                    return HttpStatusCode.BadRequest; // Retorna erro 400 se houver problema de configuração

                return HttpStatusCode.InternalServerError; // Retorna status 500 para outros erros
            }
        }

        public async Task<IEnumerable<UserFavorite>> Buscar(string userId)
        {
            var lista = await _context.QueryAsync<UserFavorite>(
                $"USER#{userId}", Amazon.DynamoDBv2.DocumentModel.QueryOperator.Equal,
                new object[] { "FAVORITE" })
           .GetRemainingAsync();
            return lista;
        }

        public async Task<UserFavorite?> Buscar(string userId, string nome)
        {
            var scanConditions = new List<ScanCondition>
            {
                new ScanCondition("NomeCidade", ScanOperator.Equal, nome)
            };

            var lista = await _context.ScanAsync<UserFavorite>(scanConditions).GetRemainingAsync();

            return lista.FirstOrDefault();
        }

        public async Task<HttpStatusCode> Deletar(UserFavorite userFavorite)
        {
            try
            {
                await _context.DeleteAsync<UserFavorite>(userFavorite);
                return HttpStatusCode.OK; // Indica que a inserção foi bem-sucedida
            }
            catch (Exception ex)
            {
                // Verifica o tipo de exceção para definir o status code adequado
                if (ex is ConditionalCheckFailedException)
                    return HttpStatusCode.Conflict;
                if (ex is ProvisionedThroughputExceededException)
                    return HttpStatusCode.ServiceUnavailable;
                if (ex is InvalidOperationException)
                    return HttpStatusCode.BadRequest; // Retorna erro 400 se houver problema de configuração

                return HttpStatusCode.InternalServerError; // Retorna status 500 para outros erros
            }
        }
    }
}
