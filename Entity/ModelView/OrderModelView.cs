using Utility;
using Entity.Model;
using Utility.Resource;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Entity.ModelView
{
    public class OrderModelView : BaseModel
    {
        public OrderModelView()
        {

        }

        public OrderModelView(Order ob)
        {
            if (ob == null)
                ob = new Order();
            this.Id = ob.Id;
            this.CodeNumber = ob.CodeNumber;
            this.Code = ob.Code;
            this.Date = ob.Date;
            this.DealerId = ob.DealerId;
            this.DealerName = ob.Dealer?.Name;
            this.Hide = ob.Hide;            
            this.Notes = ob.Notes;
            this.TableId = ob.TableId;
            this.TableName = ob.Table?.Name;
            this.CloseTable = ob.CloseTable;
            this.Status = ob.Status;
            this.ParentId = ob.ParentId;
            this.TypeId = ob.TypeId;
            this.ImgPath = ob.ImgPath;
            this.Total = ob.Total;
            this.Service = ob.Service;
            this.ServiceType = ob.ServiceType;
            this.Tax = ob.Tax;
            this.TaxType = ob.TaxType;
            this.Discount = ob.Discount;
            this.DiscountType = ob.DiscountType;
            this.Net = ob.Net;
            if (ob.OrderProducts == null)
                ob.OrderProducts = new List<OrderProduct>();
            this.OrderProducts = ob.OrderProducts.Select(e => new OrderProductModelView(e)).ToList();
        }

        public Order Model
        {
            get
            {
                return new Order
                {
                    Id = this.Id,
                    CodeNumber = this.CodeNumber,
                    Code = this.Code,
                    Date = this.Date,
                    DealerId = this.DealerId,                   
                    Total = this.Total,                   
                    Hide = this.Hide,
                    Net  = this.Net,
                    Notes = this.Notes,
                    TableId = this.TableId,
                    CloseTable = this.CloseTable,
                    Status = this.Status,
                    MaskText = this.MaskText,
                    ParentId = this.ParentId,
                    TypeId = this.TypeId,
                    ImgPath = this.ImgPath,
                    Service = this.Service,
                    ServiceType = this.ServiceType,
                    Tax = this.Tax,
                    TaxType = this.TaxType,
                    Discount = this.Discount,
                    DiscountType = this.DiscountType,
                    OrderProducts = this.OrderProducts != null ? this.OrderProducts.Select(e => e.Model).ToList() : new List<OrderProduct>()
                };
            }
        }

        public long CodeNumber { get; set; }

        [Required]
        public string Code { get; set; }       
        public long? TableId { get; set; }
        public string TableName { get; set; }
        public bool CloseTable { get; set; }
        [Required]
        public DateTime Date { get; set; }

        [Required]
        public long DealerId { get; set; }

        public string DealerName { get; set; }

        public decimal Total { get; set; }

        public decimal Discount { get; set; }
        public int DiscountType { get; set; }
        public decimal Tax { get; set; }
        public int TaxType { get; set; }
        public decimal Service { get; set; }
        public int ServiceType { get; set; }

        public decimal Net { get; set; }

        [StringLength(500)]
        public string Notes { get; set; }
        public List<OrderProductModelView> OrderProducts { get; set; }
    }
}