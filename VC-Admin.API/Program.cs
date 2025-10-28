using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;
using System.Text;
using VC_Admin.Application.Interfaces.Services;
using VC_Admin.Application.Services;
using VC_Admin.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

#region Infrastructure (DbContext, repositórios)
builder.Services.AddInfrastructure(builder.Configuration);
#endregion

#region Serviços / Automapper
builder.Services.AddScoped<IAuthService, AuthService>();
#endregion

#region JWT - Configuração
var secret = builder.Configuration["Jwt:Secret"] ?? throw new InvalidOperationException("Jwt:Secret não foi configurado");
var key = Encoding.UTF8.GetBytes(secret);

builder.Services
    .AddAuthentication(opts =>
    {
        opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        opts.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(opts =>
    {
        opts.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                if (context.Request.Cookies.TryGetValue("jwt", out var cookieToken))
                    context.Token = cookieToken;

                return Task.CompletedTask;
            }
        };

        opts.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = !string.IsNullOrWhiteSpace(builder.Configuration["Jwt:Issuer"]),
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidateAudience = !string.IsNullOrWhiteSpace(builder.Configuration["Jwt:Audience"]),
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidateLifetime = true
        };

        opts.RequireHttpsMetadata = Convert.ToBoolean(builder.Configuration["Jwt:RequireHttpsMetadata"]); // PRD é TRUE
        opts.SaveToken = true;
    });

builder.Services.AddAuthorization();
builder.Services.AddControllers();
#endregion

#region Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API VC-Admin", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Use 'Bearer {token}' ou teste com cookie 'jwt'.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme { Reference = new OpenApiReference {Type = ReferenceType.SecurityScheme, Id = "Bearer"}},
            Array.Empty<string>()
        }
    });
});
#endregion
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "VC-Admin API v1");

        // Tentar habilitar envio de cookies na UI (pode variar por versão do Swashbuckle)
        // Se não compilar, remova esta linha.
        try
        {
            c.ConfigObject.AdditionalItems["requestCredentials"] = "include";
        }
        catch { /* ignora se não suportado */ }
    });
}

#region Middlewares
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
#endregion

//app.MapGet("/weatherforecast", () =>
//{
//    var forecast =  Enumerable.Range(1, 5).Select(index =>
//        new WeatherForecast
//        (
//            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
//            Random.Shared.Next(-20, 55),
//            summaries[Random.Shared.Next(summaries.Length)]
//        ))
//        .ToArray();
//    return forecast;
//})
//.WithName("GetWeatherForecast");

app.Run();
