namespace Accounting.Contracts.Accounts;

using OrgSys.SharedKernel;

/// <summary>
/// Creates a new postable detail Account as a child of an existing Chart-of-Accounts parent —
/// e.g. auto-provisioning a Dealer's own AR/AP sub-account under the Preferences-configured
/// Receivable/Payable parent (see Parties.Application.Dealers.Commands.
/// DealerReceivableAccountProvisioning/DealerPayableAccountProvisioning). The code/code-number
/// generation (parent code + sequential suffix) and the parent's own validity are Accounting's
/// own concern; the caller supplies only what account it wants and under which parent.
/// </summary>
public sealed record ProvisionSubAccountCommand(long ParentAccountId, string Name) : ICommand<AccountLookupDto>;
