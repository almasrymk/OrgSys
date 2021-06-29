using Utility;
using Entity.Model;
using Utility.Resource;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Entity.ModelView
{
    public class FinancialModelView : BaseModel
    {
        public FinancialModelView()
        {

        }

        public FinancialModelView(Financial ob)
        {
            if (ob == null)
                ob = new Financial();
            this.Id = ob.Id;
            this.CodeNumber = ob.CodeNumber;
            this.Code = ob.Code;
            this.Date = ob.Date;
            this.DealerId = ob.DealerId;
            this.DealerName = ob.Dealer?.Name;
            this.PaymentTypeId = ob.PaymentTypeId;
            this.PaymentTypeName = ob.PaymentType?.Name;
            this.CurrencyId = ob.CurrencyId;
            this.CurrencyName = ob.Currency?.Name;
            this.OutlayId = ob.OutlayId;
            this.OutlayName = ob.Outlay?.Name;
            this.Hide = ob.Hide;
            this.Notes = ob.Notes;
            this.Amount = ob.Amount;
            this.SafeId = ob.SafeId;
            this.SafeName = ob.Safe?.Name;
            this.Status = ob.Status;
            this.ParentId = ob.ParentId;
            this.Rate = ob.Rate > 0 ? ob.Rate : (ob.Currency?.Rate??0);
            this.AmountByDefaultCurrency = ob.AmountByDefaultCurrency;
            this.TypeId = ob.TypeId;
            this.ImgPath = ob.ImgPath;          
            if (ob.FinancialInvoices == null)
                ob.FinancialInvoices = new List<FinancialInvoice>();
            this.FinancialInvoices = ob.FinancialInvoices.Select(e => new FinancialInvoiceModelView(e)).ToList();
        }
       

        public Financial Model
        {
            get
            {
                return new Financial
                {
                    Id = this.Id,
                    CodeNumber = this.CodeNumber,
                    Code = this.Code,
                    Date = this.Date,
                    DealerId = this.DealerId,
                    PaymentTypeId = this.PaymentTypeId,
                    CurrencyId = this.CurrencyId,
                    OutlayId = this.OutlayId,
                    Rate = this.Rate,
                    Amount = this.Amount,
                    AmountByDefaultCurrency = this.AmountByDefaultCurrency,
                    Hide = this.Hide,
                    Notes = this.Notes,
                    SafeId = this.SafeId,
                    Status = this.Status,
                    MaskText = this.MaskText,
                    ParentId = this.ParentId,
                    TypeId = this.TypeId,
                    ImgPath = this.ImgPath,
                    FinancialInvoices = this.FinancialInvoices != null ? this.FinancialInvoices.Select(e => e.Model).ToList() : new List<FinancialInvoice>()
                };
            }
        }

        public long CodeNumber { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public long? DealerId { get; set; }

        [Display(Name = nameof(Title_Designer.Client), ResourceType = typeof(Title_Designer))]
        public string DealerName { get; set; }
        [Required]
        public long PaymentTypeId { get; set; }

        public string PaymentTypeName { get; set; }
        public long? OutlayId { get; set; }

        public string OutlayName { get; set; }

        public long SafeId { get; set; }

        public string SafeName { get; set; }

        public decimal Amount { get; set; }

        [StringLength(500)]
        public string Notes { get; set; }
 
        public long CurrencyId { get; set; }
        public string CurrencyName { get; set; }
        public decimal Rate { get; set; }
        public decimal AmountByDefaultCurrency { get; set; }
        public List<FinancialInvoiceModelView> FinancialInvoices { get; set; }
    }
}