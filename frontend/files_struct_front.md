
----------------------------------------------------------------------------------------------------------------
src/
 ├── styles/
 │    └── theme.css              ← Variables CSS globales (colores, tipografía, radios)
 ├── features/auth/
 │    ├── auth.types.ts           ← DTOs: LoginRequest, LoginResponse, AuthUser
 │    ├── auth.service.ts         ← Llamada REST + manejo del JWT en localStorage
 │    ├── hooks/
 │    │    ├── useLogin.ts        ← useMutation de React Query
 │    │    └── useLoginForm.ts    ← useForm de React Hook Form
 │    ├── pages/
 │    │    ├── LoginPage.tsx      ← Componente principal
 │    │    └── LoginPage.module.css
 │    └── index.ts                ← Barrel export
 ├── app/providers/
 │    └── AppProviders.tsx        ← QueryClient configurado
 └── App.tsx                      ← Entrada wiring



----------------------------------------------------------------------------------------------------------------
 features/account/
 ├── account.types.ts          ← Account, AccountType, CreateAccountRequest, UpdateAccountRequest
 ├── account.service.ts        ← GET, POST, PUT, DELETE via apiClient
 ├── hooks/
 │    ├── useAccounts.ts       ← useQuery + 3 useMutation (create/update/delete)
 │    └── useAccountForm.ts    ← useForm con defaults para edición
 ├── utils/
 │    └── accountTree.ts       ← buildTree, flattenTree, filterAccounts
 ├── components/
 │    ├── AccountTypeBadge     ← Badge coloreado por tipo (5 variantes)
 │    ├── AccountTreeRow       ← Fila recursiva con toggle expand/collapse
 │    ├── AccountFormModal     ← Crear / editar con validación RHF
 │    └── DeleteConfirmModal   ← Confirmación de eliminación
 └── pages/
      └── AccountsPage.tsx     ← Página principal que orquesta todo



----------------------------------------------------------------------------------------------------------------

features/accountingPeriod/
 ├── accountingPeriod.types.ts      ← AccountingPeriod, PeriodStatus, CreatePeriodRequest
 ├── accountingPeriod.service.ts    ← getAll, getCurrent, create, close via apiClient
 ├── hooks/
 │    └── useAccountingPeriods.ts   ← useQuery x2 + useMutation x2 (create/close)
 ├── utils/
 │    └── periodUtils.ts            ← formatPeriod, nextPeriod, sortPeriods
 ├── components/
 │    ├── PeriodStatusBadge         ← dot animado verde (OPEN) / gris (CLOSED)
 │    ├── CreatePeriodModal         ← selects mes/año, pre-populated con nextPeriod()
 │    └── ClosePeriodModal          ← confirmación con warning irreversible
 └── pages/
      └── AccountingPeriodsPage.tsx ← Página principal con 4 summary cards + tabla

      
----------------------------------------------------------------------------------------------------------------
features/users/
 ├── user.types.ts            ← User, Role, PagedResult<T>, PagedQuery, todos los Request
 ├── user.service.ts          ← getAll (paginado), getById, create, update, remove, assignRole
 ├── role.service.ts          ← getAll de roles (para los selectores)
 ├── hooks/
 │    └── useUsers.ts         ← useUsers (paginado), useRoles, useCreateUser,
 │                               useUpdateUser, useDeleteUser, useAssignRole
 ├── components/
 │    ├── UserFormModal        ← Crear/editar: nombre, email, username, password, rol
 │    └── AssignRoleModal      ← Radio cards por rol, muestra el usuario y su rol actual
 └── pages/
      └── UsersPage.tsx        ← Tabla paginada con búsqueda debounced

shared/
 ├── hooks/useDebounce.ts      ← Reutilizable en cualquier módulo con búsqueda
 └── components/Pagination     ← Paginador con ventana inteligente (1…n…último)