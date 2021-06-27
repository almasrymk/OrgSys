using Utility;
using Entity.Model;
using Utility.Resource;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;

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
            this.Id = ob.Id;
            this.FinancialId = ob.FinancialId;
            this.RowNumber = ob.RowNumber;
            this.InvoiceId = ob.InvoiceId;
            this.InvoiceCode = ob.Invoice?.Code;
            this.Hide = ob.Hide;
            this.Amount = ob.Amount;           
            this.Status = ob.Status;
            this.ParentId = ob.ParentId;
            this.TypeId = ob.TypeId;
            this.ImgPath = ob.ImgPath;
        }

        public FinancialInvoice Model
        {
            get
            {
                return new FinancialInvoice
                {
                    Id = this.Id,
                    FinancialId = this.FinancialId,
                    InvoiceId = this.InvoiceId,
                    RowNumber = this.RowNumber,
                    Amount = this.Amount,
                    Hide = this.Hide,
                    Status = this.Status,
                    MaskText = this.MaskText,
                    ParentId = this.ParentId,
                    TypeId = this.TypeId,
                    ImgPath = this.ImgPath
                };
            }
        }

        public long RowNumber { get; set; }

        public long FinancialId { get; set; }
        public long InvoiceId { get; set; }

        public string InvoiceCode { get; set; }
        public decimal Amount { get; set; }
    }
}