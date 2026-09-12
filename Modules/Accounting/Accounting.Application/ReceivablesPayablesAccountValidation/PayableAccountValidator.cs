namespace Accounting.Application
{
    using MediatR;
    using Parties.Contracts.Dealers;

    public sealed class PayableAccountValidator(
        IReceivableAccountValidator _AccountValidator,
        ISender _Sender) : IPayableAccountValidator
    {
        public async Task<(DealerLookupDto? Dealer, Account? Account, List<Error> Errors)> ValidateSupplierAsync(long dealerId, CancellationToken cancellationToken = default)
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

            var (account, accountErrors) = await _AccountValidator.ValidateAccountAsync(dealer.AccountId.Value, cancellationToken);
            errors.AddRange(accountErrors);
            return (dealer, account, errors);
        }
    }
}
