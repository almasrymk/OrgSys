namespace Application.Common.Services
{
    using Domain.Entities;
    using Domain.Shared;

    /// <summary>
    /// Single source of truth for validating that a Chart-of-Accounts account is a valid
    /// posting/detail target, and that a Dealer is a valid, GL-linked AR customer — reused by
    /// Dealer create/update, Customer Receipt posting, and Customer opening balance so the same
    /// rules apply everywhere a customer's receivable account is touched.
    /// </summary>
    public interface IReceivableAccountValidator
    {
        /// <summary>Validates that <paramref name="accountId"/> is an active, postable (non-group) account.</summary>
        Task<(Account? Account, List<Error> Errors)> ValidateAccountAsync(long accountId, CancellationToken cancellationToken = default);

        /// <summary>Validates that <paramref name="dealerId"/> is an active Client dealer with a valid receivable account.</summary>
        Task<(Dealer? Dealer, Account? Account, List<Error> Errors)> ValidateCustomerAsync(long dealerId, CancellationToken cancellationToken = default);

        /// <summary>Validates that <paramref name="dealerId"/> is an active Supplier dealer with a valid payable account.</summary>
        Task<(Dealer? Dealer, Account? Account, List<Error> Errors)> ValidateSupplierAsync(long dealerId, CancellationToken cancellationToken = default);
    }
}
