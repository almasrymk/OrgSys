namespace Parties.Application.Dealers.Commands
{
    using Accounting.Contracts.Accounts;
    using MediatR;

    /// <summary>
    /// Resolves the receivable account a Client Dealer should be linked to on Create/Update: validates
    /// an explicitly-selected account, or — when requested — resolves the configured AR parent account
    /// so the caller can provision a new customer sub-account under it (via
    /// Accounting.Contracts.Accounts.ProvisionSubAccountCommand) inside its own transaction and link the
    /// freshly-generated Id to the Dealer before saving it. Account creation itself (code generation,
    /// parent validity) is Accounting's own concern — see ProvisionSubAccountCommand.
    /// Suppliers are untouched (AP is out of scope) — only Client dealers participate in these rules.
    /// </summary>
    internal static class DealerReceivableAccountProvisioning
    {
        public static async Task<(long? ExistingAccountId, long? ProvisionParentAccountId, List<Error> Errors)> ResolveAsync(
            Parties.Domain.Dealer dealer,
            long? requestedAccountId,
            bool? autoCreate,
            IReceivableAccountValidator validator,
            IRepository<Preference> preferenceRepository,
            ISender sender,
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

            var parent = (await sender.Send(new GetAccountQuery(parentAccountId), cancellationToken)).Response;
            if (parent is null || !parent.IsActive)
            {
                errors.Add(new Error("The configured Accounts Receivable parent account is missing or inactive."));
                return (null, null, errors);
            }

            return (null, parentAccountId, errors);
        }
    }
}
