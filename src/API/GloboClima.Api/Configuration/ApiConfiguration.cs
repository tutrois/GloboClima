using GloboClima.API.Configuration.Identity.Models;
using GloboClima.API.Configuration.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using GloboClima.API.Configuration.Jwt.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using GloboClima.API.Configuration.ApiExternal.OpenWeatherMap.Models;

namespace GloboClima.API.Configuration
{
    /// <summary>
    /// Configurações gerais da API, incluindo controle de comportamento de API e AutoMapper.
    /// </summary>
    public static class ApiConfiguration
    {
        /// <summary>
        /// Adiciona a configuração da API ao <see cref="WebApplicationBuilder"/>.
        /// </summary>
        /// <param name="builder">Instância do <see cref="WebApplicationBuilder"/>.</param>
        /// <returns>Retorna o <see cref="WebApplicationBuilder"/> com as configurações aplicadas.</returns>

        public static WebApplicationBuilder AddApiConfiguration(this WebApplicationBuilder builder) 
        {
            builder.Services.AddControllers()
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.SuppressModelStateInvalidFilter = true;
                });

            // Configuração Auto Mapper
            builder.Services.AddAutoMapper(typeof(StartupBase));

            // Configuração de Injeção de Dependências
            builder.Services.ResolveDependencies();

            return builder;
        }
    }

    /// <summary>
    /// Configurações do CORS para a aplicação.
    /// </summary>
    public static class CorsConfiguration
    {
        /// <summary>
        /// Adiciona as configurações de CORS ao <see cref="WebApplicationBuilder"/>.
        /// </summary>
        /// <param name="builder">Instância do <see cref="WebApplicationBuilder"/>.</param>
        /// <returns>Retorna o <see cref="WebApplicationBuilder"/> com as configurações aplicadas.</returns>

        public static WebApplicationBuilder AddCorsConfiguration(this WebApplicationBuilder builder)
        {
            // Configurações do CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("Development", builder =>
                    builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
                options.AddPolicy("Production", builder =>
                    builder
                        .WithOrigins("https://localhost:9000")
                        .WithMethods("POST")
                        .AllowAnyHeader());
            });

            return builder;
        }
    }

    /// <summary>
    /// Configurações do Swagger para documentação da API.
    /// </summary>
    public static class SwaggerConfiguration
    {
        /// <summary>
        /// Adiciona a configuração do Swagger ao <see cref="WebApplicationBuilder"/>.
        /// </summary>
        /// <param name="builder">Instância do <see cref="WebApplicationBuilder"/>.</param>
        /// <returns>Retorna o <see cref="WebApplicationBuilder"/> com as configurações aplicadas.</returns>

        public static WebApplicationBuilder AddSwaggerConfiguration(this WebApplicationBuilder builder)
        {
            builder.Services.AddEndpointsApiExplorer();

            // Configurações do Swagger
            builder.Services.AddSwaggerGen(options => {
                options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Description = "Forneça o token JWT no formato: 'Bearer {seu_token}'",
                    Name = "Authorization",
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]
                        {

                        }
                    }
                });
            });

            return builder;
        }
    }

    /// <summary>
    /// Configurações do Identity para autenticação de usuários.
    /// </summary>
    public static class IdentityConfiguration
    {
        /// <summary>
        /// Adiciona a configuração do Identity ao <see cref="WebApplicationBuilder"/>.
        /// </summary>
        /// <param name="builder">Instância do <see cref="WebApplicationBuilder"/>.</param>
        /// <returns>Retorna o <see cref="WebApplicationBuilder"/> com as configurações aplicadas.</returns>

        public static WebApplicationBuilder AddIdentityConfiguration(this WebApplicationBuilder builder)
        {
            // Configuração do Identity
            builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddTokenProvider<DataProtectorTokenProvider<ApplicationUser>>(TokenOptions.DefaultProvider)
                .AddDefaultUI()
                .AddErrorDescriber<IdentityTranslateMessages>();

            return builder;
        }
    }

    /// <summary>
    /// Configurações para autenticação JWT.
    /// </summary>
    public static class JwtConfiguration
    {
        /// <summary>
        /// Adiciona a configuração JWT ao <see cref="WebApplicationBuilder"/>.
        /// </summary>
        /// <param name="builder">Instância do <see cref="WebApplicationBuilder"/>.</param>
        /// <returns>Retorna o <see cref="WebApplicationBuilder"/> com as configurações aplicadas.</returns>
        public static WebApplicationBuilder AddJwtConfiguration(this WebApplicationBuilder builder)
        {
            //Gerando Token e chave encodada
            var JwtSettingsSection = builder.Configuration.GetSection("JwtSettings");
            builder.Services.Configure<JwtSettings>(JwtSettingsSection);

            var jwtSettings = JwtSettingsSection.Get<JwtSettings>();
            var key = Encoding.ASCII.GetBytes(s: jwtSettings.Secret);

            //Autenticação
            builder.Services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options => {
                options.RequireHttpsMetadata = true;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = jwtSettings.ValidTo,
                    ValidIssuer = jwtSettings.Issuer,
                };
            });

            return builder;
        }
    }

    public static class ApiExternalConfigurations
    {

        public static WebApplicationBuilder AddApiExternalConfigurations(this WebApplicationBuilder builder)
        {
            //Gerando Token e chave encodada
            var OpenWeatherSettingsSection = builder.Configuration.GetSection("OpenWeatherMapSettings");
            builder.Services.Configure<ApiOpenWeatherSettings>(OpenWeatherSettingsSection);

            return builder;
        }
    }
}


