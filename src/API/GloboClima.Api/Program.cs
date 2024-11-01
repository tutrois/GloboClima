using GloboClima.API.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.AddApiConfiguration()
        .AddCorsConfiguration()
        .AddSwaggerConfiguration()
        .AddIdentityConfiguration()
        .AddJwtConfiguration()
        .AddApiExternalConfigurations();

builder.Services.AddHttpClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors("Development");
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors("Production");
}

// Redireciona requisições HTTP para HTTPS, garantindo criptografia dos dados
app.UseHttpsRedirection();

app.MapGet("/", () => "API Globo Clima iniciou corretamente e contém o swagger habilitado.");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
