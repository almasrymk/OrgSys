using Entity;
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
        UnitOfWorkOrg repo;
        private string _Schema;
        public IntegrationServics(string Schema)
        {
            this._Schema = Schema;
            repo = new UnitOfWorkOrg(Schema);
        }

        public bool CreateInvoiceByOrder(Order order)
        {
            if (order != null && order.Id > 0)
            {
                var StockId = long.Parse("0" + new PreferenceService(_Schema).GetByKey("DefaultStock", "Invoice", 1, 0)?.Value);
                var inv = order.Map<InvoiceModelView>();
                inv.StockId = StockId;
                inv.StockId = long.Parse("0" + new PreferenceService(_Schema).GetByKey("DefaultStock", "Invoice", 1, 0)?.Value);
                if (order.DealerId == null || order.DealerId == 0)
                    inv.DealerId = long.Parse("0" + new PreferenceService(_Schema).GetByKey("DefaultCustomer", "Invoice", 1, 0)?.Value);
                else
                    inv.DealerId = order.DealerId.Value;
                inv.PaymentTypeId = long.Parse("0" + new PreferenceService(_Schema).GetByKey("DefaultPaymentType", "Invoice", 1, 0)?.Value);
                inv.StockId = StockId;
                inv.CurrencyId = long.Parse("0" + new PreferenceService(_Schema).GetByKey("DefaultCurrency", "Invoice", 1, 0)?.Value);

                var InvLod = repo.invoiceRepo.Get(e => e.Id == long.Parse("0" + order.InvoiceId));
                if (InvLod != null)
                {
                    inv.CodeNumber = InvLod.CodeNumber;
                    inv.Code = "" + InvLod.CodeNumber;
                }
                else
                {
                    inv.CodeNumber = new InvoiceService(_Schema).GetMaxCode(1);
                    inv.Code = "" + inv.CodeNumber;
                }

                var res = new InvoiceService(_Schema).Save(inv);
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
            var transaction = inv.Map<TransactionModelView>();
            transaction.TypeId = inv.TypeId == 2 || inv.TypeId == 3 ? 1 : 2;
            var transactionOld = repo.transactionRepo.Get(e => e.Id == inv.TransactionId);
            if (transactionOld == null || transactionOld.Id == 0)
            {
                transaction.Id = 0;
                transaction.CodeNumber = repo.transactionRepo.GetMaXCode(e => e.TypeId == (inv.TypeId == 2 || inv.TypeId == 3 ? 1 : 2));
                transaction.Code = "" + transaction.CodeNumber;
            }
            else
            {
                transaction.Id = transactionOld.Id;
                transaction.CodeNumber = transactionOld.CodeNumber;
                transaction.Code = "" + transactionOld.Code;
            }

            transaction.Stock = null;
            transaction.Dealer = null;
            foreach (var item in transaction.TransactionProductList)
            {
                item.Id = 0;
                item.Product = null;
                item.Unit = null;
            }

            transaction = new TransactionService(_Schema).Save(transaction);
            inv.TransactionId = transaction.Id;
            repo.invoiceRepo.AddOrUpdate(inv.Map<Invoice>());
            return true;
        }

        public bool CreateReceived(Transaction transaction)
        {
            if (transaction != null && transaction.Id > 0)
            {
                var ob = repo.transactionRepo.Get(e => e.ParentId == transaction.Id);
                if (ob != null)
                    repo.transactionRepo.Delete(ob.Id);

                var received = transaction.Map<TransactionModelView>();
                received.Id = 0;
                received.TypeId = 4;
                received.ParentId = transaction.Id;
                received.StockId = transaction.ToStockId;
                received.ToStockId = null;

                foreach (var item in received.TransactionProducts)
                {
                    item.Id = 0;
                    item.TypeId = 4;
                }
                if (ob.Id == 0 || ob == null)
                    received.CodeNumber = repo.transactionRepo.GetMaXCode(e => e.TypeId == 4);
                else
                    received.CodeNumber = ob.CodeNumber;
                received.Code = "" + received.CodeNumber;
                repo.transactionRepo.AddOrUpdate(received);
                return true;
            }
            return false;
        }

        public bool CreateFinancialByInvoice(InvoiceModelView inv)
        {
            var TotalCredit = repo.financialInvoiceRepo.GetList(e => e.InvoiceId == inv.Id && e.Financial.Status != Utility.Status.Deleted && e.Financial.Status != Utility.Status.Cancel, null, "Financial", Utility.Status.All)?.Sum(e => e.Amount) ?? 0;
            if (inv.Paid > TotalCredit)
            {
                var amount = inv.Paid - inv.Remaining - TotalCredit;
                var financial = inv.Map<FinancialModelView>();
                financial.Id = 0;
                financial.Dealer = null;
                financial.Amount = amount;
                financial.Rate = inv.Rate;
                financial.AmountByDefaultCurrency = inv.Credit * financial.Rate;
                financial.CreateDate = DateTime.Now;
                financial.TypeId = inv.TypeId == 1 || inv.TypeId == 4 ? 1 : 2;
                financial.SafeId = long.Parse("0" + new PreferenceService(_Schema).GetByKey("DefaultSafe", "Financial", (inv.TypeId == 1 || inv.TypeId == 4 ? 1 : 2), 0)?.Value);
                inv.CodeNumber = repo.financialRepo.GetMaXCode(e => e.TypeId == (inv.TypeId == 1 || inv.TypeId == 4 ? 1 : 2));
                inv.Code = "" + inv.CodeNumber;
                FinancialInvoiceModelView financialInvoiceModelView = inv.Map<FinancialInvoiceModelView>();
                financialInvoiceModelView.Id = 0;
                financialInvoiceModelView.TypeId = inv.TypeId == 1 || inv.TypeId == 4 ? 1 : 2;
                financialInvoiceModelView.Amount = amount;
                financial.FinancialInvoiceList = new List<FinancialInvoiceModelView>();
                financial.FinancialInvoiceList.Add(financialInvoiceModelView);
                new FinancialService(_Schema).Save(financial);
            }
            return true;
        }

        public bool CollectPaidInvoice(InvoiceModelView inv)
        {
            if (inv.Credit > 0)
            {
                var financial = inv.Map<FinancialModelView>();
                financial.Id = 0;
                financial.Dealer = null;
                financial.Amount = inv.Credit;
                financial.Rate = inv.Rate ;
                financial.AmountByDefaultCurrency = inv.Credit * financial.Rate;
                financial.CreateDate = DateTime.Now;
                financial.TypeId = inv.TypeId == 1 || inv.TypeId == 4 ? 1 : 2;
                financial.SafeId = long.Parse("0" + new PreferenceService(_Schema).GetByKey("DefaultSafe", "Financial", (inv.TypeId == 1 || inv.TypeId == 4 ? 1 : 2), 0)?.Value);
                inv.CodeNumber = repo.financialRepo.GetMaXCode(e => e.TypeId == (inv.TypeId == 1 || inv.TypeId == 4 ? 1 : 2));
                inv.Code = "" + inv.CodeNumber;
                FinancialInvoiceModelView financialInvoiceModelView = inv.Map<FinancialInvoiceModelView>();
                financialInvoiceModelView.Id = 0;
                financialInvoiceModelView.TypeId = inv.TypeId == 1 || inv.TypeId == 4 ? 1 : 2;
                financialInvoiceModelView.Amount = inv.Credit;
                financial.FinancialInvoiceList = new List<FinancialInvoiceModelView>();
                financial.FinancialInvoiceList.Add(financialInvoiceModelView);
                new FinancialService(_Schema).Save(financial);
            }
            return true;
        }

        public void UpdateCredit(List<long> ids)
        {
            var invs = repo.invoiceRepo.GetList(e => ids.Contains(e.Id), null, "", Utility.Status.New).ToList();
            if (invs == null)
                invs = new List<Invoice>();

            var financialsInvs = repo.financialInvoiceRepo.GetList(e => ids.Contains(e.InvoiceId ?? 0) && e.Financial.Status == Utility.Status.All, null, "Financial", Utility.Status.New).ToList();
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