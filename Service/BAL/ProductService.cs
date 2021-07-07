using Entity.Model;
using Entity.ModelView;
using X.PagedList;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service.BAL
{
    public class ProductService : BaseService<ProductModelView>
    {
        UnitOfWork repo;
        public ProductService()
        {
            repo = new UnitOfWork();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ob"></param>
        /// <returns></returns>
        public ProductModelView Save(ProductModelView ob)
        {    // Save
            var Nwob = repo.productRepo.AddOrUpdate(ob.Model);

            var ids = ob.ProductUnits.Select(e => e.Id).ToList();
            if (ids == null) ids = new List<long>();

            // Delete row from database
            var deleted = repo.productUnitRepo.GetList(e => e.ProductId == ob.Id && !ids.Contains(e.Id), e => e.OrderBy(e => e.Id), "", Utility.Status.All).ToList();
            if (deleted != null && deleted.Count > 0)
                repo.productUnitRepo.ShiftDelete(deleted.Select(e => e.Id).ToList());
            ////RecipeDelete from DataBase
            //var RecipeDeleted = repo.recipeRepo.GetList(e => e.ProductId == ob.Id, e => e.OrderBy(e => e.Id), "", Utility.Status.All).ToList();
            //if (RecipeDeleted != null && RecipeDeleted.Count > 0)
            //    repo.recipeRepo.ShiftDelete(RecipeDeleted.Select(e => e.Id).ToList());
        
            foreach (var productUnit in ob.ProductUnits)
            {
                var model = productUnit.Model;
                model.ProductId = Nwob.Id;
                repo.productUnitRepo.AddOrUpdate(model);
            }
            Nwob.ProductUnits = repo.productUnitRepo.GetList(e=>e.ProductId == Nwob.Id, e => e.OrderBy(e => e.Id), "", Utility.Status.All).ToList();

            //Save Recipe            
            //foreach (var productRecipe in ob.ProductRecipes)
            //{
            //    var model = productRecipe.Model;
            //    model.ProductId = Nwob.Id;
            //    repo.recipeRepo.AddOrUpdate(model);
            //}
            //Nwob.ProductRecipes = repo.recipeRepo.GetList(e => e.ProductId == Nwob.Id, e => e.OrderBy(e => e.Id), "", Utility.Status.All).ToList();
            return new ProductModelView(Nwob);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(long id)
        {
            return repo.productRepo.Delete(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<ProductModelView> GetAll(long parentId = 0, long TypeId = 0)
        {
            return repo.productRepo.GetList(e => e.OrderBy(e => e.Id), "", Utility.Status.New).Select(e => new ProductModelView(e)).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textSearch"></param>
        /// <returns></returns>
        public List<ProductModelView> GetAll(string textSearch , long parentId = 0, long TypeId = 0)
        {
            return repo.productRepo.GetList(e => e.Name.Contains("" + textSearch), e => e.OrderBy(e => e.Id), "", Utility.Status.New).Select(e => new ProductModelView(e)).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<ProductModelView> GetAll(long parentId = 0, long TypeId = 0 ,int page = 1, int pageSize = 20)
        {
            return repo.productRepo.GetList(e => e.OrderBy(e => e.Id), "", Utility.Status.New).Select(e => new ProductModelView(e)).ToPagedList(page, pageSize);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textSearch"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<ProductModelView> GetAll(string textSearch , long parentId = 0, long TypeId = 0, int page = 1, int pageSize = 20)
        {
            return repo.productRepo.GetList(e => "" + textSearch == "" || e.Name.Contains("" + textSearch) || e.Code.Contains("" + textSearch) || e.Barcode.Contains("" + textSearch) || e.Classification.Name.Contains("" + textSearch), e => e.OrderBy(e => e.Id), "Classification", Utility.Status.New).Select(e => new ProductModelView(e)).ToPagedList(page, pageSize);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textSearch"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<ProductModelView> GetAllOrderByName(string textSearch, int page = 1, int pageSize = 20)
        {
            return repo.productRepo.GetList(e => "" + textSearch == "" || e.Name.Contains("" + textSearch) || e.Code.Contains("" + textSearch) || e.Barcode.Contains("" + textSearch) || e.Classification.Name.Contains("" + textSearch), e => e.OrderBy(e => e.Name), "Classification", Utility.Status.New).Select(e => new ProductModelView(e)).ToPagedList(page, pageSize);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ProductModelView Get(long Id)
        {
            var ob = repo.productRepo.Get(e => e.Id == Id);
            if (ob != null)
            {
                ob.ProductUnits = repo.productUnitRepo.GetList(e => e.ProductId == Id, e => e.OrderBy(e => e.Id), "Unit", Utility.Status.All).ToList();
                ob.ProductRecipes = repo.recipeRepo.GetList(e => e.ProductId == Id, e => e.OrderBy(e => e.Id), "", Utility.Status.All).ToList();
            }
            var obMw = new ProductModelView(ob);
            obMw.ProductPropertyTree = GetProperties(obMw.Id);
            return obMw;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textSearch"></param>
        /// <returns></returns>
        public ProductModelView Get(string textSearch)
        {
            var ob = repo.productRepo.Get(e => e.Name.Contains(textSearch) || e.Code == textSearch || e.Barcode == textSearch || "" + textSearch == "");
            if (ob != null)
            {
                ob.ProductUnits = repo.productUnitRepo.GetList(e => e.ProductId == ob.Id, e => e.OrderBy(e => e.Id), "", Utility.Status.All).ToList();
                ob.ProductRecipes = repo.recipeRepo.GetList(e => e.ProductId == ob.Id, e => e.OrderBy(e => e.Id), "", Utility.Status.All).ToList();
            }
            var obMw = new ProductModelView(ob);
            obMw.ProductPropertyTree = GetProperties(obMw.Id);
            return obMw;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool Delete(List<long> ids)
        {
            return repo.productRepo.Delete(ids);
        }

        public List<ProductModelView> GetAll(List<long> ids, long TypeId = 0)
        {
            return repo.productRepo.GetList(e => ids.Contains(e.Id), e => e.OrderBy(e => e.Id), "", Utility.Status.New).Select(e => new ProductModelView(e)).ToList();
        }

        public List<TreeView> GetProperties(long productId)
        {
            List<TreeView> obList = new List<TreeView>();
            var pro = repo.propertyRepo.GetList(null, "", Utility.Status.New).ToList();
            foreach (var item in pro)
            {
                obList.Add(new TreeView { Id = item.Id, Value = item.Name, Key = "" + item.Id });
                var proElement = repo.propertyelementRepo.GetList(e => e.PropertyId == item.Id, null, "", Utility.Status.New).ToList();
                foreach (var item2 in proElement)
                    obList.Add(new TreeView { Id = item2.Id, Value = item2.Name, Key = "" + item2.Id, ParentId = item.Id });
            }
            return obList;
        }

        public List<ProductModelView> GetAllByBalance(long StoreId)
        {
            List<ProductModelView> list = new List<ProductModelView>();
            var trns = repo.transactionProductRepo.GetList(e => e.StoreId == StoreId, e => e.OrderBy(e => e.ProductId), "Transaction,Transaction.Store,Product,Unit,Product.ProductUnits", Utility.Status.All).ToList();
            var products = trns.Select(e => e.Product).Distinct().ToList();
            foreach (var product in products)
            {
                var ob = new ProductModelView(product);
                ob.Balance = trns.Where(e => e.ProductId == product.Id).Sum(e => e.TypeId == 1 || e.TypeId == 3 || e.TypeId == 6 ? -1 * e.Quantity : e.Quantity);
                list.Add(ob);
            }
            return list;
        }
    }
}