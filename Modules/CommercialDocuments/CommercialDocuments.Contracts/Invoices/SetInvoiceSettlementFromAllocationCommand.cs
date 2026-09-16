namespace CommercialDocuments.Contracts.Invoices;

using OrgSys.SharedKernel;

/// <summary>
/// Replaces Treasury's historic create-time formula:
/// Credit = (Net - allocatedAmount) - Paid; Paid = Net - Credit.
/// </summary>
public record SetInvoiceSettlementFromAllocationCommand(long InvoiceId, decimal AllocatedAmount) : ICommand;
