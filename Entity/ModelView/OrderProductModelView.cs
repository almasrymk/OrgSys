using Utility;
using Entity.Model;
using Utility.Resource;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Entity.ModelView
{
    public class OrderProductModelView : BaseModel
    {
        public OrderProductModelView()
        {

        }

        public OrderProductModelView(OrderProduct ob)
        {
            if (ob == null)
                ob = new OrderProduct();
            this.Id = ob.Id;
            this.OrderId = ob.OrderId;
            this.RowNumber = ob.RowNumber;
            this.ProductId = ob.ProductId;
            this.ProductName = ob.Product?.Name;
            this.UnitId = ob.UnitId;
            this.UnitName = ob.Unit?.Name;
            this.Quantity = ob.Quantity;
            this.Price = ob.Price;
            this.Discount = ob.Discount;
            this.Hide = ob.Hide;
            this.Net = ob.Net;
            this.Notes = ob.Notes;         
            this.Tax = ob.Tax;
            this.Total = ob.Total;           
            this.Status = ob.Status;
            this.ParentId = ob.ParentId;
            this.TypeId = ob.TypeId;
            this.ImgPath = ob.ImgPath;
            this.Units = new List<UnitModelView>();
            if (ob.Product != null && ob.Product.ProductUnits != null)
                this.Units = ob.Product.ProductUnits.Select(e => new UnitModelView(e.Unit)).ToList();           
        }

        public OrderProduct Model
        {
            get
            {
                return new OrderProduct
                {
                    Id = this.Id,
                    OrderId = this.OrderId,
                    ProductId = this.ProductId,
                    UnitId = this.UnitId,
                    Quantity = this.Quantity,
                    Price = this.Price,
                    RowNumber = this.RowNumber,
                    Discount = this.Discount,
                    Total = this.Total,
                    Tax = this.Tax,
                    Hide = this.Hide,
                    Net  = this.Net,
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
        public long OrderId { get; set; }

        [Required]
        public long ProductId { get; set; }

        public string ProductName { get; set; }

        [Required]
        public long UnitId { get; set; }

        public string UnitName { get; set; }

        public decimal Quantity { get; set; }

        public decimal Price { get; set; }

        public decimal Total { get; set; }

        public decimal Discount { get; set; }

        public decimal Tax { get; set; }

        public decimal Net { get; set; }

        public string Notes { get; set; }
        public List<UnitModelView> Units { get; set; }
    }
}