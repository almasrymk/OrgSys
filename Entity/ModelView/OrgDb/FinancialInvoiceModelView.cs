using Entity.Model;
using System;

namespace Entity.ModelView
{
    public class FinancialInvoiceModelView : BaseModel
    {
        public FinancialInvoiceModelView()
        {

        }

        public FinancialInvoiceModelView(FinancialInvoice ob)
        {
            if (ob == null)
                ob = new FinancialInvoice();

            this.FinancialId = ob.FinancialId;

            this.RowNumber = ob.RowNumber;

            this.InvoiceId = ob.InvoiceId;

            this.Invoice = ob.Invoice;

            this.Net = ob.Invoice?.Net;

            this.Amount = ob.Amount;

            this.Id = ob.Id;

            this.CodeNumber = ob.CodeNumber;

            this.Code = ob.Code;

            this.MaskText = ob.MaskText;

            this.ParentId = ob.ParentId;

            this.TypeId = ob.TypeId;

            this.Hide = ob.Hide;

            this.ImgPath = ob.ImgPath;

            this.Status = ob.Status;
        }

        public FinancialInvoiceModelView(Invoice ob, decimal amount)
        {
            if (ob == null)
                ob = new Invoice();

            this.RowNumber = 1;

            this.InvoiceId = ob.Id;

            this.Net = amount;

            this.Amount = amount;

            this.CodeNumber = 1;

            this.Code = "1";

            this.MaskText = ob.MaskText;

            this.ParentId = ob.ParentId;

            this.TypeId = ob.TypeId == 1 || ob.TypeId == 4 ? 1 : 2;

            this.Hide = ob.Hide;

            this.ImgPath = ob.ImgPath;

            this.Status = ob.Status;
        }

        public FinancialInvoice Model()
        {
            return new FinancialInvoice
            {
                FinancialId = this.FinancialId,
                InvoiceId = this.InvoiceId,
                RowNumber = this.RowNumber,
                Amount = this.Amount,
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

        public long FinancialId { get; set; }

        public long InvoiceId { get; set; }

        public Invoice Invoice { get; set; }

        public decimal? Net { get; set; }

        public decimal Amount { get; set; }
    }
}