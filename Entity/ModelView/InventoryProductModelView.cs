using Entity.Model;
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

            this.RowNumber = ob.RowNumber;

            this.ProductId = ob.ProductId;

            this.ProductName = ob.Product?.Name;

            this.UnitId = ob.UnitId;

            this.UnitName = ob.Unit?.Name;

            this.ActualBalance = ob.ActualBalance;

            this.CalcBalance = ob.CalcBalance;

            this.DiffQuantity = ob.DiffQuantity;

            this.InventoryId = ob.InventoryId;

            this.Notes = ob.Notes;

            this.Id = ob.Id;

            this.CodeNumber = ob.CodeNumber;

            this.Code = ob.Code;

            this.MaskText = ob.MaskText;

            this.ParentId = ob.ParentId;

            this.TypeId = ob.TypeId;

            this.Hide = ob.Hide;

            this.ImgPath = ob.ImgPath;

            this.Status = ob.Status;

            this.Units = new List<UnitModelView>();

            if (ob.Product != null && ob.Product.ProductUnits != null)
                this.Units = ob.Product.ProductUnits.Select(e => new UnitModelView(e.Unit)).ToList();
        }

        public InventoryProduct Model()
        {
            return new InventoryProduct
            {
                ProductId = this.ProductId,
                UnitId = this.UnitId,
                ActualBalance = this.ActualBalance,
                CalcBalance = this.CalcBalance,
                DiffQuantity = this.DiffQuantity,
                InventoryId = this.InventoryId,
                RowNumber = this.RowNumber,
                Notes = this.Notes,
                Id = this.Id,
                CodeNumber = this.CodeNumber,
                Code = this.Code,
                MaskText = this.MaskText,
                ParentId = this.ParentId,
                TypeId = this.TypeId,
                Hide = this.Hide,
                Status = this.Status,
                ImgPath = this.ImgPath
            };
        }

        public long RowNumber { get; set; }

        public long InventoryId { get; set; }

        public long ProductId { get; set; }

        public string ProductName { get; set; }

        public long UnitId { get; set; }

        public string UnitName { get; set; }

        public decimal CalcBalance { get; set; }

        public decimal ActualBalance { get; set; }

        public decimal DiffQuantity { get; set; }

        public string Notes { get; set; }

        public List<UnitModelView> Units { get; set; }
    }
}