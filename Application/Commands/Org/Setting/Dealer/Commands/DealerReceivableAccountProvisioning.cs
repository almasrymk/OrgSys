namespace Application.Commands.Org.Setting.Dealer.Commands
{
    using Application.Common.Services;
    using Domain.Abstraction;
    using Domain.Entities;
    using Domain.Enums;
    using Domain.Shared;

    /// <summary>
    /// Resolves the receivable account a Client Dealer should be linked to on Create/Update: validates
    /// an explicitly-selected account, or — when requested — builds (but does not yet persist) a new
    /// customer sub-account under the configured AR parent, so the caller can create it inside its own
    /// transaction and link the freshly-generated Id to the Dealer before saving it.
    /// Suppliers are untouched (AP is out of scope) — only Client dealers participate in these rules.
    /// </summary>
    internal static class DealerReceivableAccountProvisioning
    {
        public static async Task<(long? ExistingAccountId, Account? AccountToCreate, List<Error> Errors)> ResolveAsync(
            Domain.Entities.Dealer dealer,
            long? requestedAccountId,
            bool? autoCreate,
            IReceivableAccountValidator validator,
            IRepository<Preference> preferenceRepository,
            IRepository<Account> accountRepository,
            CancellationToken cancellationToken)
        {
            var errors = new List<Error>();

            if (dealer.TypeId != (long)DealerType.Client)
                return (requestedAccountId, null, errors);

            if (requestedAccountId is > 0)
            {
                var (_, accountErrors) = await validator.ValidateAccountAsync(requestedAccountId.Value, cancellationToken);
                errors.AddRange(accountErrors);
                return (requestedAccountId, null, errors);
            }

            if (autoCreate != true)
            {
                errors.Add(new Error("A customer must be linked to a receivable account: select an existing account or enable auto-create."));
                return (null, null, errors);
            }

            var preferences = (await preferenceRepository.GetListByFilterAsync(
                e => e.Reference == "Dealer" && e.TypeId == (long)DealerType.Client))?.ToList() ?? [];
            var parentAccountId = long.TryParse(preferences.FirstOrDefault(e => e.Key == "ReceivableParentAccountId")?.Value, out var id) ? id : 0;
            if (parentAccountId <= 0)
            {
                errors.Add(new Error("Auto-create receivable account is not configured (ReceivableParentAccountId preference)."));
                return (null, null, errors);
            }

            var parent = await accountRepository.GetByFilterAsync(e => e.Id == parentAccountId, string.Empty);
            if (parent is null || parent.Status == Status.Deleted || parent.Hide)
            {
                errors.Add(new Error("The configured Accounts Receivable parent account is missing or inactive."));
                return (null, null, errors);
            }

            var siblingCount = (await accountRepository.GetListByFilterAsync(e => e.ParentId == parent.Id))?.Count() ?? 0;
            var code = $"{parent.Code}{(siblingCount + 1):00}";
            if (!long.TryParse(code, out var codeNumber))
            {
                errors.Add(new Error("Could not generate a numeric account code for the new customer account."));
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
