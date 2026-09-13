namespace Accounting.Contracts.Accounts;

using OrgSys.SharedKernel;

/// <summary>
/// Batch form of GetAccountQuery — resolves Code/Name/AccountType/postable/active for many
/// accounts at once (e.g. Parties' Dealer list/search screens patching in AccountCode/AccountName
/// for many dealers without an EF Include across the module boundary). Missing ids are simply
/// absent from the result. Mirrors GetAccountingDocumentJournalsQuery's batch shape.
/// </summary>
public sealed record GetAccountLookupsQuery(IReadOnlyCollection<long> AccountIds) : IQuery<Dictionary<long, AccountLookupDto>>;
