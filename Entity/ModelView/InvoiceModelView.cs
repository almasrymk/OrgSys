using Utility;
using Entity.Model;
using Utility.Resource;
using System.ComponentModel.DataAnnotations;
using System;

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
            this.Discount = ob.Discount;
            this.Hide = ob.Hide;
            this.Net = ob.Net;
            this.Notes = ob.Notes;
            this.OrderId = ob.OrderId;
            this.PaymentTypeId = ob.PaymentTypeId;
            this.PaymentTypeName = ob.PaymentType?.Name;
            this.StoreId = ob.StoreId;
            this.StoreName = ob.Store?.Name;
            this.Tax = ob.Tax;
            this.Total = ob.Total;           
            this.Status = ob.Status;
            this.ParentId = ob.ParentId;
            this.TypeId = ob.TypeId;
            this.ImgPath = ob.ImgPath;
            this.Remaining = ob.Remaining;
            this.Paid = ob.Paid;
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
                    Discount = this.Discount,
                    Total = this.Total,
                    Tax = this.Tax,
                    Hide = this.Hide,
                    Net  = this.Net,
                    Notes = this.Notes,
                    OrderId = this.OrderId,
                    PaymentTypeId = this.PaymentTypeId,
                    StoreId = this.StoreId,
                    Status = this.Status,
                    MaskText = this.MaskText,
                    ParentId = this.ParentId,
                    TypeId = this.TypeId,
                    ImgPath = this.ImgPath,
                    Remaining = this.Remaining,
                    Paid = this.Paid
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

        public string DealerName { get; set; }

        [Required]
        public long PaymentTypeId { get; set; }

        public string PaymentTypeName { get; set; }

        [Required]
        public long StoreId { get; set; }

        public string StoreName { get; set; }

        public long? OrderId { get; set; }       

        public decimal Total { get; set; }

        public decimal Discount { get; set; }

        public decimal Tax { get; set; }

        public decimal Net { get; set; }

        [StringLength(500)]
        public string Notes { get; set; }
        public decimal Remaining { get; set; }
        public decimal Paid { get; set; }
    }
}