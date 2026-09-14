namespace Treasury.Contracts.IntegrationEvents;

using OrgSys.SharedKernel;

/// <summary>
/// Raised once a supplier payment (PostFinancialTransactionCommand, ReferenceType.Supplier,
/// Direction.Out) has been posted to the General Ledger — the sole canonical supplier-payment
/// posting path, mirrors CustomerPaymentPostedIntegrationEvent on the AR side (Direction.Out
/// instead of In — money leaving the company to pay the supplier). This carries only the total
/// amount paid against the supplier's AP account; it does not know which invoice(s) it should
/// settle — Payables applies it FIFO across the supplier's own open Payables (the same policy
/// Payables' existing GL-derived aging query already uses). This is a side-effect notification of
/// that existing posting, not a second posting decision.
/// </summary>
public sealed record SupplierPaymentPostedIntegrationEvent(
    long FinancialId,
    long SupplierId,
    decimal Amount,
    long CurrencyId,
    decimal Rate,
    DateTime PaymentDate,
    long CreateUserId,
    DateTime CreateDate,
    long? BranchId) : IntegrationEvent;
