using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Scalar.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Ecclesia.Infrastructure.Data;
using Ecclesia.Api.Middleware;
using Ecclesia.Domain.Repositories;
using Ecclesia.Infrastructure.Repositories;
using Ecclesia.Application.Auth.Services;
using Ecclesia.Api.Endpoints.Users;
using Ecclesia.Domain.Common.Constants.Permissions;
using Ecclesia.Api.Endpoints.Roles;
using Ecclesia.Infrastructure.Auth;
using Ecclesia.Api.Endpoints.Auth;
using Ecclesia.Domain.Interfaces;
using Ecclesia.Api.Endpoints.Incomes;
using Ecclesia.Api.Endpoints.ThirdParty;
using Ecclesia.Api.Endpoints.Accounts;

// handlers
using Ecclesia.Application.Users.Queries;
using Ecclesia.Application.Users.Queries.GetAllUsers;
using Ecclesia.Application.Users.Commands.CreateUser;
using Ecclesia.Application.Users.Commands.UpdateUser;
using Ecclesia.Application.Users.Commands.DeleteUser;
using Ecclesia.Application.Roles.Commands.CreateRole;
using Ecclesia.Application.Roles.Commands.AssignRoleToUser;
using Ecclesia.Application.Auth.Commands.Login;
using Ecclesia.Application.Auth.Queries.Me;
using Ecclesia.Application.Incomes.Commands.CreateIncome;
using Ecclesia.Application.ThirdParty.Queries.ListThirdParties;
using Ecclesia.Application.ThirdParty.Queries.GetThirdPartyById;
using Ecclesia.Application.ThirdParty.Commands.CreateThirdParty;
using Ecclesia.Application.ThirdParty.Commands.UpdateThirdParty;
using Ecclesia.Application.ThirdParty.Commands.DeleteThirdParty;
using Ecclesia.Application.Accounts.Queries.ListAccounts;
using Ecclesia.Application.Accounts.Commands.CreateAccount;
using Ecclesia.Application.Accounts.Queries.GetAccountById;
using Ecclesia.Application.Accounts.Commands.UpdateAccount;
using Ecclesia.Application.Accounts.Commands.DeleteAccount;
using Ecclesia.Application.Accounts.Queries.SearchAccounts;
using Ecclesia.Application.AccountingPeriods.Queries.ListAccountingPeriods;
using Ecclesia.Application.AccountingPeriods.Queries.GetAccountingPeriodById;
using Ecclesia.Application.AccountingPeriods.Commands.CreateAccountingPeriod;
using Ecclesia.Application.AccountingPeriods.Commands.CloseAccountingPeriod;
using Ecclesia.Application.AccountingPeriods.Commands.ReopenAccountingPeriod;
using Ecclesia.Api.Endpoints.AccountingPeriods;
using Ecclesia.Application.Roles.Queries.SearchRoles;
using Ecclesia.Application.Roles.Queries;
using Ecclesia.Application.Roles.Queries.GetAllRoles;

var builder = WebApplication.CreateBuilder(args);

// ── OpenAPI ───────────────────────────────────────────────────────────────────
builder.Services.AddOpenApi();

// ── Database ──────────────────────────────────────────────────────────────────
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
           .UseSnakeCaseNamingConvention());

//CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});


// ── Authentication ────────────────────────────────────────────────────────────
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme    = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    var jwtKey = builder.Configuration["Jwt:Key"];

    if (string.IsNullOrWhiteSpace(jwtKey))
        throw new Exception("JWT Key is not configured");

    var key = Encoding.UTF8.GetBytes(jwtKey);

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer           = true,
        ValidateAudience         = true,
        ValidateLifetime         = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer              = builder.Configuration["Jwt:Issuer"],
        ValidAudience            = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey         = new SymmetricSecurityKey(key)
    };
});

// ── Authorization ─────────────────────────────────────────────────────────────
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(EcclesiaPermissions.USER.CREATE,  policy => policy.RequireClaim("permission", EcclesiaPermissions.USER.CREATE));
    options.AddPolicy(EcclesiaPermissions.USER.READ,    policy => policy.RequireClaim("permission", EcclesiaPermissions.USER.READ));
    options.AddPolicy(EcclesiaPermissions.USER.UPDATE,  policy => policy.RequireClaim("permission", EcclesiaPermissions.USER.UPDATE));
    options.AddPolicy(EcclesiaPermissions.USER.DELETE,  policy => policy.RequireClaim("permission", EcclesiaPermissions.USER.DELETE));

    options.AddPolicy(EcclesiaPermissions.ROLES.CREATE, policy => policy.RequireClaim("permission", EcclesiaPermissions.ROLES.CREATE));
    options.AddPolicy(EcclesiaPermissions.ROLES.READ,   policy => policy.RequireClaim("permission", EcclesiaPermissions.ROLES.READ));
    options.AddPolicy(EcclesiaPermissions.ROLES.UPDATE, policy => policy.RequireClaim("permission", EcclesiaPermissions.ROLES.UPDATE));
    options.AddPolicy(EcclesiaPermissions.ROLES.DELETE, policy => policy.RequireClaim("permission", EcclesiaPermissions.ROLES.DELETE));
    options.AddPolicy(EcclesiaPermissions.ROLES.ASSIGN, policy => policy.RequireClaim("permission", EcclesiaPermissions.ROLES.ASSIGN));

    options.AddPolicy(EcclesiaPermissions.THIRD_PARTIES.CREATE, policy => policy.RequireClaim("permission", EcclesiaPermissions.THIRD_PARTIES.CREATE));
    options.AddPolicy(EcclesiaPermissions.THIRD_PARTIES.READ,   policy => policy.RequireClaim("permission", EcclesiaPermissions.THIRD_PARTIES.READ));
    options.AddPolicy(EcclesiaPermissions.THIRD_PARTIES.UPDATE, policy => policy.RequireClaim("permission", EcclesiaPermissions.THIRD_PARTIES.UPDATE));
    options.AddPolicy(EcclesiaPermissions.THIRD_PARTIES.DELETE, policy => policy.RequireClaim("permission", EcclesiaPermissions.THIRD_PARTIES.DELETE));

    options.AddPolicy(EcclesiaPermissions.INCOME.CREATE, policy => policy.RequireClaim("permission", EcclesiaPermissions.INCOME.CREATE));
    options.AddPolicy(EcclesiaPermissions.INCOME.READ,   policy => policy.RequireClaim("permission", EcclesiaPermissions.INCOME.READ));
    options.AddPolicy(EcclesiaPermissions.INCOME.UPDATE, policy => policy.RequireClaim("permission", EcclesiaPermissions.INCOME.UPDATE));
    options.AddPolicy(EcclesiaPermissions.INCOME.DELETE, policy => policy.RequireClaim("permission", EcclesiaPermissions.INCOME.DELETE));
});

// ── FluentValidation ──────────────────────────────────────────────────────────
// Un solo llamado cubre todos los validators del ensamblado Application
builder.Services.AddValidatorsFromAssemblyContaining<ListAccountsValidator>();

// ── Repositories ──────────────────────────────────────────────────────────────
builder.Services.AddScoped<IUserRepository,             UserRepository>();
builder.Services.AddScoped<IRoleRepository,             RoleRepository>();
builder.Services.AddScoped<IUserRoleRepository,         UserRoleRepository>();
builder.Services.AddScoped<IAuthRepository,             AuthRepository>();
builder.Services.AddScoped<IIncomeRepository,           IncomeRepository>();
builder.Services.AddScoped<IJournalVoucherRepository,   JournalVoucherRepository>();
builder.Services.AddScoped<ICommunityRepository,        CommunityRepository>();
builder.Services.AddScoped<IAccountingPeriodRepository, AccountingPeriodRepository>();
builder.Services.AddScoped<IThirdPartyRepository,       ThirdPartyRepository>();
builder.Services.AddScoped<IAccountRepository,          AccountRepository>();

// ── Services ──────────────────────────────────────────────────────────────────
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ITokenService,        TokenService>();
builder.Services.AddScoped<ICurrentUserService,  CurrentUserService>();

// ── Handlers ──────────────────────────────────────────────────────────────────
//User
builder.Services.AddScoped<GetUserByIdHandler>();
builder.Services.AddScoped<GetAllUsersHandler>();
builder.Services.AddScoped<CreateUserHandler>();
builder.Services.AddScoped<UpdateUserHandler>();
builder.Services.AddScoped<DeleteUserHandler>();
//Role
builder.Services.AddScoped<CreateRoleHandler>();
builder.Services.AddScoped<AssignRoleToUserHandler>();
builder.Services.AddScoped<SearchRolesHandler>();
builder.Services.AddScoped<GetRoleByIdHandler>();
builder.Services.AddScoped<GetAllRolesHandler>();
//Auth
builder.Services.AddScoped<LoginHandler>();
builder.Services.AddScoped<MeHandler>();
//Incomes
builder.Services.AddScoped<CreateIncomeHandler>();
//ThirdParties
builder.Services.AddScoped<ListThirdPartiesHandler>();
builder.Services.AddScoped<GetThirdPartyByIdHandler>();
builder.Services.AddScoped<CreateThirdPartyHandler>();
builder.Services.AddScoped<UpdateThirdPartyHandler>();
builder.Services.AddScoped<DeleteThirdPartyHandler>();
//Account
builder.Services.AddScoped<ListAccountsHandler>();
builder.Services.AddScoped<CreateAccountHandler>();
builder.Services.AddScoped<GetAccountByIdHandler>();
builder.Services.AddScoped<UpdateAccountHandler>();
builder.Services.AddScoped<DeleteAccountHandler>();
builder.Services.AddScoped<SearchAccountsHandler>();
//AccountingPeriod
builder.Services.AddScoped<ListAccountingPeriodsHandler>();
builder.Services.AddScoped<GetAccountingPeriodByIdHandler>();
builder.Services.AddScoped<CreateAccountingPeriodHandler>();
builder.Services.AddScoped<CloseAccountingPeriodHandler>();
builder.Services.AddScoped<ReopenAccountingPeriodHandler>();

// ── Pipeline ──────────────────────────────────────────────────────────────────
var app = builder.Build();

app.UseCors("AllowFrontend");

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// ── Endpoints ─────────────────────────────────────────────────────────────────
app.MapUsersEndpoints();
app.MapRolesEndpoints();
app.MapAuthEndpoints();
app.MapIncomesEndpoints();
app.MapThirdPartyEndpoints();
app.MapAccountEndpoints();
app.MapAccountingPeriodEndpoints();

// Endpoint de desarrollo para generar hash de contraseña
if (app.Environment.IsDevelopment())
{
    app.MapGet("/dev/hash/{password}", (string password) =>
        Results.Ok(new { hash = BCrypt.Net.BCrypt.HashPassword(password) })
    ).AllowAnonymous();
}

// Seed inicial
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await DatabaseSeeder.SeedAsync(context);
}

app.Run();