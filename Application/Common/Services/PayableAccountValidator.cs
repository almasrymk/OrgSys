namespace Application.Common.Services
{
    using Domain.Abstraction;
    using Domain.Entities;
    using Domain.Enums;
    using Domain.Shared;

    public sealed class PayableAccountValidator(
        IReceivableAccountValidator _AccountValidator,
        IRepository<Dealer> _DealerRepository) : IPayableAccountValidator
    {
        public async Task<(Dealer? Dealer, Account? Account, List<Error> Errors)> ValidateSupplierAsync(long dealerId, CancellationToken cancellationToken = default)
        {
            var errors = new List<Error>();

            var dealer = await _DealerRepository.GetByFilterAsync(e => e.Id == dealerId, string.Empty);
            if (dealer is null || dealer.Status == Status.Deleted || dealer.Hide)
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

            var (account, accountErrors) = await _AccountValidator.ValidateAccountAsync(dealer.AccountId.Value, cancellationToken);
            errors.AddRange(accountErrors);
            return (dealer, account, errors);
        }
    }
}
