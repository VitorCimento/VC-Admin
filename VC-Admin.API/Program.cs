using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using VC_Admin.Application.Mapping;
using VC_Admin.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Serviços da Aplicação (Infrastructure, DbContext, repositórios, scoped services)
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddRepositories(builder.Configuration);
builder.Services.AddScopedServices(builder.Configuration);
builder.Services.AddAutoMapper(opt => { opt.AddProfile(typeof(MappingProfile)); });

// JWT - Configuração
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
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };

        opts.RequireHttpsMetadata = Convert.ToBoolean(builder.Configuration["Jwt:RequireHttpsMetadata"]); // PRD é TRUE
        //opts.SaveToken = true;
    });

builder.Services.AddAuthorization();
builder.Services.AddControllers();

// Swagger
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
