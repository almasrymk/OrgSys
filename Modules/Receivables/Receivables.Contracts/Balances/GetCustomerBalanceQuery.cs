namespace Receivables.Contracts.Balances;

using OrgSys.SharedKernel;

/// <summary>
/// Public contract for a customer's outstanding receivable balance as of an optional date —
/// Sum(Debit - Credit) of valid Journal history posted to the customer's receivable account.
/// Handled by Receivables.Application.
/// </summary>
public record GetCustomerBalanceQuery(long DealerId, DateTime? AsOfDate) : IQuery<decimal>;
