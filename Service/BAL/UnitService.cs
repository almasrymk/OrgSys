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
    public class UnitService : BaseService<UnitModelView>
    {
        UnitOfWork repo;
        public UnitService()
        {
            repo = new UnitOfWork();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ob"></param>
        /// <returns></returns>
        public UnitModelView Save(UnitModelView ob)
        {
            return new UnitModelView(repo.unitRepo.AddOrUpdate(ob.Model));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(long id)
        {
            return repo.unitRepo.Delete(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<UnitModelView> GetAll(long parentId = 0, long TypeId = 0)
        {
            return repo.unitRepo.GetList(e => e.OrderBy(e => e.Id), "", Utility.Status.New).Select(e => new UnitModelView(e)).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textSearch"></param>
        /// <returns></returns>
        public List<UnitModelView> GetAll(string textSearch , long parentId = 0, long TypeId = 0)
        {
            return repo.unitRepo.GetList(e => e.Name.Contains("" + textSearch), e => e.OrderBy(e => e.Id), "", Utility.Status.New).Select(e => new UnitModelView(e)).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<UnitModelView> GetAll(long parentId = 0, long TypeId = 0 ,int page = 1, int pageSize = 20)
        {
            return repo.unitRepo.GetList(e => e.OrderBy(e => e.Id), "", Utility.Status.New).Select(e => new UnitModelView(e)).ToPagedList(page, pageSize);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textSearch"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<UnitModelView> GetAll(string textSearch , long parentId = 0, long TypeId = 0, int page = 1, int pageSize = 20)
        {
            return repo.unitRepo.GetList(e => "" + textSearch == "" || e.Name.Contains("" + textSearch), e => e.OrderBy(e => e.Id), "", Utility.Status.New).Select(e => new UnitModelView(e)).ToPagedList(page, pageSize);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public UnitModelView Get(long Id)
        {
            return new UnitModelView(repo.unitRepo.Get(e => e.Id == Id));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textSearch"></param>
        /// <returns></returns>
        public UnitModelView Get(string textSearch)
        {
            return new UnitModelView(repo.unitRepo.Get(e => e.Name.Contains("" + textSearch)));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool Delete(List<long> ids)
        {
            return repo.unitRepo.Delete(ids);
        }

        public List<UnitModelView> GetAll(List<long> ids, long TypeId = 0)
        {
            return repo.unitRepo.GetList(e => ids.Contains(e.Id), e => e.OrderBy(e => e.Id), "", Utility.Status.New).Select(e => new UnitModelView(e)).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textSearch"></param>
        /// <returns></returns>
        public List<UnitModelView> GetAllByProductId(long ProductId = 0)
        {
            var itemUnits =  repo.productUnitRepo.GetList(e=>e.ProductId == ProductId, null, "", Utility.Status.New).ToList();
            if (itemUnits == null)
                itemUnits = new List<ProductUnit>();
            List<long> ids = itemUnits.Select(e => e.UnitId).ToList();
            return repo.unitRepo.GetList(e => ids.Contains(e.Id), e => e.OrderBy(e => e.Id), "", Utility.Status.New).Select(e => new UnitModelView(e)).ToList();
        }
    }
}