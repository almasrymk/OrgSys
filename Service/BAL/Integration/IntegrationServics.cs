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
            var transaction  = new TransactionModelView(inv.Model);
            var transactionOld = repo.transactionRepo.Get(e => e.Id == inv.TransactionId);
            if (transactionOld == null || transactionOld.Id == 0)
            {
                transaction.CodeNumber = repo.transactionRepo.GetMaXCode(e => e.TypeId == (inv.TypeId == 2 || inv.TypeId == 3 ? 1 : 2));
                transaction.Code = "" + transaction.CodeNumber;
            }
            else
            {
                transaction.Id = transactionOld.Id;
                transaction.CodeNumber = transactionOld.CodeNumber;
                transaction.Code = "" + transactionOld.Code;
            }            
           
            transaction = new TransactionService().Save(transaction);
            inv.TransactionId = transaction.Id;
            repo.invoiceRepo.AddOrUpdate(inv.Model);
            return true;
        }

        public bool CreateReceived(Transaction transaction)
        {
            if (transaction != null && transaction.Id > 0)
            {
                var received = new TransactionModelView(transaction);
                received.TypeId = 4;
                foreach (var item in received.TransactionProducts)
                    item.TypeId = 4;
               // received.CodeNumber = GetMaxCode(4);
                received.Code = "" + received.CodeNumber;
                //Save(received);
                return true;
            }
            return false;
        }

        public bool CreateFinancialByInvoice(InvoiceModelView inv)
        {
            var TotalCredit = repo.financialInvoiceRepo.GetList(e => e.InvoiceId == inv.Id && e.Financial.Status !=  Utility.Status.Deleted && e.Financial.Status != Utility.Status.Cancel, null , "Financial", Utility.Status.All)?.Sum(e=>e.Amount)??0;
            if(inv.Paid > TotalCredit)
            {
                var amount = inv.Paid - inv.Remaining - TotalCredit;
                var financial = new FinancialModelView(inv.Model , amount);
                financial.SafeId = long.Parse("0" + new PreferenceService().GetByKey("DefaultSafe", "Financial", (inv.TypeId == 1 || inv.TypeId == 4 ? 1 : 2), 0)?.Value);
                inv.CodeNumber = repo.financialRepo.GetMaXCode(e => e.TypeId == (inv.TypeId == 1 || inv.TypeId == 4 ? 1 : 2));
                inv.Code = "" + inv.CodeNumber;
                new FinancialService().Save(financial);
            }
            return true;
        }

        public bool CollectPaidInvoice(InvoiceModelView inv)
        {
            if (inv.Credit > 0)
            {
                var financial = new FinancialModelView(inv.Model, inv.Credit);
                financial.SafeId = long.Parse("0" + new PreferenceService().GetByKey("DefaultSafe", "Financial", (inv.TypeId == 1 || inv.TypeId == 4 ? 1 : 2), 0)?.Value);
                inv.CodeNumber = repo.financialRepo.GetMaXCode(e => e.TypeId == (inv.TypeId == 1 || inv.TypeId == 4 ? 1 : 2));
                inv.Code = "" + inv.CodeNumber;
                new FinancialService().Save(financial);
            }
            return true;
        }

        public void UpdateCredit(List<long> ids)
        {
            var invs = repo.invoiceRepo.GetList(e => ids.Contains(e.Id), null, "", Utility.Status.New).ToList();
            if (invs == null)
                invs = new List<Invoice>();

            var financialsInvs = repo.financialInvoiceRepo.GetList(e => ids.Contains(e.InvoiceId), null, "", Utility.Status.New).ToList();
            if (financialsInvs == null)
                financialsInvs = new List<FinancialInvoice>();

            foreach (var inv in invs)
            {
                var amount = financialsInvs.Where(e => e.InvoiceId == inv.Id)?.Sum(e => e.Amount) ?? 0;
                inv.Credit = inv.Net - amount;
                inv.Paid = inv.Net - inv.Credit;
                var Nwob = repo.invoiceRepo.AddOrUpdate(inv);
            }
        }
    }
}