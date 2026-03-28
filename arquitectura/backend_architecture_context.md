# Ecclesia ERP - Backend Arquitectura y Contexto

## 1. Introducción
Este documento recoge el estado actual del backend de Ecclesia ERP y ofrece un punto de partida para continuar desarrollos con IA. Incluye organización, patrones, responsabilidades y recomendaciones para iterar.


## 2. Visión general de la solución
- Proyecto principal: `backend/src`.
- Solución .NET: `Ecclesia.slnx`.
- Arquitectura en capas (limpia / hexagonal) con 4 proyectos:
  - `Ecclesia.Domain` (modelo de dominio, entidades, contratos, DTOs). 
  - `Ecclesia.Application` (casos de uso, validaciones, handlers/commands/queries).
  - `Ecclesia.Infrastructure` (EF Core, repositorios, auth, servicios, integración DB).
  - `Ecclesia.Api` (API HTTP minimal, endpoints MapGroup, middleware, auth/permiso/Swashbuckle).


## 3. Configuración principal del API (Program.cs)
Ruta: `backend/src/Ecclesia.Api/Program.cs`.

Aspectos detectados:
- `AddOpenApi()` + `MapOpenApi()` para docs.
- `AddDbContext<AppDbContext>(options => options.UseNpgsql(...).UseSnakeCaseNamingConvention())`.
- JWT: `JwtBearer` validando issuer, audience, signature, lifetime.
- Políticas de autorización definidas con claims "permission" según `EcclesiaPermissions`.
- FluentValidation: se registra `AddValidatorsFromAssembly` para distintos validators.
- Repositorios e handlers registrados con `AddScoped`.
- Middleware global de error: `ErrorHandlingMiddleware`.
- Endpoints: `MapUsersEndpoints`, `MapRolesEndpoints`, `MapAuthEndpoints`.


## 4. Dominio
### 4.1 Entidades base y comunes
- `BaseEntity` (ID, auditoría y (x)min para concurrencia) en `Ecclesia.Domain/Common`.
- Paginación: `PagedQuery`, `PagedResult`.
- `Result<T>` para respuestas con éxito/fallo.

### 4.2 Entidades principales
- `UserEntity` (`access_manager.Users`) con `Name`, `Email`, `UserName`, `PasswordHash`, relación a `UserRoleEntity`.
- `RoleEntity` con permisos y relaciones.
- `UserRoleEntity` con FK `UserId`, `RoleId`.
- `RolePermissionEntity` con `Schema`, `Option`, `Permission` y FK `RoleId`.

### 4.3 Constantes de permisión
- `EcclesiaPermissions` (sistema): `access_manager.user.*`, `access_manager.roles.*`.
- `RolePermissionConstants` (schema/option/permission literal). Esto permite composición dinámica `schema.option.permission`.


## 5. Infraestructura
### 5.1 DbContext
- `AppDbContext` registra todas las DbSets y aplica configuraciones automáticas del assembly.
- Para `BaseEntity`:
  - `Id` con `gen_random_uuid()` Postgres.
  - `CreatedAt`, `UpdatedAt` con `now()`.
  - `xmin` concurrency token.
- `SaveChangesAsync` controla `CreatedAt/UpdatedAt` para entidades `Added/Modified`.

### 5.2 Repositorios
- `IUserRepository`, `IRoleRepository`, `IAuthRepository`, `IUserRoleRepository` en `Ecclesia.Domain.Repositories`.
- Implementaciones `UserRepository`, `RoleRepository`, `AuthRepository`, `UserRoleRepository` en `Ecclesia.Infrastructure.Repositories`.
- `UserRepository.ListAllAsync` tiene filtrado/vista y paginación con ILIKE y orden en DB.
- `AuthRepository.GetUserPermissionsAsync` proyecta `schema.option.permission` para claims del JWT.

### 5.3 Auth y token
- `TokenService` genera JWT usando `Jwt:Key`, `Jwt:Issuer`, `Jwt:Audience`, claims normales (`sub`, `email`, `name`, `username`) y múltiples permisos.
- `CurrentUserService` lee `ClaimsPrincipal` del contexto (`sub`, `email`, `username`, `permission`).


## 6. Capa de aplicación (use cases)
- Patrón handler: `XyzHandler` con repositorios y `Validator`.
- Validaciones FluentValidation.
- Querys y Commands bajo directorios:
  - `Users/Querys`: `GetAll`, `GetById`.
  - `Users/Commands`: `CreateUser`, `UpdateUser`, `DeleteUser`.
  - `Roles/Commands`: `CreateRole`, `AssignRoleToUser`.
  - `Auth/Commands.Login`, `Auth/Queries.Me`.

- `Result<T>` encapsula éxito/fallo, con validaciones `Failure(...)`.


## 7. API Endpoints
- Endpoints definidas como extensiones MapGroup:
  - `UsersEndpoints`, `RolesEndpoints`, `AuthEndpoints`.
- Cada ruta exige autorización vía `.RequireAuthorization(EcclesiaPermissions.X)`.
- Mapea métodos HTTP to handlers con IResult.


## 8. Seguridad y permisos
- Uso claims-based auth en JWT.
- Policies conformes a permisos concretos (READ/CREATE/UPDATE/DELETE/ASSIGN).
- Se generan permisos desde roles en DB.


## 9. Convenciones y buenas prácticas actuales
- Las entidades usan `Table(..., Schema=...)` para particionar por funcionalidad.
- Se usa fecha UTC en auditoría.
- Repositorios administran transacciones y Save.
- Se usa `AsNoTracking()` en consultas de solo lectura.
- Validaciones preventivas con FluentValidation en Application.


## 10. Contexto del modelo contable (módulos existentes)
- Hay entidades de contabilidad en Domain/Entities: `JournalVoucher`, `JournalVoucherLine`, `Account`, `AccountingPeriod`, `Income`, `Expense`, `SequenceControl`, `Community`.
- Repositorios en Infrastructure para cada uno: `JournalVoucherRepository`, `IncomeRepository`, etc.
- Proyección de un ERP transaccional con gestión de asientos y períodos.

### 10.1 Entidades clave de contabilidad (MER.dbml)
- `Account`: catálogo de cuentas con `id`, `code`, `name`, `type` (Asset, Liability, Equity, Income, Expense), y jerarquía `parent_account_id`.
- `AccountingPeriod`: periodo contable con `year`, `month`, `status` (OPEN/CLOSED), fechas de cierre y trazabilidad.
- `JournalVoucher`: asiento contable con `voucher_number`, `type` (INCOME/EXPENSE/ADJUSTMENT/REVERSAL), `status` (DRAFT/POSTED/CANCELLED), `date`, `description`, y FK a `accounting_period`, `rostro`, `community`.
- `JournalVoucherLine`: detalle de asiento (sub-asiento) con `journal_voucher_id`, `account_id`, `amount`, `line_type` (DEBIT/CREDIT).
- `CashAccount`: cuentas de caja/banco, (`name`, `type` (CASH/BANK), `is_active`).
- `Donor`: persona/empresa donante con datos de documento y flag `is_company`.
- `Income`: ingreso monetario con `date`, `amount`, FK a `donor`, `cash_account`, `community` y `journal_voucher` (Registro único por asiento).
- `Expense`: gasto con `date`, `amount`, FK a `cash_account`, `community` y `journal_voucher`.
- `Budget`: presupuesto por `year`, `month`, `community`, `account` y `amount`.
- `ApprovalRequest`: workflow de aprobación para `Income`/`Expense` con `status` (PENDING/APPROVED/REJECTED).
- `DonationCertificate`: documento asociado a donadores con `certificate_number`, `donor`, `journal_voucher`, `amount`, `issue_date`, `pdf_url`.
- `AdjustmentLog`: auditoría de ajustes de asientos con `journal_voucher_id` y `reason`.

### 10.2 Objetivo funcional de la entidad contable
- El flujo principal es el modelo de doble entrada (JournalVoucher + JournalVoucherLine) donde cada `JournalVoucher` agrupa uno o más `JournalVoucherLine` con débitos/créditos equilibrados.
- `Account` define el plan de cuentas y se utiliza para clasificar líneas contables.
- `AccountingPeriod` asegura el cierre y control de periodos antes de cerrar transacciones.
- `Income` y `Expense` son eventos financieros de negocio que generan un `JournalVoucher` auditado y ligado al área de caja (`CashAccount`) y comunidad.
- `Budget` soporta control de ejecución presupuestal por comunidad y cuenta.
- `ApprovalRequest` garantiza control de autorizaciones antes de que ingresos/gastos se procesen.
- `DonationCertificate` respalda la emisión de comprobantes fiscales para donaciones.

## 11. Entorno y despliegue
- DB: PostgreSQL (veo `database/docker-compose.yml` y scripts en `database/database/init.sql`).
- ConnectionString en `appsettings.json` / `appsettings.Development.json`.
- Se requiere crear migraciones en project `Ecclesia.Infrastructure` / EF Core.


## 12. Cómo continuar con IA
### 12.1 Atención rápida (incremental)
1. Elegir dominio o endpoint (e.g., módulo de `JournalVoucher`).
2. Definir DTO / contracto de entrada/salida.
3. Agregar query/command + handler + validaciones.
4. Conectar endpoint en `Ecclesia.Api.Endpoints` y registrar servicios.
5. Actualizar tests unitarios y/o e2e (no hay tests aquí, agregados necesarios).

### 12.2 Generación con copilotos IA
- Usar plantillas de instrucciones:
  - "Crea un nuevo QueryHandler para listar ingresos con paginación y filtro." 
  - "Escribe la entidad, configuración de EF Core y repositorio para `Asset`." 
- Reforzar con ejemplos de todos los archivos del módulo actual para mantener coherencia.

### 12.3 Sugerencias de tareas útiles
- Evolucionar `RolePermissionEntity` a permisos dinámicos por UI (CRUD completo).
- Añadir `User` soft-delete con `IsDeleted` + filtros globales.
- Módulo de auditoría de cambios (event sourcing, logs).
- Test unitarios para handlers y validaciones.


## 13. Referencias de archivos clave
- `backend/src/Ecclesia.Api/Program.cs`
- `backend/src/Ecclesia.Api/Endpoints/Users/*`, `Roles/*`, `Auth/*`
- `backend/src/Ecclesia.Application/Users/Querys`, `Roles/Commands`, `Auth/Commands`
- `backend/src/Ecclesia.Domain/Entities/*`, `Common/*`, `Repositories/*.cs`
- `backend/src/Ecclesia.Infrastructure/Data/AppDbContext.cs`
- `backend/src/Ecclesia.Infrastructure/Repositories/UsersRepository.cs`, `AuthRepository.cs`
- `backend/src/Ecclesia.Infrastructure/Auth/TokenService.cs`, `CurrentUserService.cs`


---

> Nota: Puedes usar este markdown como base para el `README` de `backend` y alimentar a un agente IA con los archivos y estados para generar PRs.
