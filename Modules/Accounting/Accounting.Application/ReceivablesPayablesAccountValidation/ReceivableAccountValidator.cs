namespace Accounting.Application
{

    public sealed class ReceivableAccountValidator(
        IRepository<Account> _AccountRepository,
        IRepository<Dealer> _DealerRepository) : IReceivableAccountValidator
    {
        public async Task<(Account? Account, List<Error> Errors)> ValidateAccountAsync(long accountId, CancellationToken cancellationToken = default)
        {
            var errors = new List<Error>();

            if (accountId <= 0)
            {
                errors.Add(new Error("A receivable account must be selected."));
                return (null, errors);
            }

            var account = await _AccountRepository.GetByFilterAsync(e => e.Id == accountId, string.Empty);
            if (account is null || account.Status == Status.Deleted || account.Hide)
            {
                errors.Add(new Error("The selected account does not exist or is not active."));
                return (null, errors);
            }

            if (!account.IsPostable)
                errors.Add(new Error($"Account '{account.Name}' is a parent/group account and cannot receive postings. Select a detail account."));

            return (account, errors);
        }

        public async Task<(Dealer? Dealer, Account? Account, List<Error> Errors)> ValidateCustomerAsync(long dealerId, CancellationToken cancellationToken = default)
        {
            var errors = new List<Error>();

            var dealer = await _DealerRepository.GetByFilterAsync(e => e.Id == dealerId, string.Empty);
            if (dealer is null || dealer.Status == Status.Deleted || dealer.Hide)
            {
                errors.Add(new Error("The selected customer does not exist or is not active."));
                return (null, null, errors);
            }

            if (dealer.TypeId != (long)Sales.Domain.DealerType.Client)
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

        public async Task<(Dealer? Dealer, Account? Account, List<Error> Errors)> ValidateSupplierAsync(long dealerId, CancellationToken cancellationToken = default)
        {
            var errors = new List<Error>();

            var dealer = await _DealerRepository.GetByFilterAsync(e => e.Id == dealerId, string.Empty);
            if (dealer is null || dealer.Status == Status.Deleted || dealer.Hide)
            {
                errors.Add(new Error("The selected supplier does not exist or is not active."));
                return (null, null, errors);
            }

            if (dealer.TypeId != (long)Sales.Domain.DealerType.Supplier)
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
