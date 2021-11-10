using Entity.Model;
using System.Collections.Generic;
using System.Linq;

namespace Entity.ModelView
{
    public class TransactionModelView : MovementModel
    {
        public TransactionModelView()
        {

        }

        public TransactionModelView(Transaction ob)
        {
            if (ob == null)
                ob = new Transaction();

            this.DealerId = ob.DealerId;

            this.DealerName = ob.Dealer?.Name;

            this.Notes = ob.Notes;

            this.OrderId = ob.OrderId;

            this.StoreId = ob.StoreId;

            this.StoreName = ob.Store?.Name;

            this.ToStoreId = ob.ToStoreId;

            this.ToStoreName = ob.ToStore?.Name;

            this.Total = ob.Total;

            this.BranchId = ob.BranchId;

            this.BranchName = ob.Branch?.Name;

            this.CreateUserId = ob.CreateUserId;

            this.CreateUserName = ob.CreateUser?.Name;

            this.ModifyUserId = ob.ModifyUserId;

            this.ModifyUserName = ob.ModifyUser?.Name;

            this.ShiftId = ob.ShiftId;

            this.ShiftName = ob.Shift?.Name;

            this.Date = ob.Date;

            this.CreateDate = ob.CreateDate;

            this.ModifyDate = ob.ModifyDate;

            this.Id = ob.Id;

            this.CodeNumber = ob.CodeNumber;

            this.Code = ob.Code;

            this.MaskText = ob.MaskText;

            this.ParentId = ob.ParentId;

            this.TypeId = ob.TypeId;

            this.Hide = ob.Hide;

            this.ImgPath = ob.ImgPath;

            this.Status = ob.Status;

            if (ob.TransactionProducts == null)
                ob.TransactionProducts = new List<TransactionProduct>();

            this.TransactionProducts = ob.TransactionProducts.Select(e => new TransactionProductModelView(e)).ToList();
        }

        public TransactionModelView(Invoice ob)
        {
            if (ob == null)
                ob = new Invoice();

            this.Notes = ob.Notes;

            this.StoreId = ob.StoreId;

            this.DealerId = ob.DealerId;

            this.Total = ob.Total;

            this.BranchId = ob.BranchId;
           
            this.CreateUserId = ob.CreateUserId;

            this.CreateDate = ob.CreateDate;

            this.ModifyUserId = ob.ModifyUserId;

            this.ModifyDate = ob.ModifyDate;

            this.ShiftId = ob.ShiftId;

            this.Date = ob.Date;

            this.MaskText = ob.MaskText;

            this.ParentId = ob.ParentId;

            this.TypeId = ob.TypeId == 2 || ob.TypeId == 3 ? 1 : 2;

            this.Hide = ob.Hide;

            this.ImgPath = ob.ImgPath;

            this.Status = ob.Status;

            if (ob.InvoiceProducts == null)
                ob.InvoiceProducts = new List<InvoiceProduct>();

            this.TransactionProducts = ob.InvoiceProducts.Select(e => new TransactionProductModelView(e)).ToList();
        }

        public Transaction Model()
        {
           return new Transaction
                {
                    DealerId = this.DealerId,
                    Total = this.Total,
                    Notes = this.Notes,
                    OrderId = this.OrderId,
                    StoreId = this.StoreId,
                    ToStoreId = this.ToStoreId,
                    BranchId = this.BranchId,
                    CreateUserId = this.CreateUserId,
                    ModifyUserId = this.ModifyUserId,
                    ShiftId = this.ShiftId,
                    Date = this.Date,
                    CreateDate = this.CreateDate,
                    ModifyDate = this.ModifyDate,
                    Id = this.Id,
                    CodeNumber = this.CodeNumber,
                    Code = this.Code,
                    MaskText = this.MaskText,
                    ParentId = this.ParentId,
                    TypeId = this.TypeId,
                    Hide = this.Hide,
                    Status = this.Status,
                    ImgPath = this.ImgPath,
                    TransactionProducts = this.TransactionProducts != null ? this.TransactionProducts.Select(e => e.Model()).ToList() : new List<TransactionProduct>()
    };
}

        public long? DealerId { get; set; }

        public string DealerName { get; set; }
     
        public long StoreId { get; set; }

        public string StoreName { get; set; }

        public long? ToStoreId { get; set; }

        public string ToStoreName { get; set; }

        public long? OrderId { get; set; }

        public decimal Total { get; set; }

        public string ParentCode { get; set; }

        public string Notes { get; set; }

        public string BranchName { get; set; }

        public string CreateUserName { get; set; }

        public string ModifyUserName { get; set; }

        public string ShiftName { get; set; }

        public List<TransactionProductModelView> TransactionProducts { get; set; }
    }
}