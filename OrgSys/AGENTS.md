# AGENTS.md

## Project
OrgSys ERP

## General Instructions
- Follow the existing solution architecture, naming conventions, coding style, and folder structure.
- Before changing code, inspect the relevant existing entities, configurations, repositories, handlers, services, DTOs, mappings, migrations, and tests.
- Do not create duplicate abstractions if an equivalent one already exists.
- Preserve backward compatibility where reasonably possible.
- After implementation, build the affected projects and fix compilation errors caused by the change.
- Do not modify unrelated features.

---

# ERP-FIN-UNIFIED-TRANSACTIONS

## Goal
Unify cash-box and bank financial movements under one transaction model.

Do NOT maintain separate `CashTransactions` and `BankTransactions` transaction models unless legacy compatibility temporarily requires them.

The target design is:

- `FinancialAccounts` — common master entity for any money-holding financial account.
- `CashBoxes` — cash-box-specific details.
- `BankAccounts` — bank-account-specific details.
- `FinancialTransactions` — one unified transaction table for both cash boxes and banks.
- `FinancialTransactionTypes` — transaction type lookup/configuration.
- `FinancialTransfers` — transfer header connecting source and destination financial accounts.

## 1. FinancialAccounts

Common master entity representing either a cash box or a bank account.

Suggested fields:

```text
Id
Code
Name
Type              // CashBox | Bank
AccountId         // General Ledger account
CurrencyId
IsActive
```

Prefer an enum or strongly typed code for:

```text
CashBox
Bank
```

The GL `AccountId` links the financial account to the Chart of Accounts.

## 2. CashBoxes

Contains only cash-box-specific data.

Suggested fields:

```text
Id
FinancialAccountId
BranchId
KeeperEmployeeId
```

`FinancialAccountId` must reference `FinancialAccounts`.

Do not duplicate common fields such as Code, Name, CurrencyId, or GL AccountId here if they already belong to `FinancialAccounts`.

## 3. BankAccounts

Contains only bank-specific data.

Suggested fields:

```text
Id
FinancialAccountId
BankId
AccountNumber
IBAN
SwiftCode
BranchName
```

`FinancialAccountId` must reference `FinancialAccounts`.

Do not put bank-only fields on the common `FinancialAccounts` table.

## 4. FinancialTransactions

Use ONE unified transaction entity/table for both cash-box and bank movements.

Suggested fields:

```text
Id
FinancialAccountId
TransactionDate
TransactionTypeId
Direction
Amount
CurrencyId
ExchangeRate
ReferenceType
ReferenceId
ContraFinancialAccountId
Description
JournalEntryId
Status
```

### Direction

Use a clear direction concept:

```text
In
Out
```

The financial account balance effect is derived from `Direction`.

### Reference

`ReferenceType` + `ReferenceId` identify the business source of the movement.

Examples:

```text
Customer
Supplier
Employee
Expense
Income
Invoice
Payment
Loan
Cheque
PaymentGateway
Transfer
Other
```

Do not create a different transaction entity for every source type.

## 5. FinancialTransactionTypes

Transaction types should describe the financial nature of the movement, not every possible business scenario.

Recommended core types:

```text
Receipt
Payment
Transfer
Deposit
Withdrawal
Fee
Interest
Cheque
Adjustment
OpeningBalance
```

Avoid exploding the type table into values such as:

```text
CustomerReceipt
SupplierPayment
EmployeePayment
ExpensePayment
```

Those distinctions should normally be represented by `ReferenceType`.

## 6. FinancialTransfers

Transfers between any two financial accounts must use a transfer header.

Suggested fields:

```text
Id
FromFinancialAccountId
ToFinancialAccountId
Amount
CurrencyId
ExchangeRate
TransactionDate
Description
Status
```

Supported transfer combinations:

```text
CashBox -> CashBox
CashBox -> Bank
Bank -> CashBox
Bank -> Bank
```

Each posted transfer creates TWO `FinancialTransactions`:

1. `Out` transaction on `FromFinancialAccountId`.
2. `In` transaction on `ToFinancialAccountId`.

Both transactions must be traceable back to the same `FinancialTransfer`.

Do not treat a transfer as income or expense.

## 7. Journal Integration

When a financial transaction is posted, integrate it with the existing journal-entry mechanism instead of implementing a second accounting engine.

Examples:

### Customer receipt into cash/bank

```text
Dr Financial Account GL
Cr Customer / Accounts Receivable
```

### Supplier payment from cash/bank

```text
Dr Supplier / Accounts Payable
Cr Financial Account GL
```

### CashBox -> Bank transfer

```text
Dr Destination Bank GL
Cr Source CashBox GL
```

### Bank -> Bank transfer

```text
Dr Destination Bank GL
Cr Source Bank GL
```

A transfer must not create revenue or expense.

Store/reference the resulting `JournalEntryId` where compatible with the existing architecture.

## 8. Implementation Rules

When asked to implement `ERP-FIN-UNIFIED-TRANSACTIONS`:

1. Inspect the current OrgSys financial/accounting implementation first.
2. Locate existing cash-box, bank-account, cash transaction, bank transaction, journal, account, currency, branch, customer, supplier, and payment models.
3. Reuse existing base entities, auditing, tenant/company fields, status conventions, repositories, CQRS handlers, DTO conventions, validation, and mapping.
4. Design the migration path before deleting or replacing legacy tables.
5. Preserve existing data when feasible.
6. Add/update EF Core configurations and relationships.
7. Add/update migrations only after the model is coherent.
8. Update Application layer commands/queries/DTOs/validators/mappings.
9. Update API/Presentation endpoints as required.
10. Update journal integration.
11. Update balance queries/reports to use the unified model.
12. Build and run relevant tests.
13. Report:
   - files changed,
   - database changes,
   - migration name,
   - compatibility concerns,
   - tests/build results.

## 9. Important Constraint

Do not blindly create the suggested field names if equivalent concepts already exist under different names in OrgSys.

Adapt this design to the existing codebase while preserving the architectural goal:

> One common financial-account abstraction and one unified transaction model for CashBox and Bank movements, with specialized detail entities and a common transfer mechanism.

---

## Codex shorthand

When the user says:

```text
Implement ERP-FIN-UNIFIED-TRANSACTIONS
```

or:

```text
نفذ ERP-FIN-UNIFIED-TRANSACTIONS
```

treat it as a request to implement the complete specification in this section, adapted to the current OrgSys repository.
