namespace Administration.Contracts.Preferences;

using OrgSys.SharedKernel;

/// <summary>
/// Reads a single Preference value by (Reference, TypeId, Key) — the same lookup shape every
/// module's pre-existing "read Preference rows directly via IRepository&lt;Preference&gt;" call
/// already used (see docs/dependency-rules.md §3's Administration.Domain exceptions list). Returns
/// null when no matching Preference row exists or its Value is empty/whitespace.
/// </summary>
public sealed record GetPreferenceValueQuery(string Reference, long TypeId, string Key) : IQuery<string?>;
