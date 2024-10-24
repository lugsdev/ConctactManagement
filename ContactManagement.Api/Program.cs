using System.Data;
using System.Data.SqlClient;
using ContactManagement.Api.Configuration;
using ContactManagement.Api.Extensions;
using ContactManagement.Application.Interfaces;
using ContactManagement.Application.Services;
using ContactManagement.Domain.Interfaces;
using ContactManagement.InfraStructure.Respositories;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ValidateModelStateFilter>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerConfig();

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

string connectionString = builder.Configuration.GetConnectionString("ConnectionRafael");

builder.Services.AddTransient<IDbConnection>( db => new SqlConnection(connectionString));
builder.Services.AddScoped<IContactServices, ContactServices>();
builder.Services.AddScoped<IContactRepository, ContactRepository>();
builder.Services.AddScoped<INotificationService, NotificationService>();


var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerConfig();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();