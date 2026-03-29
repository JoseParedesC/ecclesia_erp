# 🧠 CONTEXTO MAESTRO PARA IA (React + Vite + ERP Iglesia)

## 🎯 Contexto General

ERP para iglesias con módulos contables y administrativos.

### Stack:
- React + Vite
- TypeScript
- TailwindCSS
- React Query
- React Hook Form

### API:
- REST
- Base URL: http://localhost
- Autenticación: JWT (Bearer Token)
- Respuestas: variables

---

## 🧱 Arquitectura Frontend

### Estructura:

```
src/
 ├── features/
 │    ├── users/
 │    ├── thirdParty/
 │    ├── account/
 │    ├── accountingPeriod/
 │    ├── community/
 │    ├── cashAccount/
 │    ├── memberInfo/
 │    ├── journalVoucher/
 │    ├── income/
 │    ├── expense/
 │
 ├── shared/
 │    ├── components/
 │    ├── hooks/
 │    ├── services/
 │    ├── types/
 │    ├── utils/
 │
 ├── app/
 │    ├── router/
 │    ├── providers/
```

---

## ⚙️ Consumo de API

- Usar React Query
- No usar fetch directo en componentes
- Centralizar llamadas en services

---

## 🧩 Reglas de Arquitectura

1. No lógica de negocio en componentes  
2. Usar hooks personalizados  
3. Separar DTOs/types  
4. Manejo centralizado de API  

---

## 🔄 Manejo de Datos

- Tipado por módulo
- Adaptación por endpoint
- Manejo centralizado de errores

---

## 🧪 Formularios

- React Hook Form
- Validaciones en hooks
- Componentes simples

---

## 🎨 UI

- TailwindCSS
- Componentes reutilizables en shared/components

---

## 🔐 Seguridad

- JWT
- Token en headers automáticamente

---

## 📦 Convenciones

- TypeScript obligatorio
- PascalCase
- Hooks: useFeature
- Servicios: feature.service.ts
- Tipos: feature.types.ts

---

## ⚡ Reglas para IA

- Respuestas cortas
- Código limpio
- Sin explicaciones innecesarias
- Cumplir todas las reglas

---

## 🚀 Ejemplo de Prompt

```
Genera módulo Users:

- service GET /users
- hook useUsers
- types
- componente UsersList

Reglas:
- sin lógica en componentes
- tipado estricto
- separación por capas
```

---

## 🧠 Recomendaciones

### Estandarizar API:

```
{
  "data": {},
  "message": "",
  "errors": []
}
```

### API Client:

```ts
export const api = axios.create({
  baseURL: 'http://localhost',
});
```

---

## 📌 Módulos

- Users  
- ThirdParty  
- Account  
- AccountingPeriod  
- Community  
- CashAccount  
- MemberInfo  
- JournalVoucher  
- Income  
- Expense  

---

## 🎯 Flujos críticos

- Crear factura  
- Cerrar periodo  
- Abrir periodo  
