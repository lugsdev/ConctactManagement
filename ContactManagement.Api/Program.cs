using System.Data;
using System.Data.SqlClient;
using System.Text;
using ContactManagement.Application.Interfaces;
using ContactManagement.Application.Services;
using ContactManagement.Application.Validators;
using ContactManagement.Domain.Interfaces;
using ContactManagement.InfraStructure.Respositories;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddFluentValidation(config =>
    {
        config.RegisterValidatorsFromAssemblyContaining<ContactDtoValidator>();
    });

// Configure Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Desafio Fase 1 Grupo2", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization Header - utilize Bearer + seu token",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

string connectionString = builder.Configuration.GetConnectionString("ConnectionMarla");
builder.Services.AddTransient<IDbConnection>(db => new SqlConnection(connectionString));

builder.Services.AddScoped<IContactServices, ContactServices>();
builder.Services.AddScoped<IContactRepository, ContactRepository>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

var key = Encoding.ASCII.GetBytes(builder.Configuration.GetValue<string>("SecretJWT"));

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication(); // Não esqueça de adicionar o middleware de autenticação
app.UseAuthorization();

app.MapControllers();

app.Run();
