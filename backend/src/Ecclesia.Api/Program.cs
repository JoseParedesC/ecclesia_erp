using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Scalar.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Ecclesia.Infrastructure.Data;
using Ecclesia.Api.Middleware;
// repositories
using Ecclesia.Domain.Repositories;
// repositories implementations
using Ecclesia.Infrastructure.Repositories;
// handlers
using Ecclesia.Application.Users.Queries;
using Ecclesia.Application.Users.Commands.CreateUser;
using Ecclesia.Application.Users.Commands.UpdateUser;
using Ecclesia.Application.Users.Commands.DeleteUser;
// endpoints
using Ecclesia.Api.Endpoints.Users;
using Ecclesia.Application.Users.Queries.GetAllUsers;
using Ecclesia.Domain.Common.Constants.Permissions;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Add database context
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
        .UseSnakeCaseNamingConvention());

// Add authentication and authorization
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(EcclesiaPermissions.USER.CREATE, policy =>
        policy.RequireClaim("permission", EcclesiaPermissions.USER.CREATE));

    options.AddPolicy(EcclesiaPermissions.USER.READ, policy =>
        policy.RequireClaim("permission", EcclesiaPermissions.USER.READ));

    options.AddPolicy(EcclesiaPermissions.USER.UPDATE, policy =>
        policy.RequireClaim("permission", EcclesiaPermissions.USER.UPDATE));

    options.AddPolicy(EcclesiaPermissions.USER.DELETE, policy =>
        policy.RequireClaim("permission", EcclesiaPermissions.USER.DELETE));
});

// FluentValidation
builder.Services.AddValidatorsFromAssembly(typeof(GetUserByIdValidator).Assembly);
builder.Services.AddValidatorsFromAssembly(typeof(GetAllUsersValidator).Assembly);
builder.Services.AddValidatorsFromAssembly(typeof(CreateUserValidator).Assembly);
builder.Services.AddValidatorsFromAssembly(typeof(UpdateUserValidator).Assembly);
builder.Services.AddValidatorsFromAssembly(typeof(DeleteUserValidator).Assembly);

// Handlers
builder.Services.AddScoped<GetUserByIdHandler>();
builder.Services.AddScoped<GetAllUsersHandler>();
builder.Services.AddScoped<CreateUserHandler>();
builder.Services.AddScoped<UpdateUserHandler>();
builder.Services.AddScoped<DeleteUserHandler>();

// register repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

// Middleware for global error handling
app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication(); // autenticación
app.UseAuthorization();  // autorización

// Map endpoints
app.MapUsersEndpoints();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
