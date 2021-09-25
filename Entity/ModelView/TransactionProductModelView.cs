using Entity.Model;
using System.Collections.Generic;
using System.Linq;

namespace Entity.ModelView
{
    public class TransactionProductModelView : BaseModel
    {
        public TransactionProductModelView()
        {

        }

        public TransactionProductModelView(TransactionProduct ob)
        {
            if (ob == null)
                ob = new TransactionProduct();

            this.TransactionId = ob.TransactionId;

            this.RowNumber = ob.RowNumber;

            this.ProductId = ob.ProductId;

            this.ProductName = ob.Product?.Name;

            this.StoreId = ob.StoreId;

            this.StoreName = ob.Store?.Name;

            this.UnitId = ob.UnitId;

            this.UnitName = ob.Unit?.Name;

            this.Quantity = ob.Quantity;

            this.Notes = ob.Notes;

            this.Cost = ob.Cost;

            this.Total = ob.Total;

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

        public TransactionProductModelView(InvoiceProduct ob)
        {
            if (ob == null)
                ob = new InvoiceProduct();

            this.RowNumber = ob.RowNumber;

            this.ProductId = ob.ProductId;

            this.UnitId = ob.UnitId;

            this.Quantity = ob.Quantity;

            this.Cost = ob.Price - ob.Discount;

            this.Notes = ob.Notes;

            this.StoreId = ob.StoreId;

            this.Total = ob.Net;

            //this.Id = ob.Id;

            this.CodeNumber = ob.CodeNumber;

            this.Code = ob.Code;

            this.MaskText = ob.MaskText;

            this.ParentId = ob.ParentId;

            this.TypeId = ob.Invoice?.TypeId == 2 || ob.Invoice?.TypeId == 3 ? 1 : 2;

            this.Hide = ob.Hide;

            this.ImgPath = ob.ImgPath;

            this.Status = ob.Status;

            this.Units = new List<UnitModelView>();

            if (ob.Product != null && ob.Product.ProductUnits != null)
                this.Units = ob.Product.ProductUnits.Select(e => new UnitModelView(e.Unit)).ToList();
        }

        public TransactionProductModelView(InventoryProduct ob)
        {
            if (ob == null)
                ob = new InventoryProduct();

            this.RowNumber = ob.RowNumber;

            this.ProductId = ob.ProductId;

            this.ProductName = ob.Product?.Name;

            this.UnitId = ob.UnitId;

            this.UnitName = ob.Unit?.Name;

            this.Quantity = ob.DiffQuantity < 0 ? -1 *  ob.DiffQuantity : ob.DiffQuantity;

            this.Cost = 0;

            this.Notes = ob.Notes; 
            
            this.Total = 0;

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
      
        public TransactionProduct Model
        {
            get
            {
                return new TransactionProduct
                {
                    TransactionId = this.TransactionId,
                    ProductId = this.ProductId,
                    StoreId = this.StoreId,
                    UnitId = this.UnitId,
                    Quantity = this.Quantity,
                    RowNumber = this.RowNumber,
                    Cost = this.Cost,
                    Total = this.Total,
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
        }

        public long RowNumber { get; set; }

        public long TransactionId { get; set; }

        public long ProductId { get; set; }

        public string ProductName { get; set; }

        public long UnitId { get; set; }

        public string UnitName { get; set; }

        public long StoreId { get; set; }

        public string StoreName { get; set; }

        public decimal Quantity { get; set; }

        public decimal Cost { get; set; }

        public decimal Total { get; set; }

        public string Notes { get; set; }

        public List<UnitModelView> Units { get; set; }
    }
}