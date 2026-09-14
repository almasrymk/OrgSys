namespace Advances.Domain;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

/// <summary>
/// One historical handover record within a Custody — brief §38: transferring a custody must never
/// just overwrite Custody.HolderId, it must preserve an audit trail of who held it, when, and why.
/// No public constructor, only reachable through Custody.TransferHolder. Mirrors
/// Payables.Domain.SupplierPaymentApplicationLine's child-entity shape.
/// </summary>
[Table("CustodyHandover")]
public class CustodyHandover : BaseModel
{
    /// <summary>EF materialization constructor only.</summary>
    protected CustodyHandover() { }

    internal CustodyHandover(long fromHolderId, long toHolderId, DateTime transferDate, decimal transferredAmount, string reason, long approvedByUserId)
    {
        FromHolderId = fromHolderId;
        ToHolderId = toHolderId;
        TransferDate = transferDate;
        TransferredAmount = transferredAmount;
        Reason = reason;
        ApprovedByUserId = approvedByUserId;
    }

    [ForeignKey(nameof(Custody))]
    public virtual long CustodyId { get; internal set; }

    public virtual Custody? Custody { get; set; }

    public virtual long FromHolderId { get; private set; }

    public virtual long ToHolderId { get; private set; }

    public virtual DateTime TransferDate { get; private set; }

    [Column(TypeName = "decimal(18,2)")]
    public virtual decimal TransferredAmount { get; private set; }

    [StringLength(500)]
    public virtual string? Reason { get; private set; }

    public virtual long ApprovedByUserId { get; private set; }
}
