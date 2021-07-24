using Entity.Model;
using System.Collections.Generic;
using System.Linq;

namespace Entity.ModelView
{
    public class FinancialModelView : MovementModel
    {
        public FinancialModelView()
        {

        }

        public FinancialModelView(Financial ob)
        {
            if (ob == null)
                ob = new Financial();

            this.Date = ob.Date;

            this.DealerId = ob.DealerId;

            this.DealerName = ob.Dealer?.Name;

            this.PaymentTypeId = ob.PaymentTypeId;

            this.PaymentTypeName = ob.PaymentType?.Name;

            this.CurrencyId = ob.CurrencyId;

            this.CurrencyName = ob.Currency?.Name;

            this.OutlayId = ob.OutlayId;

            this.OutlayName = ob.Outlay?.Name;

            this.Notes = ob.Notes;

            this.Amount = ob.Amount;

            this.SafeId = ob.SafeId;

            this.SafeName = ob.Safe?.Name;

            this.Rate = ob.Rate > 0 ? ob.Rate : (ob.Currency?.Rate??0);

            this.AmountByDefaultCurrency = ob.AmountByDefaultCurrency;

            this.Id = ob.Id;

            this.CodeNumber = ob.CodeNumber;

            this.Code = ob.Code;

            this.MaskText = ob.MaskText;

            this.ParentId = ob.ParentId;

            this.TypeId = ob.TypeId;

            this.Hide = ob.Hide;

            this.ImgPath = ob.ImgPath;

            this.Status = ob.Status;

            if (ob.FinancialInvoices == null)
                ob.FinancialInvoices = new List<FinancialInvoice>();

            this.FinancialInvoices = ob.FinancialInvoices.Select(e => new FinancialInvoiceModelView(e)).ToList();
        }

        public FinancialModelView(Invoice ob)
        {
            if (ob == null)
                ob = new Invoice();

            this.Date = ob.Date;

            this.DealerId = ob.DealerId;

            this.CurrencyId = ob.CurrencyId;

            this.Notes = ob.Notes;

            this.Amount = ob.Credit;          

            this.Rate = ob.Rate > 0 ? ob.Rate : (ob.Currency?.Rate ?? 0);

            this.AmountByDefaultCurrency = ob.Credit * this.Rate;

            this.Id = ob.Id;

            this.CodeNumber = ob.CodeNumber;

            this.Code = ob.Code;

            this.MaskText = ob.MaskText;

            this.ParentId = ob.ParentId;

            this.TypeId = ob.TypeId == 1 ||  ob.TypeId == 4 ? 1 : 2;

            this.Hide = ob.Hide;

            this.ImgPath = ob.ImgPath;

            this.Status = ob.Status;

            this.FinancialInvoices.Add(new FinancialInvoiceModelView(ob));
        }

        public Financial Model
        {
            get
            {
                return new Financial
                {
                    Date = this.Date,
                    DealerId = this.DealerId,
                    PaymentTypeId = this.PaymentTypeId,
                    CurrencyId = this.CurrencyId,
                    OutlayId = this.OutlayId,
                    Rate = this.Rate,
                    Amount = this.Amount,
                    AmountByDefaultCurrency = this.AmountByDefaultCurrency,
                    Notes = this.Notes,
                    SafeId = this.SafeId,
                    Id = this.Id,
                    CodeNumber = this.CodeNumber,
                    Code = this.Code,
                    MaskText = this.MaskText,
                    ParentId = this.ParentId,
                    TypeId = this.TypeId,
                    Hide = this.Hide,
                    Status = this.Status,
                    ImgPath = this.ImgPath,
                    FinancialInvoices = this.FinancialInvoices != null ? this.FinancialInvoices.Select(e => e.Model).ToList() : new List<FinancialInvoice>()
                };
            }
        }
      
        public long? DealerId { get; set; }

        public string DealerName { get; set; }
        
        public long PaymentTypeId { get; set; }

        public string PaymentTypeName { get; set; }

        public long? OutlayId { get; set; }

        public string OutlayName { get; set; }

        public long SafeId { get; set; }

        public string SafeName { get; set; }

        public decimal Amount { get; set; }

        public string Notes { get; set; }
 
        public long CurrencyId { get; set; }

        public string CurrencyName { get; set; }

        public decimal Rate { get; set; }

        public decimal AmountByDefaultCurrency { get; set; }

        public List<FinancialInvoiceModelView> FinancialInvoices { get; set; }
    }
}