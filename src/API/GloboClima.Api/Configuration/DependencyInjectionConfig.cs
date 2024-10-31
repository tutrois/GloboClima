using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2;
using Microsoft.AspNetCore.Identity;
using GloboClima.API.Configuration.Identity.Models;
using GloboClima.API.Configuration.Identity.Data;
using GloboClima.API.Configuration.Notificacoes;
using GloboClima.API.Repository;

namespace GloboClima.API.Configuration
{
    /// <summary>
    /// Classe responsável pela configuração de injeção de dependências da aplicação.
    /// </summary>
    public static class DependencyInjectionConfig
    {
        /// <summary>
        /// Registra as dependências necessárias no contêiner de injeção de dependência.
        /// </summary>
        /// <param name="services">A coleção de serviços a serem configurados.</param>
        /// <returns>A coleção de serviços configurada.</returns>
        public static IServiceCollection ResolveDependencies(this IServiceCollection services) 
        {
            // Registro de serviços para Amazon DynamoDB
            services.AddSingleton<IAmazonDynamoDB, AmazonDynamoDBClient>();
            services.AddSingleton<IDynamoDBContext, DynamoDBContext>();

            // Registro de serviços para notificações e identidade
            services.AddScoped<INotificador, Notificador>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IUserFavoriteRepository, UserFavoriteRepository>();

            services.AddScoped<IUserStore<ApplicationUser>, UserStore>();
            services.AddScoped<IRoleStore<ApplicationRole>, RoleStore>();

            return services;
        }
    }
}
