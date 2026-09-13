namespace Accounting.Application
{
    using Accounting.Application.Accounts.Queries;
    using Accounting.Contracts.Accounts;
    using MediatR;
    using Parties.Contracts.Dealers;

    public sealed class ReceivableAccountValidator(
        IRepository<Account> _AccountRepository,
        ISender _Sender) : IReceivableAccountValidator
    {
        public async Task<(AccountLookupDto? Account, List<Error> Errors)> ValidateAccountAsync(long accountId, CancellationToken cancellationToken = default)
        {
            var errors = new List<Error>();

            if (accountId <= 0)
            {
                errors.Add(new Error("A receivable account must be selected."));
                return (null, errors);
            }

            var account = await _AccountRepository.GetByFilterAsync(e => e.Id == accountId, "AccountType");
            if (account is null || account.Status == Status.Deleted || account.Hide)
            {
                errors.Add(new Error("The selected account does not exist or is not active."));
                return (null, errors);
            }

            if (!account.IsPostable)
                errors.Add(new Error($"Account '{account.Name}' is a parent/group account and cannot receive postings. Select a detail account."));

            return (GetAccountQueryHandler.ToDto(account), errors);
        }

        public async Task<(DealerLookupDto? Dealer, AccountLookupDto? Account, List<Error> Errors)> ValidateCustomerAsync(long dealerId, CancellationToken cancellationToken = default)
        {
            var errors = new List<Error>();

            var dealerResult = await _Sender.Send(new GetDealerByIdQuery(dealerId), cancellationToken);
            var dealer = dealerResult.Response;
            if (dealer is null || dealer.IsDeleted || dealer.Hide)
            {
                errors.Add(new Error("The selected customer does not exist or is not active."));
                return (null, null, errors);
            }

            if (dealer.TypeId != (long)DealerType.Client)
            {
                errors.Add(new Error($"'{dealer.Name}' is not a customer."));
                return (dealer, null, errors);
            }

            if (dealer.AccountId is not > 0)
            {
                errors.Add(new Error($"Customer '{dealer.Name}' does not have a linked receivable account."));
                return (dealer, null, errors);
            }

            var (account, accountErrors) = await ValidateAccountAsync(dealer.AccountId.Value, cancellationToken);
            errors.AddRange(accountErrors);
            return (dealer, account, errors);
        }

        public async Task<(DealerLookupDto? Dealer, AccountLookupDto? Account, List<Error> Errors)> ValidateSupplierAsync(long dealerId, CancellationToken cancellationToken = default)
        {
            var errors = new List<Error>();

            var dealerResult = await _Sender.Send(new GetDealerByIdQuery(dealerId), cancellationToken);
            var dealer = dealerResult.Response;
            if (dealer is null || dealer.IsDeleted || dealer.Hide)
            {
                errors.Add(new Error("The selected supplier does not exist or is not active."));
                return (null, null, errors);
            }

            if (dealer.TypeId != (long)DealerType.Supplier)
            {
                errors.Add(new Error($"'{dealer.Name}' is not a supplier."));
                return (dealer, null, errors);
            }

            if (dealer.AccountId is not > 0)
            {
                errors.Add(new Error($"Supplier '{dealer.Name}' does not have a linked payable account."));
                return (dealer, null, errors);
            }

            var (account, accountErrors) = await ValidateAccountAsync(dealer.AccountId.Value, cancellationToken);
            errors.AddRange(accountErrors);
            return (dealer, account, errors);
        }
    }
}
