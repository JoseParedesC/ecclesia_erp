# ERP Iglesia Frontend

Frontend para un sistema ERP orientado a la gestión contable y administrativa de iglesias.

---

## 🚀 Stack Tecnológico

- **React + Vite**
- **TypeScript**
- **TailwindCSS**
- **React Query**
- **React Hook Form**

---

## 🧠 Arquitectura

El proyecto sigue una arquitectura **Feature-Based**, promoviendo escalabilidad, mantenibilidad y separación de responsabilidades.

### 📁 Estructura del Proyecto

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

## ⚙️ Integración con API

- Arquitectura basada en **API REST**
- Base URL configurable (`http://localhost`)
- Autenticación mediante **JWT (Bearer Token)**

### Reglas de Consumo

- Todas las llamadas HTTP deben implementarse en `services`
- Uso obligatorio de **React Query**
- Prohibido consumir API directamente en componentes

---

## 🧩 Principios de Arquitectura

- Separación estricta de responsabilidades
- Componentes desacoplados de lógica de negocio
- Uso de hooks personalizados para lógica
- Tipado fuerte con TypeScript

### Reglas Obligatorias

1. No lógica de negocio en componentes  
2. Uso de hooks para encapsular lógica  
3. Separación de DTOs/types  
4. Manejo centralizado de API  

---

## 🔄 Manejo de Datos

Debido a la variabilidad en las respuestas de la API:

- Se definen tipos por módulo
- Se adaptan respuestas en cada servicio
- Manejo centralizado de errores

---

## 🧪 Formularios

- Uso de **React Hook Form**
- Validaciones dentro de hooks
- Componentes presentacionales

---

## 🎨 UI

- Estilizado con **TailwindCSS**
- Componentes reutilizables en `shared/components`

---

## 🔐 Seguridad

- Autenticación basada en JWT
- Manejo de token en headers automáticamente
- Preparado para implementación de roles y permisos

---

## 📦 Convenciones

- TypeScript obligatorio
- PascalCase para componentes
- Hooks: `useFeature`
- Servicios: `feature.service.ts`
- Tipos: `feature.types.ts`

---

## 🧱 Módulos del Sistema

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

## 🎯 Flujos Críticos

- Creación de facturas
- Apertura de periodos contables
- Cierre de periodos contables

---

## 💡 Recomendaciones

### Estandarización de API

Se recomienda unificar las respuestas bajo el siguiente esquema:

```
{
  "data": {},
  "message": "",
  "errors": []
}
```

---

### Cliente HTTP Centralizado

```ts
import axios from 'axios';

export const api = axios.create({
  baseURL: 'http://localhost',
});
```

---

## 🤖 Uso con IA

Este proyecto está diseñado para ser extendido mediante generación asistida por IA.

### Ejemplo de Prompt

```
Genera el módulo Users con:

- service para GET /users
- hook useUsers con React Query
- types
- componente UsersList

Cumpliendo:
- sin lógica en componentes
- tipado estricto
- separación por capas
```

---

## 📌 Objetivo

Construir un frontend escalable, mantenible y consistente, alineado con buenas prácticas de arquitectura moderna.
