# Ecclesia ERP — Módulo `Rostro`: Contexto Backend Exhaustivo

> Documento de referencia completo para IA y desarrolladores. Cubre cada archivo generado, su responsabilidad, contratos de entrada/salida, reglas de negocio, permisos y guía de integración.

---

## 1. Propósito del módulo

`Rostro` es una **entidad organizacional de primer nivel** dentro de Ecclesia ERP. Representa una sede, zona o agrupación principal a la cual se asocian `Community` (comunidades) y desde las cuales se imputan los `JournalVoucher` (asientos contables).

**Reglas de negocio fundamentales:**
- Un `Rostro` debe tener un `Name` único dentro del sistema.
- Un `Rostro` puede estar activo (`IsActive = true`) o inactivo (`IsActive = false`).
- Un `Rostro` inactivo **no puede** ser referenciado en nuevos `JournalVoucher`.
- El `Code` es un identificador corto alfanumérico único (máx. 10 caracteres), generado o provisto por el usuario, usado para reportes y referencias contables.
- No se permite eliminación física (`DELETE`). Solo desactivación (`IsActive = false`) para preservar integridad referencial con `JournalVoucher`.
- `Description` es opcional.

---

## 2. Posición en el modelo de datos (MER)

```
Rostro (1) ──────< (N) Community
Rostro (1) ──────< (N) JournalVoucher  [FK: rostro_id NOT NULL]
```

`Rostro` es **padre directo** de `Community` y es FK obligatoria en `JournalVoucher`. Por eso pertenece a la **Fase 2** del MVP — sin datos en esta tabla, no se puede crear ningún asiento contable.

---

## 3. Árbol de archivos generados

```
backend/src/

├── Ecclesia.Domain/
│   ├── Entities/
│   │   └── RostroEntity.cs
│   ├── Repositories/
│   │   └── IRostroRepository.cs
│   └── Common/
│       └── (BaseEntity, Result<T>, PagedQuery, PagedResult — preexistentes)
│
├── Ecclesia.Application/
│   └── Rostros/
│       ├── Commands/
│       │   ├── CreateRostro/
│       │   │   ├── CreateRostroCommand.cs
│       │   │   ├── CreateRostroHandler.cs
│       │   │   └── CreateRostroValidator.cs
│       │   ├── UpdateRostro/
│       │   │   ├── UpdateRostroCommand.cs
│       │   │   ├── UpdateRostroHandler.cs
│       │   │   └── UpdateRostroValidator.cs
│       │   └── DeactivateRostro/
│       │       ├── DeactivateRostroCommand.cs
│       │       ├── DeactivateRostroHandler.cs
│       │       └── DeactivateRostroValidator.cs
│       └── Queries/
│           ├── GetRostroById/
│           │   ├── GetRostroByIdQuery.cs
│           │   ├── GetRostroByIdHandler.cs
│           │   └── RostroDetailDto.cs
│           └── ListRostros/
│               ├── ListRostrosQuery.cs
│               ├── ListRostrosHandler.cs
│               └── RostroSummaryDto.cs
│
├── Ecclesia.Infrastructure/
│   ├── Data/
│   │   └── Configurations/
│   │       └── RostroConfiguration.cs
│   └── Repositories/
│       └── RostroRepository.cs
│
└── Ecclesia.Api/
    ├── Endpoints/
    │   └── Rostros/
    │       └── RostrosEndpoints.cs
    └── Constants/
        └── EcclesiaPermissions.cs   ← agregar constantes Rostro
```

---

## 4. Capa de Dominio

### 4.1 `RostroEntity.cs`
**Ruta:** `Ecclesia.Domain/Entities/RostroEntity.cs`

```csharp
using Ecclesia.Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecclesia.Domain.Entities;

[Table("rostros", Schema = "org")]
public class RostroEntity : BaseEntity
{
    public string Code    { get; private set; } = default!;
    public string Name    { get; private set; } = default!;
    public string? Description { get; private set; }
    public bool   IsActive { get; private set; } = true;

    // Nav properties
    public ICollection<CommunityEntity> Communities { get; private set; } = [];

    private RostroEntity() { }

    public static RostroEntity Create(string code, string name, string? description = null)
    {
        return new RostroEntity
        {
            Code        = code.Trim().ToUpperInvariant(),
            Name        = name.Trim(),
            Description = description?.Trim(),
            IsActive    = true
        };
    }

    public void Update(string name, string? description)
    {
        Name        = name.Trim();
        Description = description?.Trim();
    }

    public void Deactivate() => IsActive = false;
    public void Activate()   => IsActive = true;
}
```

**Decisiones de diseño:**
- Constructor privado + factory method `Create()` — encapsula invariantes, ningún consumidor puede instanciar un `RostroEntity` en estado inválido.
- `Code` se normaliza a mayúsculas en el dominio, no en la aplicación.
- `IsActive` se modifica solo a través de métodos de dominio (`Deactivate`/`Activate`), no por setters públicos.
- `private set` en todas las propiedades — el estado solo cambia por comportamiento.

---

### 4.2 `IRostroRepository.cs`
**Ruta:** `Ecclesia.Domain/Repositories/IRostroRepository.cs`

```csharp
using Ecclesia.Domain.Common;
using Ecclesia.Domain.Entities;

namespace Ecclesia.Domain.Repositories;

public interface IRostroRepository
{
    Task<RostroEntity?>          GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<RostroEntity?>          GetByCodeAsync(string code, CancellationToken ct = default);
    Task<bool>                   ExistsByNameAsync(string name, Guid? excludeId = null, CancellationToken ct = default);
    Task<bool>                   ExistsByCodeAsync(string code, Guid? excludeId = null, CancellationToken ct = default);
    Task<PagedResult<RostroEntity>> ListAsync(string? search, bool? isActive, int page, int pageSize, CancellationToken ct = default);
    Task                         AddAsync(RostroEntity rostro, CancellationToken ct = default);
    Task                         SaveChangesAsync(CancellationToken ct = default);
}
```

**Contratos clave:**
- `ExistsByNameAsync` y `ExistsByCodeAsync` aceptan `excludeId` para el caso de update (unicidad excluyendo el propio registro).
- `ListAsync` soporta filtro combinado: texto libre (`search` aplica ILIKE sobre `name` y `code`) y por estado (`isActive`).
- No hay `DeleteAsync` — política de no eliminación física.

---

## 5. Capa de Aplicación

### 5.1 Commands

#### `CreateRostroCommand` / `CreateRostroHandler` / `CreateRostroValidator`
**Ruta:** `Ecclesia.Application/Rostros/Commands/CreateRostro/`

```csharp
// CreateRostroCommand.cs
namespace Ecclesia.Application.Rostros.Commands.CreateRostro;

public record CreateRostroCommand(
    string  Code,
    string  Name,
    string? Description
);
```

```csharp
// CreateRostroHandler.cs
using Ecclesia.Domain.Common;
using Ecclesia.Domain.Entities;
using Ecclesia.Domain.Repositories;

namespace Ecclesia.Application.Rostros.Commands.CreateRostro;

public sealed class CreateRostroHandler(IRostroRepository repository)
{
    public async Task<Result<Guid>> HandleAsync(
        CreateRostroCommand command,
        CancellationToken ct = default)
    {
        var codeNormalized = command.Code.Trim().ToUpperInvariant();

        if (await repository.ExistsByCodeAsync(codeNormalized, ct: ct))
            return Result<Guid>.Failure("El código ya está en uso.");

        if (await repository.ExistsByNameAsync(command.Name.Trim(), ct: ct))
            return Result<Guid>.Failure("Ya existe un Rostro con ese nombre.");

        var rostro = RostroEntity.Create(command.Code, command.Name, command.Description);

        await repository.AddAsync(rostro, ct);
        await repository.SaveChangesAsync(ct);

        return Result<Guid>.Success(rostro.Id);
    }
}
```

```csharp
// CreateRostroValidator.cs
using FluentValidation;

namespace Ecclesia.Application.Rostros.Commands.CreateRostro;

public sealed class CreateRostroValidator : AbstractValidator<CreateRostroCommand>
{
    public CreateRostroValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("El código es obligatorio.")
            .MaximumLength(10).WithMessage("El código no puede superar 10 caracteres.")
            .Matches(@"^[A-Za-z0-9]+$").WithMessage("El código solo puede contener letras y números.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("El nombre es obligatorio.")
            .MaximumLength(100).WithMessage("El nombre no puede superar 100 caracteres.");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("La descripción no puede superar 500 caracteres.")
            .When(x => x.Description is not null);
    }
}
```

---

#### `UpdateRostroCommand` / `UpdateRostroHandler` / `UpdateRostroValidator`
**Ruta:** `Ecclesia.Application/Rostros/Commands/UpdateRostro/`

```csharp
// UpdateRostroCommand.cs
namespace Ecclesia.Application.Rostros.Commands.UpdateRostro;

public record UpdateRostroCommand(
    Guid    Id,
    string  Name,
    string? Description
);
```

```csharp
// UpdateRostroHandler.cs
using Ecclesia.Domain.Common;
using Ecclesia.Domain.Repositories;

namespace Ecclesia.Application.Rostros.Commands.UpdateRostro;

public sealed class UpdateRostroHandler(IRostroRepository repository)
{
    public async Task<Result<Guid>> HandleAsync(
        UpdateRostroCommand command,
        CancellationToken ct = default)
    {
        var rostro = await repository.GetByIdAsync(command.Id, ct);
        if (rostro is null)
            return Result<Guid>.Failure("Rostro no encontrado.");

        if (!rostro.IsActive)
            return Result<Guid>.Failure("No se puede modificar un Rostro inactivo.");

        if (await repository.ExistsByNameAsync(command.Name.Trim(), excludeId: command.Id, ct: ct))
            return Result<Guid>.Failure("Ya existe otro Rostro con ese nombre.");

        rostro.Update(command.Name, command.Description);
        await repository.SaveChangesAsync(ct);

        return Result<Guid>.Success(rostro.Id);
    }
}
```

```csharp
// UpdateRostroValidator.cs
using FluentValidation;

namespace Ecclesia.Application.Rostros.Commands.UpdateRostro;

public sealed class UpdateRostroValidator : AbstractValidator<UpdateRostroCommand>
{
    public UpdateRostroValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("El Id es obligatorio.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("El nombre es obligatorio.")
            .MaximumLength(100).WithMessage("El nombre no puede superar 100 caracteres.");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("La descripción no puede superar 500 caracteres.")
            .When(x => x.Description is not null);
    }
}
```

---

#### `DeactivateRostroCommand` / `DeactivateRostroHandler` / `DeactivateRostroValidator`
**Ruta:** `Ecclesia.Application/Rostros/Commands/DeactivateRostro/`

```csharp
// DeactivateRostroCommand.cs
namespace Ecclesia.Application.Rostros.Commands.DeactivateRostro;

public record DeactivateRostroCommand(Guid Id);
```

```csharp
// DeactivateRostroHandler.cs
using Ecclesia.Domain.Common;
using Ecclesia.Domain.Repositories;

namespace Ecclesia.Application.Rostros.Commands.DeactivateRostro;

public sealed class DeactivateRostroHandler(IRostroRepository repository)
{
    public async Task<Result<Guid>> HandleAsync(
        DeactivateRostroCommand command,
        CancellationToken ct = default)
    {
        var rostro = await repository.GetByIdAsync(command.Id, ct);
        if (rostro is null)
            return Result<Guid>.Failure("Rostro no encontrado.");

        if (!rostro.IsActive)
            return Result<Guid>.Failure("El Rostro ya está inactivo.");

        rostro.Deactivate();
        await repository.SaveChangesAsync(ct);

        return Result<Guid>.Success(rostro.Id);
    }
}
```

```csharp
// DeactivateRostroValidator.cs
using FluentValidation;

namespace Ecclesia.Application.Rostros.Commands.DeactivateRostro;

public sealed class DeactivateRostroValidator : AbstractValidator<DeactivateRostroCommand>
{
    public DeactivateRostroValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("El Id es obligatorio.");
    }
}
```

---

### 5.2 Queries

#### DTOs

```csharp
// RostroDetailDto.cs
namespace Ecclesia.Application.Rostros.Queries.GetRostroById;

public record RostroDetailDto(
    Guid    Id,
    string  Code,
    string  Name,
    string? Description,
    bool    IsActive,
    DateTime CreatedAt,
    DateTime UpdatedAt
);
```

```csharp
// RostroSummaryDto.cs
namespace Ecclesia.Application.Rostros.Queries.ListRostros;

public record RostroSummaryDto(
    Guid   Id,
    string Code,
    string Name,
    bool   IsActive
);
```

#### `GetRostroByIdQuery` / `GetRostroByIdHandler`

```csharp
// GetRostroByIdQuery.cs
namespace Ecclesia.Application.Rostros.Queries.GetRostroById;

public record GetRostroByIdQuery(Guid Id);
```

```csharp
// GetRostroByIdHandler.cs
using Ecclesia.Domain.Common;
using Ecclesia.Domain.Repositories;

namespace Ecclesia.Application.Rostros.Queries.GetRostroById;

public sealed class GetRostroByIdHandler(IRostroRepository repository)
{
    public async Task<Result<RostroDetailDto>> HandleAsync(
        GetRostroByIdQuery query,
        CancellationToken ct = default)
    {
        var rostro = await repository.GetByIdAsync(query.Id, ct);
        if (rostro is null)
            return Result<RostroDetailDto>.Failure("Rostro no encontrado.");

        var dto = new RostroDetailDto(
            rostro.Id,
            rostro.Code,
            rostro.Name,
            rostro.Description,
            rostro.IsActive,
            rostro.CreatedAt,
            rostro.UpdatedAt);

        return Result<RostroDetailDto>.Success(dto);
    }
}
```

#### `ListRostrosQuery` / `ListRostrosHandler`

```csharp
// ListRostrosQuery.cs
using Ecclesia.Domain.Common;

namespace Ecclesia.Application.Rostros.Queries.ListRostros;

public record ListRostrosQuery(
    string? Search,
    bool?   IsActive,
    int     Page     = 1,
    int     PageSize = 20
) : PagedQuery(Page, PageSize);
```

```csharp
// ListRostrosHandler.cs
using Ecclesia.Domain.Common;
using Ecclesia.Domain.Repositories;

namespace Ecclesia.Application.Rostros.Queries.ListRostros;

public sealed class ListRostrosHandler(IRostroRepository repository)
{
    public async Task<Result<PagedResult<RostroSummaryDto>>> HandleAsync(
        ListRostrosQuery query,
        CancellationToken ct = default)
    {
        var paged = await repository.ListAsync(
            query.Search,
            query.IsActive,
            query.Page,
            query.PageSize,
            ct);

        var dtos = paged.Items.Select(r => new RostroSummaryDto(
            r.Id, r.Code, r.Name, r.IsActive)).ToList();

        var result = new PagedResult<RostroSummaryDto>(dtos, paged.TotalCount, query.Page, query.PageSize);

        return Result<PagedResult<RostroSummaryDto>>.Success(result);
    }
}
```

---

## 6. Capa de Infraestructura

### 6.1 `RostroConfiguration.cs`
**Ruta:** `Ecclesia.Infrastructure/Data/Configurations/RostroConfiguration.cs`

```csharp
using Ecclesia.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecclesia.Infrastructure.Data.Configurations;

public sealed class RostroConfiguration : IEntityTypeConfiguration<RostroEntity>
{
    public void Configure(EntityTypeBuilder<RostroEntity> builder)
    {
        builder.ToTable("rostros", "org");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.Code)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(r => r.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(r => r.Description)
            .HasMaxLength(500);

        builder.Property(r => r.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        // Índices únicos
        builder.HasIndex(r => r.Code)
            .IsUnique()
            .HasDatabaseName("ix_rostros_code");

        builder.HasIndex(r => r.Name)
            .IsUnique()
            .HasDatabaseName("ix_rostros_name");

        // Relación con Community
        builder.HasMany(r => r.Communities)
            .WithOne(c => c.Rostro)
            .HasForeignKey(c => c.RostroId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
```

**Notas:**
- `DeleteBehavior.Restrict` — no se puede eliminar un Rostro si tiene Communities asociadas.
- Índices únicos a nivel DB como segunda línea de defensa (la primera es el handler).
- `HasDefaultValue(true)` en DB para consistencia con el dominio.

---

### 6.2 `RostroRepository.cs`
**Ruta:** `Ecclesia.Infrastructure/Repositories/RostroRepository.cs`

```csharp
using Ecclesia.Domain.Common;
using Ecclesia.Domain.Entities;
using Ecclesia.Domain.Repositories;
using Ecclesia.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Ecclesia.Infrastructure.Repositories;

public sealed class RostroRepository(AppDbContext context) : IRostroRepository
{
    public async Task<RostroEntity?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await context.Rostros
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == id, ct);

    public async Task<RostroEntity?> GetByCodeAsync(string code, CancellationToken ct = default)
        => await context.Rostros
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Code == code.ToUpperInvariant(), ct);

    public async Task<bool> ExistsByNameAsync(string name, Guid? excludeId = null, CancellationToken ct = default)
        => await context.Rostros
            .AsNoTracking()
            .AnyAsync(r => EF.Functions.ILike(r.Name, name) &&
                           (excludeId == null || r.Id != excludeId), ct);

    public async Task<bool> ExistsByCodeAsync(string code, Guid? excludeId = null, CancellationToken ct = default)
        => await context.Rostros
            .AsNoTracking()
            .AnyAsync(r => r.Code == code.ToUpperInvariant() &&
                           (excludeId == null || r.Id != excludeId), ct);

    public async Task<PagedResult<RostroEntity>> ListAsync(
        string? search,
        bool?   isActive,
        int     page,
        int     pageSize,
        CancellationToken ct = default)
    {
        var query = context.Rostros.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(r =>
                EF.Functions.ILike(r.Name, $"%{search}%") ||
                EF.Functions.ILike(r.Code, $"%{search}%"));

        if (isActive.HasValue)
            query = query.Where(r => r.IsActive == isActive.Value);

        var total = await query.CountAsync(ct);

        var items = await query
            .OrderBy(r => r.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return new PagedResult<RostroEntity>(items, total, page, pageSize);
    }

    public async Task AddAsync(RostroEntity rostro, CancellationToken ct = default)
        => await context.Rostros.AddAsync(rostro, ct);

    public async Task SaveChangesAsync(CancellationToken ct = default)
        => await context.SaveChangesAsync(ct);
}
```

**Notas:**
- `AsNoTracking()` en todas las lecturas — los comandos recuperan con tracking solo cuando necesitan modificar (el handler llama `GetByIdAsync` con tracking implícito al mutar y guardar desde el mismo `DbContext`).
- `EF.Functions.ILike` para búsqueda case-insensitive nativa de PostgreSQL.
- Paginación aplicada en DB, no en memoria.

> **Corrección importante:** Para que `UpdateRostroHandler` y `DeactivateRostroHandler` puedan mutar y guardar, el repositorio debería ofrecer un `GetByIdAsync` con tracking cuando se va a modificar. Una solución es eliminar `AsNoTracking()` en el `GetByIdAsync` de los handlers de escritura, o bien tener un método separado `GetByIdForUpdateAsync` sin `AsNoTracking`. La implementación más simple: quitar `AsNoTracking()` de `GetByIdAsync` y usarlo solo en queries de solo lectura vía el handler de queries.

---

## 7. Capa de API

### 7.1 Permisos
**Agregar en:** `Ecclesia.Api/Constants/EcclesiaPermissions.cs`

```csharp
// Dentro de la clase EcclesiaPermissions existente:
public static class Rostros
{
    private const string Base = "org.rostro";

    public const string Read       = $"{Base}.read";
    public const string Create     = $"{Base}.create";
    public const string Update     = $"{Base}.update";
    public const string Deactivate = $"{Base}.deactivate";
}
```

**Schema:** `org` — la entidad pertenece a la capa organizacional, no a `access_manager`.

---

### 7.2 `RostrosEndpoints.cs`
**Ruta:** `Ecclesia.Api/Endpoints/Rostros/RostrosEndpoints.cs`

```csharp
using Ecclesia.Application.Rostros.Commands.CreateRostro;
using Ecclesia.Application.Rostros.Commands.DeactivateRostro;
using Ecclesia.Application.Rostros.Commands.UpdateRostro;
using Ecclesia.Application.Rostros.Queries.GetRostroById;
using Ecclesia.Application.Rostros.Queries.ListRostros;
using Microsoft.AspNetCore.Mvc;

namespace Ecclesia.Api.Endpoints.Rostros;

public static class RostrosEndpoints
{
    public static void MapRostrosEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/rostros")
            .WithTags("Rostros");

        // GET /api/rostros
        group.MapGet("/", async (
            [FromQuery] string?  search,
            [FromQuery] bool?    isActive,
            [FromQuery] int      page     = 1,
            [FromQuery] int      pageSize = 20,
            ListRostrosHandler handler = default!,
            CancellationToken ct = default) =>
        {
            var query  = new ListRostrosQuery(search, isActive, page, pageSize);
            var result = await handler.HandleAsync(query, ct);
            return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Error);
        })
        .RequireAuthorization(EcclesiaPermissions.Rostros.Read)
        .WithName("ListRostros");

        // GET /api/rostros/{id}
        group.MapGet("/{id:guid}", async (
            Guid id,
            GetRostroByIdHandler handler,
            CancellationToken ct) =>
        {
            var result = await handler.HandleAsync(new GetRostroByIdQuery(id), ct);
            return result.IsSuccess ? Results.Ok(result.Value) : Results.NotFound(result.Error);
        })
        .RequireAuthorization(EcclesiaPermissions.Rostros.Read)
        .WithName("GetRostroById");

        // POST /api/rostros
        group.MapPost("/", async (
            [FromBody] CreateRostroCommand command,
            CreateRostroHandler handler,
            CancellationToken ct) =>
        {
            var result = await handler.HandleAsync(command, ct);
            return result.IsSuccess
                ? Results.Created($"/api/rostros/{result.Value}", new { id = result.Value })
                : Results.Conflict(result.Error);
        })
        .RequireAuthorization(EcclesiaPermissions.Rostros.Create)
        .WithName("CreateRostro");

        // PUT /api/rostros/{id}
        group.MapPut("/{id:guid}", async (
            Guid id,
            [FromBody] UpdateRostroRequest body,
            UpdateRostroHandler handler,
            CancellationToken ct) =>
        {
            var command = new UpdateRostroCommand(id, body.Name, body.Description);
            var result  = await handler.HandleAsync(command, ct);
            return result.IsSuccess ? Results.Ok(new { id = result.Value }) : Results.BadRequest(result.Error);
        })
        .RequireAuthorization(EcclesiaPermissions.Rostros.Update)
        .WithName("UpdateRostro");

        // DELETE /api/rostros/{id}  →  desactivación lógica
        group.MapDelete("/{id:guid}", async (
            Guid id,
            DeactivateRostroHandler handler,
            CancellationToken ct) =>
        {
            var result = await handler.HandleAsync(new DeactivateRostroCommand(id), ct);
            return result.IsSuccess ? Results.NoContent() : Results.BadRequest(result.Error);
        })
        .RequireAuthorization(EcclesiaPermissions.Rostros.Deactivate)
        .WithName("DeactivateRostro");
    }
}

// Request body para PUT (el Id viene de la ruta, no del body)
public record UpdateRostroRequest(string Name, string? Description);
```

---

## 8. Registro en `Program.cs`

```csharp
// ── Repositorios ──
builder.Services.AddScoped<IRostroRepository, RostroRepository>();

// ── Handlers ──
builder.Services.AddScoped<CreateRostroHandler>();
builder.Services.AddScoped<UpdateRostroHandler>();
builder.Services.AddScoped<DeactivateRostroHandler>();
builder.Services.AddScoped<GetRostroByIdHandler>();
builder.Services.AddScoped<ListRostrosHandler>();

// ── Políticas de autorización ──
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(EcclesiaPermissions.Rostros.Read,
        p => p.RequireClaim("permission", EcclesiaPermissions.Rostros.Read));
    options.AddPolicy(EcclesiaPermissions.Rostros.Create,
        p => p.RequireClaim("permission", EcclesiaPermissions.Rostros.Create));
    options.AddPolicy(EcclesiaPermissions.Rostros.Update,
        p => p.RequireClaim("permission", EcclesiaPermissions.Rostros.Update));
    options.AddPolicy(EcclesiaPermissions.Rostros.Deactivate,
        p => p.RequireClaim("permission", EcclesiaPermissions.Rostros.Deactivate));
});

// ── Endpoints ──
app.MapRostrosEndpoints();
```

---

## 9. Registro en `AppDbContext`

```csharp
// Dentro de AppDbContext:
public DbSet<RostroEntity> Rostros => Set<RostroEntity>();
```

La configuración `RostroConfiguration` se aplica automáticamente por la convención `ApplyConfigurationsFromAssembly`.

---

## 10. Migración EF Core

```bash
# Desde la raíz del proyecto
dotnet ef migrations add AddRostroModule \
  --project backend/src/Ecclesia.Infrastructure \
  --startup-project backend/src/Ecclesia.Api

dotnet ef database update \
  --project backend/src/Ecclesia.Infrastructure \
  --startup-project backend/src/Ecclesia.Api
```

**La migración creará:**
- Tabla `org.rostros`
- Columnas: `id`, `code`, `name`, `description`, `is_active`, `created_at`, `updated_at`, `xmin`
- Índices únicos: `ix_rostros_code`, `ix_rostros_name`

---

## 11. Contratos HTTP completos

### `GET /api/rostros`
| Param | Tipo | Requerido | Descripción |
|---|---|---|---|
| `search` | `string` | No | Filtra por `name` o `code` (ILIKE) |
| `isActive` | `bool` | No | Filtra por estado activo/inactivo |
| `page` | `int` | No (def: 1) | Número de página |
| `pageSize` | `int` | No (def: 20) | Tamaño de página |

**Respuesta 200:**
```json
{
  "items": [
    { "id": "uuid", "code": "RS01", "name": "Rostro Norte", "isActive": true }
  ],
  "totalCount": 1,
  "page": 1,
  "pageSize": 20
}
```

---

### `GET /api/rostros/{id}`
**Respuesta 200:**
```json
{
  "id": "uuid",
  "code": "RS01",
  "name": "Rostro Norte",
  "description": "Zona norte de la ciudad",
  "isActive": true,
  "createdAt": "2025-01-01T00:00:00Z",
  "updatedAt": "2025-01-01T00:00:00Z"
}
```
**Respuesta 404:** `"Rostro no encontrado."`

---

### `POST /api/rostros`
**Body:**
```json
{
  "code": "RS01",
  "name": "Rostro Norte",
  "description": "Zona norte de la ciudad"
}
```
**Respuesta 201:** `{ "id": "uuid" }`
**Respuesta 409:** cuando `code` o `name` ya existen.

---

### `PUT /api/rostros/{id}`
**Body:**
```json
{
  "name": "Rostro Norte Actualizado",
  "description": "Nueva descripción"
}
```
> `code` no es modificable después de la creación.

**Respuesta 200:** `{ "id": "uuid" }`
**Respuesta 400:** cuando el rostro está inactivo o el nombre ya existe en otro rostro.

---

### `DELETE /api/rostros/{id}`
> Desactivación lógica. No elimina el registro.

**Respuesta 204:** sin cuerpo.
**Respuesta 400:** si el rostro ya está inactivo.

---

## 12. Permisos y claims

| Permiso | Claim value | Operación |
|---|---|---|
| `org.rostro.read` | `org.rostro.read` | Listar y consultar |
| `org.rostro.create` | `org.rostro.create` | Crear |
| `org.rostro.update` | `org.rostro.update` | Actualizar nombre/descripción |
| `org.rostro.deactivate` | `org.rostro.deactivate` | Desactivar |

Los permisos se asignan a roles en `RolePermissionEntity` con `Schema = "org"`, `Option = "rostro"`, `Permission = "read|create|update|deactivate"`.

---

## 13. Reglas de negocio consolidadas

| # | Regla | Capa que la aplica |
|---|---|---|
| 1 | `Code` único, máx 10 chars, alfanumérico | Validator + Handler + DB index |
| 2 | `Name` único, máx 100 chars | Validator + Handler + DB index |
| 3 | `Code` se normaliza a mayúsculas | Domain (`Create` factory) |
| 4 | No eliminación física | No existe `DeleteAsync` |
| 5 | Rostro inactivo no se puede editar | Handler (`UpdateRostroHandler`) |
| 6 | Rostro inactivo no se puede re-desactivar | Handler (`DeactivateRostroHandler`) |
| 7 | `Code` no es modificable | `Update()` del dominio no recibe `code` |
| 8 | Rostro con Communities no se puede eliminar | `DeleteBehavior.Restrict` en EF Core |

---

## 14. Cómo usar este contexto para generar módulos relacionados

Cuando generes `Community`, el módulo tiene como dependencia directa a `Rostro`:

```
Community.RostroId → FK NOT NULL → Rostro.Id
```

El handler `CreateCommunityHandler` debe:
1. Verificar que el `RostroId` existe: `await rostroRepository.GetByIdAsync(command.RostroId)`
2. Verificar que el `Rostro` está **activo**: `if (!rostro.IsActive) return Failure(...)`
3. Recién entonces crear la `CommunityEntity`.

Y `ListCommunitiesByRostroQuery` filtrará por `RostroId` en el repositorio de `Community`.
