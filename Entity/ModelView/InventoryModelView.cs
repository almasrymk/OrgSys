using Entity.Model;
using System.Collections.Generic;
using System.Linq;

namespace Entity.ModelView
{
    public class InventoryModelView : MovementModel
    {
        public InventoryModelView()
        {

        }

        public InventoryModelView(Inventory ob)
        {
            if (ob == null)
                ob = new Inventory();

            this.StoreId = ob.StoreId;

            this.StoreName = ob.Store?.Name;

            this.UserId = ob.UserId;

            this.UserName = ob.User?.Name;

            this.Review = ob.Review;

            this.Closed = ob.Closed;

            this.Notes = ob.Notes;

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

            if (ob.InventoryProducts == null)
                ob.InventoryProducts = new List<InventoryProduct>();

            this.InventoryProducts = ob.InventoryProducts.Select(e => new InventoryProductModelView(e)).ToList();
        }

        public Inventory Model
        {
            get
            {
                return new Inventory
                {
                    StoreId = this.StoreId,
                    UserId = this.UserId,
                    Closed = this.Closed,
                    Review = this.Review,
                    Notes = this.Notes,
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
                    InventoryProducts = this.InventoryProducts != null ? this.InventoryProducts.Select(e => e.Model).ToList() : new List<InventoryProduct>()
                };
            }
        }

        public long StoreId { get; set; }

        public string StoreName { get; set; }

        public long? UserId { get; set; }

        public string UserName { get; set; }

        public string Notes { get; set; }

        public bool Review { get; set; }

        public bool Closed { get; set; }

        public string BranchName { get; set; }

        public string CreateUserName { get; set; }

        public string ModifyUserName { get; set; }

        public string ShiftName { get; set; }

        public List<InventoryProductModelView> InventoryProducts { get; set; }
    }
}