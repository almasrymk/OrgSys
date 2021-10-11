using Entity.Model;
using Entity.ModelView;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service
{
    public class IntegrationServics
    {
        string Includes = "";
        UnitOfWork repo;
        public IntegrationServics()
        {
            repo = new UnitOfWork();
        }

        public bool CreateInvoiceByOrder(Order order)
        {
            if (order != null && order.Id > 0)
            {
                var StoreId = long.Parse("0" + new PreferenceService().GetByKey("DefaultStore", "Invoice", 1, 0)?.Value);
                var inv = new InvoiceModelView(order, StoreId);
                inv.StoreId = long.Parse("0" + new PreferenceService().GetByKey("DefaultStore", "Invoice", 1, 0)?.Value);
                if (order.DealerId == null || order.DealerId == 0)
                    inv.DealerId = long.Parse("0" + new PreferenceService().GetByKey("DefaultCustomer", "Invoice", 1, 0)?.Value);
                else
                    inv.DealerId = order.DealerId.Value;
                inv.PaymentTypeId = long.Parse("0" + new PreferenceService().GetByKey("DefaultPaymentType", "Invoice", 1, 0)?.Value);
                inv.StoreId = StoreId;
                inv.CurrencyId = long.Parse("0" + new PreferenceService().GetByKey("DefaultCurrency", "Invoice", 1, 0)?.Value);

                var InvLod = repo.invoiceRepo.Get(e => e.Id == long.Parse("0" + order.InvoiceId));
                if (InvLod != null)
                {
                    inv.CodeNumber = InvLod.CodeNumber;
                    inv.Code = "" + InvLod.CodeNumber;
                }
                else
                {
                    inv.CodeNumber = new InvoiceService().GetMaxCode(1);
                    inv.Code = "" + inv.CodeNumber;
                }

                var res = new InvoiceService().Save(inv);
                order.InvoiceId = res.Id;
                order.CloseTable = true;
                repo.orderRepo.AddOrUpdate(order);
                return true;
            }
            return false;
        }

        public bool DeleteInvoice(long Id)
        {
            if (repo.orderRepo.Any(x => x.InvoiceId == Id))
            {
                var rps = repo.orderRepo.GetList(e => e.InvoiceId == Id, e => e.OrderBy(e => e.Id), "", Utility.Status.All);
                foreach (var rp in rps)
                {
                    rp.InvoiceId = null;
                    repo.orderRepo.AddOrUpdateTemp(rp);                    
                }
                return repo.orderRepo.SaveChanges();
            }

            return false;
        }

        public bool CreateTransactionByInvoice(InvoiceModelView inv)
        {
            if (inv != null && inv.Id > 0 && inv.Credit > 0)
            {


                //Transaction transaction = repo.transactionRepo.Get(e => e.ParentId == inv.Id && e.TypeId == 1);
                //if (transaction == null)
                //    transaction = new Transaction();

                //transaction.

                //var financial = new FinancialModelView(inv.Model);
                //financial.SafeId = long.Parse("0" + new PreferenceService().GetByKey("DefaultSafe", "Financial", (inv.TypeId == 1 || inv.TypeId == 4 ? 1 : 2), 0)?.Value);
                //financial.PaymentTypeId = long.Parse("0" + new PreferenceService().GetByKey("DefaultPaymentType", "Financial", (inv.TypeId == 1 || inv.TypeId == 4 ? 1 : 2), 0)?.Value);
                //inv.CodeNumber = GetMaxCode(inv.TypeId == 1 || inv.TypeId == 4 ? 1 : 2);
                //inv.Code = "" + inv.CodeNumber;



                //var transaction1 = new TransactionModelView( repo.transactionRepo.Get(e => e.ParentId == inv.Id, Includes));
                var transaction = new TransactionModelView(inv.Model);

                //= new TransactionModelView(inv.Model);
                //inv.CodeNumber = GetMaxCode(inv.TypeId == 2 || inv.TypeId == 3 ? 1 : 2);
                //inv.Code = "" + inv.CodeNumber;

                new TransactionService().Save(transaction);
                //repo.transactionRepo.AddOrUpdate(transaction.Model);
                //Save(transaction);
                return true;
            }
            return false;
        }

    }



}
