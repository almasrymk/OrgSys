namespace Treasury.Contracts.IntegrationEvents;

using OrgSys.SharedKernel;

/// <summary>
/// Raised once a customer receipt (PostFinancialTransactionCommand, ReferenceType.Customer,
/// Direction.In) has been posted to the General Ledger — the sole canonical customer-receipt
/// posting path (see docs/architecture/receivables-ddd-migration.md §9/§12 Phase 6; the older
/// Financials.Commands.CreateFinancialCommand/CreateFinancialPaidInvoiceCommand per-invoice
/// allocation paths were found NOT to be reachable from the live Angular UI or to post to the GL at
/// all — see the migration doc for the evidence). This carries only the total amount received
/// against the customer's AR account; it does not know which invoice(s) it should settle —
/// Receivables applies it FIFO across the customer's own open Receivables (the same policy
/// Receivables' existing GL-derived aging query already uses). This is a side-effect notification
/// of that existing posting, not a second posting decision.
/// </summary>
public sealed record CustomerPaymentPostedIntegrationEvent(
    long FinancialId,
    long CustomerId,
    decimal Amount,
    long CurrencyId,
    decimal Rate,
    DateTime PaymentDate,
    long CreateUserId,
    DateTime CreateDate,
    long? BranchId) : IntegrationEvent;
