﻿using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using GloboClima.API.Configuration.Identity.Models;
using GloboClima.API.Data.Models;
using System.Net;
using System.Threading;

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

        public async Task<IEnumerable<UserFavorite>> Buscar(Guid userId)
        {
            try
            {
                var scanConditions = new List<ScanCondition>
            {
                new ScanCondition("UserId", ScanOperator.Equal, userId.ToString()),
                new ScanCondition("SortKey", ScanOperator.Equal, new UserFavorite().SortKey)
            };

            var lista = await _context.ScanAsync<UserFavorite>(scanConditions).GetRemainingAsync();
            return lista;

            }
            catch (Exception ex)
            {
                // Log do erro para diagnóstico
                Console.WriteLine($"Erro ao executar a operação ScanAsync: {ex.Message}");
                // Retorna uma lista vazia em caso de erro
                return new List<UserFavorite>();
            }
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
