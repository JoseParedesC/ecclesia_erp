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
using Ecclesia.Application.Roles.Commands.CreateRole;
using Ecclesia.Application.Roles.Commands.AssignRoleToUser;
using Ecclesia.Application.Auth.Commands.Login;
using Ecclesia.Application.Auth.Queries.Me;
using Ecclesia.Application.Auth.Services;
// endpoints
using Ecclesia.Api.Endpoints.Users;
using Ecclesia.Application.Users.Queries.GetAllUsers;
using Ecclesia.Domain.Common.Constants.Permissions;
using Ecclesia.Api.Endpoints.Roles;
using Ecclesia.Infrastructure.Auth;
using Ecclesia.Api.Endpoints.Auth;
using Ecclesia.Domain.Interfaces;
using Ecclesia.Application.Incomes.Commands.CreateIncome;
using Ecclesia.Api.Endpoints.Incomes;
using Ecclesia.Api.Endpoints.ThirdParty;
using Ecclesia.Application.ThirdParty.Queries.ListThirdParties;
using Ecclesia.Application.ThirdParty.Queries.GetThirdPartyById;
using Ecclesia.Application.ThirdParty.Commands.CreateThirdParty;
using Ecclesia.Application.ThirdParty.Commands.UpdateThirdParty;
using Ecclesia.Application.ThirdParty.Commands.DeleteThirdParty;


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
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer           = true,
        ValidateAudience         = true,
        ValidateLifetime         = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer              = builder.Configuration["Jwt:Issuer"],
        ValidAudience            = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey         = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
    };
});

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


    //
    options.AddPolicy(EcclesiaPermissions.ROLES.CREATE, policy =>
        policy.RequireClaim("permission", EcclesiaPermissions.ROLES.CREATE));

    options.AddPolicy(EcclesiaPermissions.ROLES.READ, policy =>
        policy.RequireClaim("permission", EcclesiaPermissions.ROLES.READ));

    options.AddPolicy(EcclesiaPermissions.ROLES.UPDATE, policy =>
        policy.RequireClaim("permission", EcclesiaPermissions.ROLES.UPDATE));

    options.AddPolicy(EcclesiaPermissions.ROLES.DELETE, policy =>
        policy.RequireClaim("permission", EcclesiaPermissions.ROLES.DELETE));

    options.AddPolicy(EcclesiaPermissions.ROLES.ASSIGN, policy =>
        policy.RequireClaim("permission", EcclesiaPermissions.ROLES.ASSIGN));
});

// FluentValidation
builder.Services.AddValidatorsFromAssembly(typeof(GetUserByIdValidator).Assembly);
builder.Services.AddValidatorsFromAssembly(typeof(GetAllUsersValidator).Assembly);
builder.Services.AddValidatorsFromAssembly(typeof(CreateUserValidator).Assembly);
builder.Services.AddValidatorsFromAssembly(typeof(UpdateUserValidator).Assembly);
builder.Services.AddValidatorsFromAssembly(typeof(DeleteUserValidator).Assembly);


// register repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IUserRoleRepository, UserRoleRepository>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IIncomeRepository, IncomeRepository>();
builder.Services.AddScoped<IJournalVoucherRepository, JournalVoucherRepository>();
builder.Services.AddScoped<ICommunityRepository, CommunityRepository>();
builder.Services.AddScoped<IAccountingPeriodRepository, AccountingPeriodService>();
builder.Services.AddScoped<IThirdPartyRepository, ThirdPartyRepository>();


builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();


// Handlers
builder.Services.AddScoped<GetUserByIdHandler>();
builder.Services.AddScoped<GetAllUsersHandler>();
builder.Services.AddScoped<CreateUserHandler>();
builder.Services.AddScoped<UpdateUserHandler>();
builder.Services.AddScoped<DeleteUserHandler>();
builder.Services.AddScoped<CreateRoleHandler>();
builder.Services.AddScoped<AssignRoleToUserHandler>();
builder.Services.AddScoped<LoginHandler>();
builder.Services.AddScoped<MeHandler>();
builder.Services.AddScoped<CreateIncomeHandler>();
builder.Services.AddScoped<ListThirdPartiesHandler>();
builder.Services.AddScoped<GetThirdPartyByIdHandler>();
builder.Services.AddScoped<CreateThirdPartyHandler>();
builder.Services.AddScoped<UpdateThirdPartyHandler>();
builder.Services.AddScoped<DeleteThirdPartyHandler>();


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
app.MapRolesEndpoints();
app.MapAuthEndpoints();
app.MapIncomesEndpoints();
app.MapThirdPartyEndpoints();

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
