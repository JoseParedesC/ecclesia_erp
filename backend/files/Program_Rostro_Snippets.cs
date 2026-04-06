// ============================================================
// ROSTRO MODULE — snippets para agregar en Program.cs
// ============================================================
// Pegar cada bloque en la sección correspondiente de tu Program.cs

// ── 1. Repositorios ──────────────────────────────────────────
builder.Services.AddScoped<IRostroRepository, RostroRepository>();

// ── 2. Handlers ──────────────────────────────────────────────
builder.Services.AddScoped<CreateRostroHandler>();
builder.Services.AddScoped<UpdateRostroHandler>();
builder.Services.AddScoped<DeactivateRostroHandler>();
builder.Services.AddScoped<GetRostroByIdHandler>();
builder.Services.AddScoped<ListRostrosHandler>();

// ── 3. Políticas de autorización ─────────────────────────────
// Agregar dentro del bloque AddAuthorization existente:
options.AddPolicy(EcclesiaPermissions.Rostros.Read,
    p => p.RequireClaim("permission", EcclesiaPermissions.Rostros.Read));
options.AddPolicy(EcclesiaPermissions.Rostros.Create,
    p => p.RequireClaim("permission", EcclesiaPermissions.Rostros.Create));
options.AddPolicy(EcclesiaPermissions.Rostros.Update,
    p => p.RequireClaim("permission", EcclesiaPermissions.Rostros.Update));
options.AddPolicy(EcclesiaPermissions.Rostros.Deactivate,
    p => p.RequireClaim("permission", EcclesiaPermissions.Rostros.Deactivate));

// ── 4. Endpoints (después de app.Build()) ────────────────────
app.MapRostrosEndpoints();

// ── 5. Usings necesarios ─────────────────────────────────────
// using Ecclesia.Api.Constants;
// using Ecclesia.Api.Endpoints.Rostros;
// using Ecclesia.Application.Rostros.Commands.CreateRostro;
// using Ecclesia.Application.Rostros.Commands.DeactivateRostro;
// using Ecclesia.Application.Rostros.Commands.UpdateRostro;
// using Ecclesia.Application.Rostros.Queries.GetRostroById;
// using Ecclesia.Application.Rostros.Queries.ListRostros;
// using Ecclesia.Domain.Repositories;
// using Ecclesia.Infrastructure.Repositories;
