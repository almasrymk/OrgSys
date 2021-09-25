using Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Entity.ModelView
{
    public class InvoiceModelView : MovementModel
    {
        public InvoiceModelView()
        {

        }

        public InvoiceModelView(Invoice ob)
        {
            if (ob == null)
                ob = new Invoice();

            this.DealerId = ob.DealerId;

            this.DealerName = ob.Dealer?.Name;

            this.Notes = ob.Notes;

            this.PaymentTypeId = ob.PaymentTypeId;

            this.PaymentTypeName = ob.PaymentType?.Name;

            this.CurrencyId = ob.CurrencyId;

            this.CurrencyName = ob.Currency?.Name;

            this.TransactionId = ob.TransactionId;

            this.Transaction = new TransactionModelView(ob.Transaction);

            this.Rate = ob.Rate;

            this.StoreId = ob.StoreId;

            this.StoreName = ob.Store?.Name;

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

            this.Credit = ob.Credit;

            this.CreditByDefaultCurrency = ob.CreditByDefaultCurrency;

            this.NetByDefaultCurrency = ob.NetByDefaultCurrency;

            this.BranchId = ob.BranchId;

            this.BranchName = ob.Branch?.Name;

            this.CreateUserId = ob.CreateUserId;

            this.CreateUserName = ob.CreateUser?.Name;

            this.ModifyUserId = ob.ModifyUserId;

            this.ModifyUserName = ob.ModifyUser?.Name;

            this.ShiftId = ob.ShiftId;

            this.ShiftName = ob.Shift?.Name;

            this.Date =  ob.Date;

            this.CreateDate =  ob.CreateDate;

            this.ModifyDate =  ob.ModifyDate;

            this.Id = ob.Id;

            this.CodeNumber = ob.CodeNumber;

            this.Code = ob.Code;

            this.MaskText = ob.MaskText;

            this.ParentId = ob.ParentId;

            this.TypeId = ob.TypeId;

            this.Hide = ob.Hide;

            this.ImgPath = ob.ImgPath;

            this.Status = ob.Status;

            if (ob.InvoiceProducts == null)
                ob.InvoiceProducts = new List<InvoiceProduct>();

            this.InvoiceProducts = ob.InvoiceProducts.Select(e => new InvoiceProductModelView(e)).ToList();
        }

        public InvoiceModelView(Order ob , long StoreId)
        {
            if (ob == null)
                ob = new Order();

            this.Notes = ob.Notes;

            if (ob.DealerId != null)
                this.DealerId = ob.DealerId.Value;

            if (ob.InvoiceId != null)
                this.Id = ob.InvoiceId ?? 0;

            this.StoreId = StoreId;

            this.Total = ob.Total;

            this.Service = ob.Service;

            this.ServiceType = ob.ServiceType;

            this.Tax = ob.Tax;

            this.TaxType = ob.TaxType;

            this.Discount = ob.Discount;

            this.DiscountType = ob.DiscountType;

            this.Net = ob.Net;

            this.BranchId = ob.BranchId;

            this.CreateUserId = ob.CreateUserId;

            this.ModifyUserId = ob.ModifyUserId;

            this.ShiftId = ob.ShiftId;

            this.Date = ob.Date;

            this.CreateDate = ob.CreateDate;

            this.ModifyDate = ob.ModifyDate;
         
            this.MaskText = ob.MaskText;

            this.ParentId = ob.ParentId;

            this.TypeId = 1;

            this.Hide = ob.Hide;

            this.ImgPath = ob.ImgPath;

            this.Status = ob.Status;

            if (ob.OrderProducts == null)
                ob.OrderProducts = new List<OrderProduct>();

            this.InvoiceProducts = ob.OrderProducts.Select(e => new InvoiceProductModelView(e , StoreId)).ToList();
        }

        public Invoice Model
        {
            get
            {
                return new Invoice
                {
                    DealerId = this.DealerId,
                    Total = this.Total,
                    Net = this.Net,
                    Rate = this.Rate,
                    NetByDefaultCurrency = this.NetByDefaultCurrency,
                    Notes = this.Notes,
                    PaymentTypeId = this.PaymentTypeId,
                    CurrencyId = this.CurrencyId,
                    TransactionId = this.TransactionId,
                    StoreId = this.StoreId,
                    Remaining = this.Remaining,
                    Paid = this.Paid,
                    Service = this.Service,
                    ServiceType = this.ServiceType,
                    Tax = this.Tax,
                    TaxType = this.TaxType,
                    Discount = this.Discount,
                    DiscountType = this.DiscountType,
                    Credit = this.Credit,
                    CreditByDefaultCurrency = this.CreditByDefaultCurrency,
                    BranchId = this.BranchId,
                    CreateUserId = this.CreateUserId,
                    ModifyUserId = this.ModifyUserId,
                    ShiftId = this.ShiftId,
                    Date = this.Date,
                    CreateDate = this.CreateDate,
                    ModifyDate = this.ModifyDate,
                    Id = this.Id,
                    CodeNumber = this.CodeNumber,
                    Code = this.Code,
                    MaskText = this.MaskText,
                    ParentId = this.ParentId,
                    TypeId = this.TypeId,
                    Hide = this.Hide,
                    Status = this.Status,
                    ImgPath = this.ImgPath,
                    InvoiceProducts = this.InvoiceProducts != null ? this.InvoiceProducts.Select(e => e.Model).ToList() : new List<InvoiceProduct>()
                };
            }
        }
      
        public long DealerId { get; set; }
     
        public string DealerName { get; set; }
       
        public long PaymentTypeId { get; set; }

        public string PaymentTypeName { get; set; }

        public long StoreId { get; set; }

        public string StoreName { get; set; }
     
        public long? TransactionId { get; set; }

        public decimal Total { get; set; }

        public decimal Discount { get; set; }

        public int DiscountType { get; set; }

        public decimal Tax { get; set; }

        public int TaxType { get; set; }

        public decimal Service { get; set; }

        public int ServiceType { get; set; }

        public decimal Net { get; set; }

        public string Notes { get; set; }

        public decimal Remaining { get; set; }

        public decimal Paid { get; set; }

        public string ParentCode { get; set; }

        public long CurrencyId { get; set; }

        public string CurrencyName { get; set; }

        public decimal Rate { get; set; }

        public decimal NetByDefaultCurrency { get; set; }

        public decimal Credit { get; set; }

        public decimal CreditByDefaultCurrency { get; set; }

        public string BranchName { get; set; }

        public string CreateUserName { get; set; }

        public string ModifyUserName { get; set; }

        public string ShiftName { get; set; }

        public TransactionModelView Transaction { get; set; }

        public List<InvoiceProductModelView> InvoiceProducts { get; set; }
    }
}