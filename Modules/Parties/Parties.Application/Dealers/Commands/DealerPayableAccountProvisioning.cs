namespace Parties.Application.Dealers.Commands
{
    using Accounting.Contracts.Accounts;
    using Administration.Contracts.Preferences;
    using MediatR;

    /// <summary>
    /// Resolves the payable account a Supplier Dealer should be linked to on Create/Update: validates
    /// an explicitly-selected account, or — when requested — resolves the configured AP parent account
    /// so the caller can provision a new supplier sub-account under it (via
    /// Accounting.Contracts.Accounts.ProvisionSubAccountCommand) inside its own transaction and link the
    /// freshly-generated Id to the Dealer before saving it. Account creation itself (code generation,
    /// parent validity) is Accounting's own concern — see ProvisionSubAccountCommand.
    /// Mirrors <see cref="DealerReceivableAccountProvisioning"/> for the AP (Supplier) side — see
    /// that type's doc comment for why neither helper guards on dealer.TypeId internally (the
    /// dual-role AssignSupplierRoleCommand may run against a Dealer whose primary TypeId is Client).
    /// </summary>
    internal static class DealerPayableAccountProvisioning
    {
        public static async Task<(long? ExistingAccountId, long? ProvisionParentAccountId, List<Error> Errors)> ResolveAsync(
            Parties.Domain.Dealer dealer,
            long? requestedAccountId,
            bool? autoCreate,
            IReceivableAccountValidator validator,
            ISender sender,
            CancellationToken cancellationToken)
        {
            var errors = new List<Error>();

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

            var preferences = (await sender.Send(
                new GetPreferenceValuesQuery("Dealer", (long)DealerType.Supplier), cancellationToken)).Response
                ?? new Dictionary<string, string?>();
            var parentAccountId = preferences.TryGetValue("PayableParentAccountId", out var raw)
                && long.TryParse(raw, out var id) ? id : 0;
            if (parentAccountId <= 0)
            {
                errors.Add(new Error("Auto-create payable account is not configured (PayableParentAccountId preference)."));
                return (null, null, errors);
            }

            var parent = (await sender.Send(new GetAccountQuery(parentAccountId), cancellationToken)).Response;
            if (parent is null || !parent.IsActive)
            {
                errors.Add(new Error("The configured Accounts Payable parent account is missing or inactive."));
                return (null, null, errors);
            }

            return (null, parentAccountId, errors);
        }
    }
}
