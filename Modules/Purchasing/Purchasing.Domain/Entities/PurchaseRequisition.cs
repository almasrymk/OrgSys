namespace Purchasing.Domain
{
    /// <summary>
    /// An internal request to buy something, raised before a supplier/price is chosen. Lifecycle:
    /// Status.New (draft, still editable) → Status.UnderReview (submitted) →
    /// Status.Approved (converted into a PurchaseOrder — see ConvertToPurchaseOrderCommand). No
    /// separate approval gate is modeled today (brief: submit converts directly, permission-gated
    /// only) — Status.UnderReview/Approved are reused here purely as workflow markers, not as an
    /// approval-chain concept.
    /// </summary>
    [Table("PurchaseRequisition")]
    public class PurchaseRequisition : MovementModel
    {
        [StringLength(500)]
        public virtual string? Notes { get; set; }

        public virtual ICollection<PurchaseRequisitionProduct>? PurchaseRequisitionProducts { get; set; }
    }
}
