using Entity;
using System.Linq;
using X.PagedList;
using Entity.Model;
using Entity.ModelView;
using System.Collections.Generic;

namespace Service
{
    public class TransactionService : BaseOrgService<TransactionModelView, Transaction>
    {
        public TransactionService(string Schema) : base(Schema, "Dealer,Store,ToStore,TransactionProducts,TransactionProducts.Product,TransactionProducts.Product.ProductUnits,TransactionProducts.Product.ProductUnits.Unit") { }

        #region Save / Delete
        public override TransactionModelView Save(TransactionModelView ob)
        {
            if (ob.Id > 0)
            {
                ob.ParentId = Get(ob.Id).ParentId;
            }

            // Save
            var Nwob = repo.AddOrUpdate(ob.Map<Transaction>());

            if (ob.Id > 0)
            {
                var ids = ob.TransactionProductList.Select(e => e.Id).ToList();
                if (ids == null) ids = new List<long>();

                // Delete row from database
                var deleted = repoAll.transactionProductRepo.GetList(e => e.TransactionId == ob.Id && !ids.Contains(e.Id), e => e.OrderBy(e => e.Id), "", Utility.Status.All).ToList();
                if (deleted != null && deleted.Count > 0)
                    repoAll.transactionProductRepo.ShiftDelete(deleted.Select(e => e.Id).ToList());

                foreach (var productUnit in ob.TransactionProductList)
                {
                    var model = productUnit.Map<TransactionProduct>();
                    model.TransactionId = Nwob.Id;
                    repoAll.transactionProductRepo.AddOrUpdate(model);
                }
                Nwob.TransactionProducts = repoAll.transactionProductRepo.GetList(e => e.TransactionId == Nwob.Id, e => e.OrderBy(e => e.Id), "", Utility.Status.All).ToList();
            }

            if (ob.TypeId == 3)
            {
                var setting = new PreferenceService(_Schema);
                if (long.Parse("0" + setting.GetByKey("AutoReceived", "Transaction", ob.TypeId, 0)?.Value) == 1 || ob.ParentId > 0)
                    new IntegrationServics(_Schema).CreateReceived(Nwob);
            }

            return Nwob.Map<TransactionModelView>();
        }

        public override bool Delete(long id)
        {
            return repo.Delete(id);
        }

        public override bool Delete(List<long> ids)
        {
            return repo.Delete(ids);
        }
        #endregion

        public override TransactionModelView Get(long Id)
        {
            var ob = base.Get(Id);
            if (ob == null)
                ob = new TransactionModelView() { TransactionProductList = new List<TransactionProductModelView>() };
            foreach (var products in ob.TransactionProductList)
            {
                products.UnitList = products.Product.ProductUnits.Select(e => e.Unit.Map<UnitModelView>()).ToList();
            }
            return ob;
        }
    }
}