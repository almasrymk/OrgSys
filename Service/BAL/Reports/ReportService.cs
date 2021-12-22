using System;
using Repository;
using X.PagedList;
using Entity.ModelReport;
using System.Collections.Generic;

namespace Service
{
    public class ReportService 
    {
        UnitOfWorkOrg repo;
        private string _Schema;
        public ReportService(string Schema)
        {
            this._Schema = Schema;
            repo = new UnitOfWorkOrg(Schema);
        }

        public IPagedList<InvoiceDetail> InvoiceDetail(long typeId, DateTime fromDate, DateTime toDate, long dealerId, long shiftId, long branchId, long userId, int page = 1, int pageSize = 100)
        {
            var obList = repo.reportRepo.InvoiceDetails(typeId, fromDate, toDate, dealerId, shiftId, branchId, userId).ToPagedList(page, pageSize);
            foreach (var ob in obList)
            {

            }
            return obList;
        }

        public IPagedList<InvoiceDetail> PurchesDetail(long typeId, DateTime fromDate, DateTime toDate, long dealerId, long shiftId, long branchId, long userId, int page = 1, int pageSize = 100)
        {
            var obList = repo.reportRepo.InvoiceDetails(typeId, fromDate, toDate, dealerId, shiftId, branchId, userId).ToPagedList(page, pageSize);
            foreach (var ob in obList)
            {

            }
            return obList;
        }

        public IPagedList<Customer> Customers(long typeId, long dealerId, int page = 1, int pageSize = 100)
        {
            var obList = repo.reportRepo.Customer(typeId, dealerId).ToPagedList(page, pageSize);
            foreach (var ob in obList)
            {

            }
            return obList;
        }

        public IPagedList<DealerInvoice> TotainvoiceCustomers(long typeId, DateTime fromDate, DateTime toDate, long dealerId, int page = 1, int pageSize = 100)
        {
            var obList = repo.reportRepo.TotalInvoicCustomer(typeId,
                                                             fromDate,
                                                             toDate,
                                                             dealerId).ToPagedList(page, pageSize);
            foreach (var ob in obList)
            {

            }
            return obList;
        }

        public IPagedList<SupplierSheetReport> SuplierSheetReportList(long typeId, DateTime fromDate, DateTime toDate, long dealerId, int typeinvoiceorpinvoice, int typeReturninvoiceorpinvoice, int page = 1, int pageSize = 100)
        {
            IPagedList<SupplierSheetReport> obList = repo.reportRepo.SuplierSheetReport(typeId,
                                                             fromDate,
                                                             toDate,
                                                             dealerId, typeinvoiceorpinvoice, typeReturninvoiceorpinvoice).ToPagedList(page, pageSize);
            List<SupplierSheetReport> list = new List<SupplierSheetReport>();
            SupplierSheetReport data = null;
            int LastDealerID = 0;
            decimal balens = 0;
            foreach (var ob in obList)
            {
                if (ob.InvoiceCode == -1)
                {

                    ob.BeginBalance = (decimal)(ob.Credit - ob.Debit);


                    ob.Credit = ob.BeginBalance < 0 ? ob.Credit : 0;

                    ob.Debit = ob.BeginBalance > 0 ? ob.Debit : 0;
                }

                if (LastDealerID != ob.DealarId)
                {
                    LastDealerID = ob.DealarId;

                    balens = 0;
                }
                balens = (decimal)(ob.BeginBalance + ob.Debit - ob.Credit + balens);
                data = new SupplierSheetReport()
                {
                    DealarCode = ob.DealarCode,
                    DealarName = ob.DealarName
                    ,
                    GetDateTime = ob.GetDateTime
                    ,
                    TypeInvoice = ob.TypeInvoice
                    ,
                    InvoiceCode = ob.InvoiceCode
                    ,
                    Credit = ob.Credit
                    ,
                    Debit = ob.Debit
                    ,
                    Balnce = balens
                };
                list.Add(data);
            }
            return list.ToPagedList(page, pageSize);
        }
    }
}