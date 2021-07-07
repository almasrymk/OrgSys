using Utility;
using Entity.Model;
using Utility.Resource;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Entity.ModelView
{
    public class InventoryModelView : BaseModel
    {
        public InventoryModelView()
        {

        }

        public InventoryModelView(Inventory ob)
        {
            if (ob == null)
                ob = new Inventory();
            this.Id = ob.Id;
            this.CodeNumber = ob.CodeNumber;
            this.Code = ob.Code;
            this.Date = ob.Date;
            this.StoreId = ob.StoreId;
            this.StoreName = ob.Store?.Name;
            this.UserId = ob.UserId;
            this.UserName = ob.User?.Name;
            this.Review = ob.Review;
            this.Closed = ob.Closed;
            this.Hide = ob.Hide;
            this.Notes = ob.Notes;
            this.Status = ob.Status;
            this.ParentId = ob.ParentId;
            this.TypeId = ob.TypeId;
            this.ImgPath = ob.ImgPath;
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
                    Id = this.Id,
                    CodeNumber = this.CodeNumber,
                    Code = this.Code,
                    Date = this.Date,
                    StoreId = this.StoreId,
                    UserId = this.UserId,
                    Closed = this.Closed,
                    Review = this.Review,
                    Hide = this.Hide,
                    Notes = this.Notes,
                    Status = this.Status,
                    MaskText = this.MaskText,
                    ParentId = this.ParentId,
                    TypeId = this.TypeId,
                    ImgPath = this.ImgPath,
                    InventoryProducts = this.InventoryProducts != null ? this.InventoryProducts.Select(e => e.Model).ToList() : new List<InventoryProduct>()
                };
            }
        }

        public long CodeNumber { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        public DateTime Date { get; set; }
        [Required]
        public long StoreId { get; set; }

        public string StoreName { get; set; }
        public long? UserId { get; set; }

        public string UserName { get; set; }

        [StringLength(500)]
        public string Notes { get; set; }
        public bool Review { get; set; }
        public bool Closed { get; set; }
        public List<InventoryProductModelView> InventoryProducts { get; set; }
    }
}