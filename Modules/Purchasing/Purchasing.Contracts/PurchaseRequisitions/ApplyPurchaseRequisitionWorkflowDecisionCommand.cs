namespace Purchasing.Contracts.PurchaseRequisitions;

using OrgSys.SharedKernel;

/// <summary>Workflow notifies Purchasing of an approval decision. Workflow never mutates a PurchaseOrder.</summary>
public sealed record ApplyPurchaseRequisitionWorkflowDecisionCommand(long PurchaseRequisitionId, bool Approved) : ICommand;
