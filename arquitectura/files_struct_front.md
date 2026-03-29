
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