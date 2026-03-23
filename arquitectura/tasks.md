# API Endpoints – MVP Task List & Recommendations

## 🎯 Objective

Define the **minimum viable set of endpoints** required to build a functional church accounting system, aligned with:

- Clean Architecture
- CQRS
- Financial traceability
- Low complexity (MVP)

---

# 🧠 Design Principles

- Do NOT design endpoints per database table
- Design endpoints based on **use cases**
- Accounting should be **implicit**, not directly exposed
- All financial operations must be **traceable**

---

# 🚀 Required Endpoints (MVP)

## 🔐 1. Authentication


POST /auth/login
GET /auth/me


**Purpose:**
- User authentication
- Role-based access control (RBAC)

---

## 💰 2. Incomes (Core)


POST /incomes
GET /incomes
GET /incomes/{id}


**Purpose:**
- Register donations/income
- List incomes
- View income details

---

## 💸 3. Expenses


POST /expenses
GET /expenses
GET /expenses/{id}

**Purpose:**
- Register donations/income
- List incomes
- View income details

---

## 💸 3. Expenses



**Purpose:**
- Register expenses
- Track outgoing money

---

## ✅ 4. Approval Workflow


POST /approval-requests
POST /approval-requests/{id}/approve
POST /approval-requests/{id}/reject
GET /approval-requests



**Purpose:**
- Handle approval flow:
  - Secretary → Request
  - Manager → Approves/Rejects

---

## 📊 5. Dashboard

**Response should include:**
- Current month income
- Current month expenses
- Balance
- Comparison with previous month

---

## 🧾 6. Accounting (Read-Only)

GET /accounts
GET /journal-vouchers/{id}
GET /reports/trial-balance

**Purpose:**
- View chart of accounts
- Inspect generated vouchers
- Basic financial reporting

---

## 🧑‍🤝‍🧑 7. Donors


POST /donors
GET /donors



**Purpose:**
- Manage donors
- Associate donors with incomes

---

## 🏦 8. Cash Accounts




**Purpose:**
- List available cash/bank accounts
- Used during income/expense creation

---

## 🏷️ 9. Communities (Cost Centers)



**Purpose:**
- Generate donation certificates
- Provide downloadable documents

---

# ❌ Endpoints to AVOID

Do NOT expose accounting internals directly:
POST /journal-vouchers
PUT /journal-vouchers
DELETE /journal-vouchers
