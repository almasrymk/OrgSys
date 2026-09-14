namespace Purchasing.Application
{
    /// <summary>
    /// Standalone DTO — deliberately does NOT inherit from Purchasing.Domain.PurchaseOrder (unlike
    /// before this hardening pass; see PurchaseOrderProductDto's identical remark). Field set
    /// (BaseModel + MovementModel + PurchaseOrder's own properties, plus the Dto-only additions) is
    /// identical to what the old inherited DTO exposed, so the JSON wire shape/HTTP contract is
    /// unchanged for existing callers.
    /// </summary>
    public class PurchaseOrderDto
    {
        // ----- BaseModel fields -----
        public long Id { get; set; }
        public long CodeNumber { get; set; }
        public string? Code { get; set; }
        public string? MaskText { get; set; }
        public long ParentId { get; set; }
        public long TypeId { get; set; }
        public bool Hide { get; set; }
        public string? ImgPath { get; set; }
        public Status Status { get; set; }

        // ----- MovementModel fields -----
        public DateTime Date { get; set; }
        public long CreateUserId { get; set; }
        public DateTime CreateDate { get; set; }
        public long? ModifyUserId { get; set; }
        public DateTime? ModifyDate { get; set; }
        public long? ShiftId { get; set; }
        public long? BranchId { get; set; }
        public bool HasJournal { get; set; }
        public bool Review { get; set; }
        public bool Posted { get; set; }

        // ----- PurchaseOrder's own fields -----
        public long DealerId { get; set; }
        public long? PurchaseRequisitionId { get; set; }
        public long? InvoiceId { get; set; }
        public decimal Total { get; set; }
        public decimal Discount { get; set; }
        public int DiscountType { get; set; }
        public decimal Tax { get; set; }
        public int TaxType { get; set; }
        public decimal Net { get; set; }
        public string? Notes { get; set; }

        // ----- Dto-only additions -----
        public string? DealerName { get; set; }
        public string? CreateUserName { get; set; }
        public List<PurchaseOrderProductDto>? PurchaseOrderProductList { get; set; }
    }
}
