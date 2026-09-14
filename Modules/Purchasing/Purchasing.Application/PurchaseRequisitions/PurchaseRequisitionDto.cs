namespace Purchasing.Application
{
    /// <summary>Standalone DTO — see PurchaseOrderDto's remark for why this no longer inherits
    /// from Purchasing.Domain.PurchaseRequisition.</summary>
    public class PurchaseRequisitionDto
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

        // ----- PurchaseRequisition's own fields -----
        public string? Notes { get; set; }

        // ----- Dto-only additions -----
        public string? CreateUserName { get; set; }
        public List<PurchaseRequisitionProductDto>? PurchaseRequisitionProductList { get; set; }
    }
}
