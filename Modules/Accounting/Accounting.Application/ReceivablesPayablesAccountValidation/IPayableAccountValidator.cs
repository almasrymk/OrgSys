namespace Accounting.Application
{

    /// <summary>
    /// Validates that a Dealer is a valid, GL-linked AP supplier — reused by Dealer create/update,
    /// Supplier Payment posting, and Supplier opening balance so the same rules apply everywhere a
    /// supplier's payable account is touched. Account-level validity itself is delegated to
    /// <see cref="IReceivableAccountValidator.ValidateAccountAsync"/> (the existing single source of
    /// truth for "is this account an active, postable account") rather than duplicated here.
    /// </summary>
    public interface IPayableAccountValidator
    {
        /// <summary>Validates that <paramref name="dealerId"/> is an active Supplier dealer with a valid payable account.</summary>
        Task<(Dealer? Dealer, Account? Account, List<Error> Errors)> ValidateSupplierAsync(long dealerId, CancellationToken cancellationToken = default);
    }
}
