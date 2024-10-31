using GloboClima.App.Extensions;
using GloboClima.App.Service;
using Microsoft.AspNetCore.Identity;

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
            services.AddHttpClient<IAuthService, AuthService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IUser, AspNetUser>();

            services.AddSingleton<IWeatherService, WeatherService>();
            services.AddSingleton<IUserFavoriteService, UserFavoriteService>();


            return services;
        }
    }
}
