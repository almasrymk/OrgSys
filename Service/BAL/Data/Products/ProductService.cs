using Entity;
using System.Linq;
using X.PagedList;
using Entity.Model;
using Entity.ModelView;
using System.Collections.Generic;
using System;

namespace Service
{
    public class ProductService : BaseOrgService<ProductModelView, Product>
    {
        public ProductService(string Schema) : base(Schema , "Classification,Dealer,ProductUnits,ProductRecipes,ProductPropertyElements") { }

        public override ProductModelView Get(long Id)
        {
            var ob = repo.Get(e => e.Id == Id, Includes);
            if (ob != null)
            {
                ob.ProductUnits = repoAll.productUnitRepo.GetList(e => e.ProductId == Id, e => e.OrderBy(e => e.Id), "Unit", Utility.Status.All).ToList();
                ob.ProductRecipes = repoAll.recipeRepo.GetList(e => e.ProductId == Id, e => e.OrderBy(e => e.Id), "", Utility.Status.All).ToList();
            }
            var obMw = ob.Map<ProductModelView>();
            obMw.ProductPropertyTree = GetProperties(obMw.Id);
            return obMw;
        }

        public override ProductModelView Get(string textSearch)
        {
            var ob = repo.Get(e => e.Name.Contains(textSearch) || e.Code == textSearch || e.Barcode == textSearch || "" + textSearch == "", Includes);
            if (ob != null)
            {
                ob.ProductUnits = repoAll.productUnitRepo.GetList(e => e.ProductId == ob.Id, e => e.OrderBy(e => e.Id), "", Utility.Status.All).ToList();
                ob.ProductRecipes = repoAll.recipeRepo.GetList(e => e.ProductId == ob.Id, e => e.OrderBy(e => e.Id), "", Utility.Status.All).ToList();
            }
            var obMw = ob.Map<ProductModelView>();
            obMw.ProductPropertyTree = GetProperties(obMw.Id);
            return obMw;
        }

        public List<ProductModelView> GetAllByBalance(long StoreId, DateTime date)
        {
            List<ProductModelView> list = new List<ProductModelView>();
            var trns = repoAll.transactionProductRepo.GetList(e => e.StoreId == StoreId && e.Transaction.Date <= date, e => e.OrderBy(e => e.ProductId), "Transaction,Transaction.Store,Product,Unit,Product.ProductUnits", Utility.Status.All).ToList();
            var products = trns.Select(e => e.Product).Distinct().ToList();
            foreach (var product in products)
            {
                var ob = product.Map<ProductModelView>();
                ob.Balance = trns.Where(e => e.ProductId == product.Id).Sum(e => e.Transaction.TypeId == 2 || e.Transaction.TypeId == 3 || e.Transaction.TypeId == 6 ? -1 * e.Quantity : e.Quantity);
                list.Add(ob);
            }
            return list;
        }

        public List<TreeView> GetProperties(long productId)
        {
            List<TreeView> obList = new List<TreeView>();
            var pro = repoAll.propertyRepo.GetList(null, null, "", Utility.Status.New).ToList();
            foreach (var item in pro)
            {
                obList.Add(new TreeView { Id = item.Id, Value = item.Name, Key = "" + item.Id });
                var proElement = repoAll.propertyelementRepo.GetList(e => e.PropertyId == item.Id, null, Includes, Utility.Status.New).ToList();
                foreach (var item2 in proElement)
                    obList.Add(new TreeView { Id = item2.Id, Value = item2.Name, Key = "" + item2.Id, ParentId = item.Id });
            }
            return obList;
        }
    }
}