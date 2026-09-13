namespace Accounting.Contracts.Postings;

using OrgSys.SharedKernel;

/// <summary>
/// Mirrors a source document's status (e.g. Cancel/Redo) onto its posted Journal(s).
/// Source-neutral by (ReferenceTable, SourceDocumentId) — see PostAccountingDocumentCommand.
/// </summary>
public sealed record SetAccountingDocumentJournalStatusCommand(string ReferenceTable, long SourceDocumentId, Status Status) : ICommand;
