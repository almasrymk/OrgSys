# Integration events

Events implement `IntegrationEvent` (`EventId` for inbox idempotency). Publishers write `OutboxMessage` in the same `OrgContext` transaction (`OutboxIntegrationEventPublisher`). `OutboxDispatcher` is a hosted service. Consumers claim `InboxMessage` by EventId before applying.

| Event | Contract | Publisher | Consumers |
|-------|----------|-----------|-----------|
| `JournalPostedIntegrationEvent` | Accounting.Contracts | Journal post | GL-side listeners |
| `JournalReversedIntegrationEvent` | Accounting.Contracts | Journal reverse | GL-side listeners |
| `JournalCancelledIntegrationEvent` | Accounting.Contracts | Journal cancel | GL-side listeners |
| `SalesInvoicePostedIntegrationEvent` | CommercialDocuments.Contracts | Posted sales invoice | Receivables open-item; Reporting aging/sales-summary projections |
| `PurchaseInvoicePostedIntegrationEvent` | CommercialDocuments.Contracts | Posted purchase invoice | Payables open-item |
| `CustomerPaymentPostedIntegrationEvent` | Treasury.Contracts | Customer receipt posted | Receivables FIFO application |
| `SupplierPaymentPostedIntegrationEvent` | Treasury.Contracts | Supplier payment posted | Payables FIFO application |
| `GoodsReceiptPostedIntegrationEvent` | Inventory.Contracts | Posted inventory receipt | Inventory inbox claim; Purchasing `RecordReceipt` when `PurchaseOrderId` + lines are present |

Transfers create two `Financial` rows (Out/In) traceable to the same `FinancialTransfer`; they are not income/expense events.

Do not raise a second posting event that re-posts the General Ledger.
