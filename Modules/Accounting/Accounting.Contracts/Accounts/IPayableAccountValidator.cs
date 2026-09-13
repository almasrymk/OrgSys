namespace Accounting.Contracts.Accounts;

using OrgSys.SharedKernel;
using Parties.Contracts.Dealers;

/// <summary>
/// Validates that a Dealer is a valid, GL-linked AP supplier — reused by Dealer create/update,
/// Supplier Payment posting, and Supplier opening balance so the same rules apply everywhere a
/// supplier's payable account is touched. Public contract (moved out of Accounting.Application —
/// see the Accounting DDD cleanup report). Implemented in Accounting.Application.
/// </summary>
public interface IPayableAccountValidator
{
    /// <summary>Validates that <paramref name="dealerId"/> is an active Supplier dealer with a valid payable account.</summary>
    Task<(DealerLookupDto? Dealer, AccountLookupDto? Account, List<Error> Errors)> ValidateSupplierAsync(long dealerId, CancellationToken cancellationToken = default);
}
