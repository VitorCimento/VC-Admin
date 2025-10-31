using VC_Admin.Application.Mapping;
using VC_Admin.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAutoMapper(opt => { opt.AddProfile(typeof(MappingProfile)); });

builder.Services.ConfigureInfrastructure(builder.Configuration);
builder.Services.ConfigureRepositories(builder.Configuration);
builder.Services.ConfigureScopedServices(builder.Configuration);
builder.Services.ConfigureAuthentication(builder.Configuration);
builder.Services.ConfigureAuthorization(builder.Configuration);
builder.Services.ConfigureSwagger(builder.Configuration);

builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "VC-Admin API v1");

        // Tentar habilitar envio de cookies na UI (pode variar por versão do Swashbuckle)
        // Se não compilar, remover este bloco
        try
        {
            c.ConfigObject.AdditionalItems["requestCredentials"] = "include";
        }
        catch { /* ignora se não suportado */ }
    });
}

// Middlewares
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
