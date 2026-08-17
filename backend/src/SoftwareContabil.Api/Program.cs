using System.Text;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SoftwareContabil.Api.Middleware;
using SoftwareContabil.Application;
using SoftwareContabil.Infrastructure;
using SoftwareContabil.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// ---------- Controllers ----------
builder.Services.AddControllers().AddJsonOptions(opt =>
{
    opt.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    opt.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
});

// ---------- Camadas (cada uma registra suas próprias dependências) ----------
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

// ---------- Autenticação JWT ----------
var jwtKey = builder.Configuration["Jwt:Key"]
    ?? throw new InvalidOperationException("Configuração 'Jwt:Key' não encontrada.");

// Falha rápido e com mensagem clara em vez de um erro críptico do middleware JWT
// mais tarde, caso alguém esqueça de trocar a chave de exemplo ou use uma curta demais.
if (jwtKey.Length < 32)
{
    throw new InvalidOperationException(
        "'Jwt:Key' precisa ter pelo menos 32 caracteres (HMAC-SHA256 exige uma chave forte). " +
        "Configure um valor seguro via variável de ambiente ou 'dotnet user-secrets' antes de rodar a API.");
}

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
    };
});
builder.Services.AddAuthorization();

// ---------- CORS (libera os frontends em dev — React e Vue podem rodar ao mesmo tempo em portas diferentes) ----------
var frontendOrigins = (builder.Configuration["Frontend:Urls"] ?? "http://localhost:5173,http://localhost:5174")
    .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy.WithOrigins(frontendOrigins)
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// ---------- Rate limiting (protege o login contra força bruta) ----------
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    options.AddFixedWindowLimiter("auth", opt =>
    {
        opt.PermitLimit = 10;                    // até 10 tentativas...
        opt.Window = TimeSpan.FromMinutes(1);     // ...por minuto...
        opt.QueueLimit = 0;                       // ...sem fila: excedeu, rejeita direto (429)
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    });
});

// ---------- Health checks ----------
builder.Services.AddHealthChecks()
    .AddDbContextCheck<AppDbContext>("postgres");

// ---------- Swagger / OpenAPI ----------
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Software Contábil API",
        Version = "v1",
        Description = "API REST do sistema de gestão contábil (financeiro, patrimônio, estoque, compra/venda e gerencial), " +
                       "organizada em camadas (Domain / Application / Infrastructure / Api)."
    });

    var jwtScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Informe: Bearer {seu token JWT}",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
    };
    options.AddSecurityDefinition("Bearer", jwtScheme);
    options.AddSecurityRequirement(new OpenApiSecurityRequirement { { jwtScheme, Array.Empty<string>() } });
});

var app = builder.Build();

// ---------- Pipeline ----------
app.UseAppExceptionHandling(); // primeiro: captura exceções de qualquer camada abaixo

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Software Contábil API v1");
    options.RoutePrefix = "swagger";
});

app.UseHttpsRedirection();
app.UseCors("Frontend");
app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHealthChecks("/health");

app.Run();
