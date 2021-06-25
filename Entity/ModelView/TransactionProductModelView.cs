using Utility;
using Entity.Model;
using Utility.Resource;
using System.ComponentModel.DataAnnotations;
using System;
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
            this.Id = ob.Id;
            this.TransactionId = ob.TransactionId;
            this.RowNumber = ob.RowNumber;
            this.ProductId = ob.ProductId;
            this.ProductName = ob.Product?.Name;
            this.StoreId = ob.StoreId;
            this.StoreName = ob.Store?.Name;
            this.UnitId = ob.UnitId;
            this.UnitName = ob.Unit?.Name;
            this.Quantity = ob.Quantity;
            this.Hide = ob.Hide;
            this.Notes = ob.Notes;
            this.Cost = ob.Cost;
            this.Total = ob.Total;           
            this.Status = ob.Status;
            this.ParentId = ob.ParentId;
            this.TypeId = ob.TypeId;
            this.ImgPath = ob.ImgPath;
            this.Units = new List<UnitModelView>();
            if (ob.Product != null && ob.Product.ProductUnits != null)
                this.Units = ob.Product.ProductUnits.Select(e => new UnitModelView(e.Unit)).ToList();           
        }

        public TransactionProductModelView(InvoiceProduct ob , long _StoreId)
        {
            if (ob == null)
                ob = new InvoiceProduct();
            this.RowNumber = ob.RowNumber;
            this.ProductId = ob.ProductId;
            this.ProductName = ob.Product?.Name;
            this.UnitId = ob.UnitId;
            this.UnitName = ob.Unit?.Name;
            this.Quantity = ob.Quantity;
            this.Cost = ob.Price - ob.Discount;
            this.Hide = ob.Hide;
            this.Notes = ob.Notes;
            this.StoreId = _StoreId;
            this.Total = ob.Net;
            this.Status = ob.Status;
            this.ParentId = ob.ParentId;
            this.TypeId = ob.TypeId;
            this.ImgPath = ob.ImgPath;
            this.Units = new List<UnitModelView>();
            if (ob.Product != null && ob.Product.ProductUnits != null)
                this.Units = ob.Product.ProductUnits.Select(e => new UnitModelView(e.Unit)).ToList();
        }

        public TransactionProductModelView(TransactionProduct ob, long _StoreId)
        {
            if (ob == null)
                ob = new TransactionProduct();
            this.RowNumber = ob.RowNumber;
            this.ProductId = ob.ProductId;
            this.ProductName = ob.Product?.Name;
            this.UnitId = ob.UnitId;
            this.UnitName = ob.Unit?.Name;
            this.Quantity = ob.Quantity;
            this.Cost = ob.Cost;
            this.Hide = ob.Hide;
            this.Notes = ob.Notes;
            this.StoreId = _StoreId;
            this.Total = ob.Total;
            this.Status = ob.Status;
            this.ParentId = ob.ParentId;
            this.TypeId = ob.TypeId;
            this.ImgPath = ob.ImgPath;
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
                    Id = this.Id,
                    TransactionId = this.TransactionId,
                    ProductId = this.ProductId,
                    StoreId = this.StoreId,
                    UnitId = this.UnitId,
                    Quantity = this.Quantity,
                    RowNumber = this.RowNumber,
                    Cost = this.Cost,
                    Total = this.Total,
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