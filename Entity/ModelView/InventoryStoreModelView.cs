using Utility;
using Entity.Model;
using Utility.Resource;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Entity.ModelView
{
    public class InventoryStoreModelView : BaseModel
    {
        public InventoryStoreModelView()
        {

        }

        public InventoryStoreModelView(InventoryStore ob)
        {
            if (ob == null)
                ob = new InventoryStore();
            this.Id = ob.Id;
            this.InventoryId = this.InventoryId;
            this.StoreId = this.StoreId;
            this.UserId = this.UserId;
            this.Review = this.Review;
            this.Closed = this.Closed;
            this.CreateTransaction = this.CreateTransaction;
            this.Date = ob.Date;
            this.Hide = ob.Hide;
            this.Notes = ob.Notes;
            this.Status = ob.Status;
            this.ParentId = ob.ParentId;
            this.TypeId = ob.TypeId;
            this.ImgPath = ob.ImgPath;
            if (ob.InventoryStoreProducts == null)
                ob.InventoryStoreProducts = new List<InventoryStoreProduct>();
            this.InventoryStoreProducts = ob.InventoryStoreProducts.Select(e => new InventoryStoreProductModelView(e)).ToList();
        }

        public InventoryStore Model
        {
            get
            {
                return new InventoryStore
                {
                    Id = this.Id,
                    InventoryId = this.InventoryId,
                    StoreId = this.StoreId,
                    UserId = this.UserId,
                    Review = this.Review,
                    Closed = this.Closed,
                    CreateTransaction = this.CreateTransaction,
                    Date = this.Date,
                    Hide = this.Hide,
                    Notes = this.Notes,
                    Status = this.Status,
                    MaskText = this.MaskText,
                    ParentId = this.ParentId,
                    TypeId = this.TypeId,
                    ImgPath = this.ImgPath,
                    InventoryStoreProducts = this.InventoryStoreProducts != null ? this.InventoryStoreProducts.Select(e => e.Model).ToList() : new List<InventoryStoreProduct>()
                };
            }
        }

        [Required]
        public long InventoryId { get; set; }

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
        public bool CreateTransaction { get; set; }
        public bool Closed { get; set; }

        public List<InventoryStoreProductModelView> InventoryStoreProducts { get; set; }
    }
}