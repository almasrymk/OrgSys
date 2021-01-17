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
        {
            var ids = ob.ProductUnits.Select(e => e.Id).ToList();
            if (ids == null) ids = new List<long>();

            // Delete row from database
            var deleted = repo.productUnitRepo.GetList(e => e.ProductId == ob.Id && !ids.Contains(e.Id), e => e.OrderBy(e => e.Id), "", Utility.Status.All).ToList();
            if (deleted != null && deleted.Count > 0)
                repo.productUnitRepo.ShiftDelete(deleted.Select(e => e.Id).ToList());
          
            // Save
            var Nwob = repo.productRepo.AddOrUpdate(ob.Model);
            foreach (var productUnit in ob.ProductUnits)
            {
                var model = productUnit.Model;
                model.ProductId = Nwob.Id;
                repo.productUnitRepo.AddOrUpdate(model);
            }
            Nwob.ProductUnits = repo.productUnitRepo.GetList(e=>e.ProductId == Nwob.Id, e => e.OrderBy(e => e.Id), "", Utility.Status.All).ToList();
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
            return repo.productRepo.GetList(e => "" + textSearch == "" || e.Name.Contains("" + textSearch), e => e.OrderBy(e => e.Id), "Classification", Utility.Status.New).Select(e => new ProductModelView(e)).ToPagedList(page, pageSize);
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
                ob.ProductUnits = repo.productUnitRepo.GetList(e=>e.ProductId == Id, e => e.OrderBy(e => e.Id), "", Utility.Status.All).ToList();
            return new ProductModelView(ob);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textSearch"></param>
        /// <returns></returns>
        public ProductModelView Get(string textSearch)
        {
            var ob = repo.productRepo.Get(e => e.Name.Contains(textSearch) || e.Code == textSearch || "" + textSearch == "");
            if (ob != null)
                ob.ProductUnits = repo.productUnitRepo.GetList(e => e.ProductId == ob.Id, e => e.OrderBy(e => e.Id), "", Utility.Status.All).ToList();
            return new ProductModelView(ob);
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

        public List<ProductModelView> GetAll(List<long> ids)
        {
            return repo.productRepo.GetList(e => ids.Contains(e.Id), e => e.OrderBy(e => e.Id), "", Utility.Status.New).Select(e => new ProductModelView(e)).ToList();
        }
    }
}