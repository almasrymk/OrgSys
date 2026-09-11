namespace Sales.Application.Dealers.Commands
{
    using Accounting.Application;

    /// <summary>
    /// Resolves the payable account a Supplier Dealer should be linked to on Create/Update: validates
    /// an explicitly-selected account, or — when requested — builds (but does not yet persist) a new
    /// supplier sub-account under the configured AP parent, so the caller can create it inside its own
    /// transaction and link the freshly-generated Id to the Dealer before saving it.
    /// Mirrors <see cref="DealerReceivableAccountProvisioning"/> for the AP (Supplier) side — Client
    /// dealers are untouched here.
    /// </summary>
    internal static class DealerPayableAccountProvisioning
    {
        public static async Task<(long? ExistingAccountId, Account? AccountToCreate, List<Error> Errors)> ResolveAsync(
            Sales.Domain.Dealer dealer,
            long? requestedAccountId,
            bool? autoCreate,
            IReceivableAccountValidator validator,
            IRepository<Preference> preferenceRepository,
            IRepository<Account> accountRepository,
            CancellationToken cancellationToken)
        {
            var errors = new List<Error>();

            if (dealer.TypeId != (long)DealerType.Supplier)
                return (requestedAccountId, null, errors);

            if (requestedAccountId is > 0)
            {
                var (_, accountErrors) = await validator.ValidateAccountAsync(requestedAccountId.Value, cancellationToken);
                errors.AddRange(accountErrors);
                return (requestedAccountId, null, errors);
            }

            if (autoCreate != true)
            {
                errors.Add(new Error("A supplier must be linked to a payable account: select an existing account or enable auto-create."));
                return (null, null, errors);
            }

            var preferences = (await preferenceRepository.GetListByFilterAsync(
                e => e.Reference == "Dealer" && e.TypeId == (long)DealerType.Supplier))?.ToList() ?? [];
            var parentAccountId = long.TryParse(preferences.FirstOrDefault(e => e.Key == "PayableParentAccountId")?.Value, out var id) ? id : 0;
            if (parentAccountId <= 0)
            {
                errors.Add(new Error("Auto-create payable account is not configured (PayableParentAccountId preference)."));
                return (null, null, errors);
            }

            var parent = await accountRepository.GetByFilterAsync(e => e.Id == parentAccountId, string.Empty);
            if (parent is null || parent.Status == Status.Deleted || parent.Hide)
            {
                errors.Add(new Error("The configured Accounts Payable parent account is missing or inactive."));
                return (null, null, errors);
            }

            var siblingCount = (await accountRepository.GetListByFilterAsync(e => e.ParentId == parent.Id))?.Count() ?? 0;
            var code = $"{parent.Code}{(siblingCount + 1):00}";
            if (!long.TryParse(code, out var codeNumber))
            {
                errors.Add(new Error("Could not generate a numeric account code for the new supplier account."));
                return (null, null, errors);
            }

            var account = new Account
            {
                Name = dealer.Name,
                Code = code,
                CodeNumber = codeNumber,
                ParentId = parent.Id,
                AccountTypeId = parent.AccountTypeId,
                IsPostable = true,
                Hide = false,
                Debit = 0,
                Credit = 0
            };
            return (null, account, errors);
        }
    }
}
