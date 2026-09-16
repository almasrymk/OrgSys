namespace Administration.Contracts.Preferences;

using OrgSys.SharedKernel;

/// <summary>
/// Reads every Preference row for a (Reference, TypeId) pair — the batch form of
/// <see cref="GetPreferenceValueQuery"/> used by posting bridges that consult several keys
/// (AccountsIntegration, AutoCreateJournalEntry, SalesAccount, ...).
/// </summary>
public sealed record GetPreferenceValuesQuery(string Reference, long TypeId)
    : IQuery<IReadOnlyDictionary<string, string?>>;
