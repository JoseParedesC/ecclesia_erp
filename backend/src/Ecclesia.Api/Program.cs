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

var builder = WebApplication.CreateBuilder(args);

// ── OpenAPI ───────────────────────────────────────────────────────────────────
builder.Services.AddOpenApi();

// ── Database ──────────────────────────────────────────────────────────────────
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
           .UseSnakeCaseNamingConvention());

// ── Authentication ────────────────────────────────────────────────────────────
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme    = JwtBearerDefaults.AuthenticationScheme;
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
builder.Services.AddScoped<IAccountingPeriodRepository, AccountingPeriodService>();
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

// ── Pipeline ──────────────────────────────────────────────────────────────────
var app = builder.Build();

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

app.Run();