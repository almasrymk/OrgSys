using Utility;
using Entity.Model;
using Utility.Resource;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Entity.ModelView
{
    public class InventoryProductModelView : BaseModel
    {
        public InventoryProductModelView()
        {

        }

        public InventoryProductModelView(InventoryProduct ob)
        {
            if (ob == null)
                ob = new InventoryProduct();
            this.Id = ob.Id;
            this.RowNumber = ob.RowNumber;
            this.ProductId = ob.ProductId;
            this.ProductName = ob.Product?.Name;
            this.UnitId = ob.UnitId;
            this.UnitName = ob.Unit?.Name;
            this.ActualBalance = this.ActualBalance;
            this.CalcBalance = this.CalcBalance;
            this.DiffQuantity = this.DiffQuantity;
            this.InventoryStoreId = this.InventoryStoreId;
            this.Hide = ob.Hide;
            this.Notes = ob.Notes;         
            this.Status = ob.Status;
            this.ParentId = ob.ParentId;
            this.TypeId = ob.TypeId;
            this.ImgPath = ob.ImgPath;
            this.Units = new List<UnitModelView>();
            if (ob.Product != null && ob.Product.ProductUnits != null)
                this.Units = ob.Product.ProductUnits.Select(e => new UnitModelView(e.Unit)).ToList();           
        }

        public InventoryProduct Model
        {
            get
            {
                return new InventoryProduct
                {
                    Id = this.Id,
                    ProductId = this.ProductId,
                    UnitId = this.UnitId,
                    ActualBalance = this.ActualBalance,
                    CalcBalance = this.CalcBalance,
                    DiffQuantity = this.DiffQuantity,
                    InventoryStoreId = this.InventoryStoreId,
                    RowNumber = this.RowNumber,
                    Hide = this.Hide,
                    Notes = this.Notes,
                    Status = this.Status,
                    MaskText = this.MaskText,
                    ParentId = this.ParentId,
                    TypeId = this.TypeId,
                    ImgPath = this.ImgPath
                };
            }
        }

        [Required]
        public long RowNumber { get; set; }

        [Required]
        public long InventoryStoreId { get; set; }


        [Required]
        public long ProductId { get; set; }

        public string ProductName { get; set; }

        [Required]
        public long UnitId { get; set; }

        public string UnitName { get; set; }

        public decimal CalcBalance { get; set; }
        public decimal ActualBalance { get; set; }
        public decimal DiffQuantity { get; set; }

        public string Notes { get; set; }
        public List<UnitModelView> Units { get; set; }
    }
}