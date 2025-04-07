using System.Text.Json.Serialization;
using Microsoft.OpenApi.Models;
using RegistrationFormAPI.BusinessLayer.Interfaces;
using RegistrationFormAPI.BusinessLayer.Services;
using RegistrationFormAPI.DataAccessLayer.Interfaces;
using RegistrationFormAPI.DataAccessLayer.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Register dependencies
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

// Add logging
builder.Services.AddLogging();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Registration Form API",
        Version = "v1",
        Description = "ASP.NET Core Web API for user registration"
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();