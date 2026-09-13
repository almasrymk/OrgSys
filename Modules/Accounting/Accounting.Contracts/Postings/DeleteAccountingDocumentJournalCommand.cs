namespace Accounting.Contracts.Postings;

using OrgSys.SharedKernel;

/// <summary>
/// Deletes the Journal(s) posted against a source document (e.g. an Invoice or Transaction being
/// deleted, or whose posting preferences turned off). Source-neutral by (ReferenceTable,
/// SourceDocumentId) — see PostAccountingDocumentCommand.
/// </summary>
public sealed record DeleteAccountingDocumentJournalCommand(string ReferenceTable, long SourceDocumentId) : ICommand;
