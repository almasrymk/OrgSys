using Utility;
using Entity.Model;
using Utility.Resource;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Entity.ModelView
{
    public class InvoiceModelView : BaseModel
    {
        public InvoiceModelView()
        {

        }

        public InvoiceModelView(Invoice ob)
        {
            if (ob == null)
                ob = new Invoice();
            this.Id = ob.Id;
            this.CodeNumber = ob.CodeNumber;
            this.Code = ob.Code;
            this.Date = ob.Date;
            this.DealerId = ob.DealerId;
            this.DealerName = ob.Dealer?.Name;           
            this.Hide = ob.Hide;            
            this.Notes = ob.Notes;
            this.PaymentTypeId = ob.PaymentTypeId;
            this.PaymentTypeName = ob.PaymentType?.Name;
            this.StoreId = ob.StoreId;
            this.StoreName = ob.Store?.Name;                           
            this.Status = ob.Status;
            this.ParentId = ob.ParentId;
            this.TypeId = ob.TypeId;
            this.ImgPath = ob.ImgPath;
            this.Remaining = ob.Remaining;
            this.Paid = ob.Paid;
            this.Total = ob.Total;
            this.Service = ob.Service;
            this.ServiceType = ob.ServiceType;
            this.Tax = ob.Tax;
            this.TaxType = ob.TaxType;
            this.Discount = ob.Discount;
            this.DiscountType = ob.DiscountType;
            this.Net = ob.Net;
            if (ob.InvoiceProducts == null)
                ob.InvoiceProducts = new List<InvoiceProduct>();
            this.InvoiceProducts = ob.InvoiceProducts.Select(e => new InvoiceProductModelView(e)).ToList();
        }

        public InvoiceModelView UpdateData(Order ob , long StoreId)
        {
            if (ob == null)
                ob = new Order();
            this.Date = ob.Date;           
            this.Hide = ob.Hide;
            this.Notes = ob.Notes;           
            this.Status = ob.Status;
            this.StoreId = StoreId;
            if (ob.DealerId != null)
                this.DealerId = ob.DealerId.Value;
            this.ParentId = ob.ParentId;
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
            this.InvoiceProducts = ob.OrderProducts.Select(e => new InvoiceProductModelView(e , StoreId)).ToList();
            return this;
        }
        public Invoice Model
        {
            get
            {
                return new Invoice
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
                    PaymentTypeId = this.PaymentTypeId,
                    StoreId = this.StoreId,
                    Status = this.Status,
                    MaskText = this.MaskText,
                    ParentId = this.ParentId,
                    TypeId = this.TypeId,
                    ImgPath = this.ImgPath,
                    Remaining = this.Remaining,
                    Paid = this.Paid,
                    Service=this.Service,
                    ServiceType=this.ServiceType,
                    Tax = this.Tax,
                    TaxType =this.TaxType,
                    Discount = this.Discount,
                    DiscountType =this.DiscountType,
                    InvoiceProducts = this.InvoiceProducts != null ? this.InvoiceProducts.Select(e => e.Model).ToList() : new List<InvoiceProduct>()
                };
            }
        }

        public long CodeNumber { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public long DealerId { get; set; }

        [Display(Name = nameof(Title_Designer.Client), ResourceType = typeof(Title_Designer))]
        public string DealerName { get; set; }

        [Required]
        public long PaymentTypeId { get; set; }

        public string PaymentTypeName { get; set; }

        [Required]       
        public long StoreId { get; set; }

        [Display(Name = nameof(Title_Designer.Store), ResourceType = typeof(Title_Designer))]
        public string StoreName { get; set; }      

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
        public decimal Remaining { get; set; }
        public decimal Paid { get; set; }

        public List<InvoiceProductModelView> InvoiceProducts { get; set; }
    }
}