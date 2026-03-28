# Razonamiento por fases — MVP Ecclesia ERP

> **Principio rector:** El núcleo del negocio es simple — dinero entra, dinero sale, queda registrado con doble entrada. Todo lo demás es accesorio hasta que eso funcione.

---

## Fase 1 — Fundación

Son **prerequisitos puros**. Todo el modelo tiene FK hacia estas tres entidades, lo que significa que no puedes instanciar ninguna otra entidad sin ellas.

- **`ThirdParty`** es la entidad base de todos los actores del sistema: donantes, miembros, proveedores, empleados. Ningún ingreso ni gasto puede existir sin un tercero asociado.
- **`Account`** define el plan de cuentas. Sin él no hay `JournalVoucherLine`, y sin líneas no hay contabilidad de doble entrada. Es el catálogo que clasifica cada movimiento financiero.
- **`AccountingPeriod`** controla el ciclo contable. Un `JournalVoucher` con status `POSTED` requiere que el período esté `OPEN`. Sin este control no puedes garantizar la integridad del cierre contable ni prevenir registros retroactivos.

**Costo de no hacerlo primero:** bloqueo total. Ninguna otra fase puede avanzar.

**CQRS a generar:**
- `Account` → `CreateAccountCommand`, `GetAccountsQuery` (árbol jerárquico por `parent_account_id`)
- `AccountingPeriod` → `CreatePeriodCommand`, `ClosePeriodCommand`, `GetCurrentPeriodQuery`
- `ThirdParty` → `CreateThirdPartyCommand`, `GetThirdPartyByIdQuery`, `ListThirdPartiesQuery` (paginado)

---

## Fase 2 — Estructura organizacional

Resuelve las FK `NOT NULL` que `JournalVoucher`, `Income` y `Expense` exigen antes de poder existir.

- **`Rostro` y `Community`** son obligatorios en `JournalVoucher`. La organización opera por comunidades y cada asiento debe tener contexto organizacional. Sin ellos el motor contable no tiene a qué comunidad imputar el movimiento.
- **`CashAccount`** es FK `NOT NULL` en `Income` y `Expense`. Representa las cajas físicas y cuentas bancarias donde entra y sale el dinero real. Sin este catálogo no puedes registrar ningún movimiento financiero de negocio.
- **`MemberInfo`** es el rol más usado desde el primer día. La razón de ser de Ecclesia es gestionar miembros de comunidades. Aunque técnicamente no bloquea el motor contable, su ausencia hace el sistema inútil para el usuario final.

**Costo de no hacerlo antes de la Fase 3:** `JournalVoucher` tiene `rostro_id` y `community_id` como `NOT NULL` — no puedes crear ni un asiento de prueba sin estas tablas pobladas.

**CQRS a generar:**
- `Rostro` → `CreateRostroCommand`, `ListRostrosQuery`
- `Community` → `CreateCommunityCommand`, `ListCommunitiesByRostroQuery`
- `CashAccount` → `CreateCashAccountCommand`, `GetCashAccountsQuery`
- `MemberInfo` → `CreateMemberCommand`, `UpdateMemberStatusCommand`, `GetMemberByThirdPartyQuery`

---

## Fase 3 — Motor contable

Es el **corazón del MVP**. El par `JournalVoucher` + `JournalVoucherLine` es lo que diferencia Ecclesia ERP de una hoja de cálculo. Toda la integridad financiera reside aquí.

- **`JournalVoucher`** agrupa un asiento contable completo con su tipo (`INCOME`, `EXPENSE`, `ADJUSTMENT`, `REVERSAL`), su estado (`DRAFT` → `POSTED` → `CANCELLED`) y su contexto organizacional. El flujo de estados es crítico: solo los asientos `POSTED` afectan el balance.
- **`JournalVoucherLine`** implementa la partida doble. La regla de negocio fundamental es que la suma de todos los débitos debe ser igual a la suma de todos los créditos dentro del mismo voucher. Esta validación debe ocurrir en el handler, no en la base de datos.

**La lógica crítica de este módulo:**

```
∑ DEBIT lines == ∑ CREDIT lines  →  condición para poder hacer POST
AccountingPeriod.status == OPEN  →  condición para poder hacer POST
```

**CQRS a generar:**
- `CreateJournalVoucherCommand` — crea el voucher en estado `DRAFT` con sus líneas, valida balance débito/crédito
- `PostJournalVoucherCommand` — cambia estado a `POSTED`, verifica período `OPEN`
- `CancelJournalVoucherCommand` — solo si está en `DRAFT`; si está `POSTED` debe generar un `REVERSAL`
- `GetJournalVoucherByIdQuery` — incluye líneas y totales
- `ListJournalVouchersQuery` — paginado, filtrable por tipo, estado, período, comunidad

---

## Fase 4 — Capa financiera de negocio

Son la **interfaz de negocio** sobre el motor contable. El usuario no registra asientos directamente; registra ingresos y gastos, y el sistema genera el `JournalVoucher` automáticamente.

- **`Income`** representa el evento de negocio de entrada de dinero (donaciones, ofrendas). Su handler debe: crear el `Income`, generar el `JournalVoucher` correspondiente con sus líneas (débito a `CashAccount`, crédito a cuenta de ingreso), y vincular ambos con `journal_voucher_id`.
- **`Expense`** es el flujo simétrico: salida de dinero. Débito a cuenta de gasto, crédito a `CashAccount`.
- **`DonorInfo`** va aquí porque sin donantes no hay ingresos que registrar. Es el rol más frecuente de `ThirdParty` en el contexto de una organización religiosa.

**La lógica crítica de este módulo:**

```
RegisterIncome → CreateJournalVoucher(INCOME, DRAFT)
                  → AddLine(CashAccount.linkedAccount, DEBIT)
                  → AddLine(IncomeAccount, CREDIT)
                  → PostJournalVoucher (si ApprovalRequest no está activo)
```

**CQRS a generar:**
- `RegisterIncomeCommand` — crea `Income` + `JournalVoucher` en una transacción
- `RegisterExpenseCommand` — crea `Expense` + `JournalVoucher` en una transacción
- `GetIncomeByIdQuery`, `ListIncomesQuery` (filtros: comunidad, período, tercero)
- `GetExpenseByIdQuery`, `ListExpensesQuery`
- `CreateDonorInfoCommand`, `GetDonorByThirdPartyQuery`

---

## Fase 5 — Control y aprobación

Son los **controles de gobernanza**. Sin ellos el sistema funciona, pero sin auditoría ni supervisión. Para una organización religiosa con múltiples líderes, la aprobación previa a cualquier movimiento es una necesidad cultural, no solo técnica.

- **`ApprovalRequest`** intercepta `Income` y `Expense` antes de que se posteen. El flujo es: registro en `DRAFT` → solicitud de aprobación en `PENDING` → aprobado/rechazado → post o cancelación. Protege el flujo de caja desde el primer día con usuarios reales.
- **`Budget`** permite controlar la ejecución presupuestal por comunidad y cuenta contable. Habilita reportes de variación (presupuestado vs ejecutado) que los líderes necesitan para tomar decisiones. Es prioritario sobre certificados y auditoría porque impacta la operación diaria.

**CQRS a generar:**
- `CreateApprovalRequestCommand` — se dispara automáticamente al crear `Income`/`Expense`
- `ApproveRequestCommand`, `RejectRequestCommand`
- `GetPendingApprovalsQuery` — filtrado por aprobador
- `CreateBudgetCommand`, `UpdateBudgetCommand`
- `GetBudgetVsActualQuery` — comparativo presupuesto vs ejecutado por comunidad/período

---

## Post-MVP — Diferir para v2

Estas entidades son útiles pero no bloquean la operación principal. Se pueden agregar incrementalmente una vez que el flujo de registro, aprobación y cierre de período funcione end-to-end.

| Entidad | Razón para diferir |
|---|---|
| `DonationCertificate` | Requiere generación de PDF y numeración fiscal — complejidad de infraestructura alta |
| `AdjustmentLog` | Auditoría deseable pero no crítica para el MVP |
| `SupplierInfo` | Rol secundario de `ThirdParty`; los gastos funcionan sin detalle de proveedor |
| `PartnerInfo` | Sin casos de uso definidos en el flujo principal |
| `EmployeeInfo` | Nómina es un módulo separado; no impacta contabilidad básica |
| `CustomerInfo` | Segmento poco relevante para el contexto eclesial |

---

## Orden de implementación recomendado por sprint

```
Sprint 1  →  Fase 1 completa (ThirdParty, Account, AccountingPeriod)
Sprint 2  →  Fase 2 completa (Rostro, Community, CashAccount, MemberInfo)
Sprint 3  →  Fase 3 completa (JournalVoucher + JournalVoucherLine)
Sprint 4  →  Fase 4 completa (Income, Expense, DonorInfo)
Sprint 5  →  Fase 5 completa (ApprovalRequest, Budget)
Sprint 6+  →  Post-MVP según demanda
```

> Cada sprint sigue el patrón CQRS establecido: `Command/Query` → `Handler` → `Validator` (FluentValidation) → `Repository` → `Endpoint` (MapGroup) → registro en `Program.cs`.
