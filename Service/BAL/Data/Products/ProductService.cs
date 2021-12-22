using Entity;
using System;
using System.Linq;
using Entity.Model;
using Entity.ModelView;
using System.Collections.Generic;

namespace Service
{
    public class ProductService : BaseOrgService<ProductModelView, Product>
    {
        public ProductService(string Schema) : base(Schema , "Classification,Dealer,ProductUnits,ProductRecipes,ProductPropertyElements") { }

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
    }
}