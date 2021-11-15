using Entity.Model;
using Entity.ModelView;
using X.PagedList;
using Repository;
using System.Collections.Generic;
using System.Linq;

namespace Service
{
    public class UnitService : BaseService<UnitModelView>
    {
        string Includes = "";
        UnitOfWorkOrg repo;
        public UnitService(string Schema)
        {
            if (repo == null)
                repo = new UnitOfWorkOrg(Schema);
        }

        #region Save / Delete
        public UnitModelView Save(UnitModelView ob)
        {
            return new UnitModelView(repo.unitRepo.AddOrUpdate(ob.Model()));
        }
         
        public bool Delete(long id)
        {
            return repo.unitRepo.Delete(id);
        }

        public bool Delete(List<long> ids)
        {
            return repo.unitRepo.Delete(ids);
        }
        #endregion

        #region Gets
        public UnitModelView Get(long Id)
        {
            return new UnitModelView(repo.unitRepo.Get(e => e.Id == Id , Includes));
        }

        public UnitModelView Get(string textSearch)
        {
            return new UnitModelView(repo.unitRepo.Get(e => e.Name.Contains("" + textSearch) , Includes));
        }

        public List<UnitModelView> GetAll(long parentId = 0, long TypeId = 0)
        {
            return repo.unitRepo.GetList(e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new UnitModelView(e)).ToList();
        }
         
        public List<UnitModelView> GetAll(string textSearch , long parentId = 0, long TypeId = 0)
        {
            return repo.unitRepo.GetList(e => e.Name.Contains("" + textSearch), e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new UnitModelView(e)).ToList();
        }
         
        public IPagedList<UnitModelView> GetAll(long parentId = 0, long TypeId = 0 ,int page = 1, int pageSize = 20)
        {
            return repo.unitRepo.GetList(e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new UnitModelView(e)).ToPagedList(page, pageSize);
        }
         
        public IPagedList<UnitModelView> GetAll(string textSearch , long parentId = 0, long TypeId = 0, int page = 1, int pageSize = 20)
        {
            return repo.unitRepo.GetList(e => "" + textSearch == "" || e.Name.Contains("" + textSearch), e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new UnitModelView(e)).ToPagedList(page, pageSize);
        }
                
        public List<UnitModelView> GetAll(List<long> ids, long TypeId = 0)
        {
            return repo.unitRepo.GetList(e => ids.Contains(e.Id), e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new UnitModelView(e)).ToList();
        }
         
        public List<UnitModelView> GetAllByProductId(long ProductId = 0)
        {
            var itemUnits =  repo.productUnitRepo.GetList(e=>e.ProductId == ProductId, null, Includes, Utility.Status.New).ToList();
            if (itemUnits == null)
                itemUnits = new List<ProductUnit>();
            List<long> ids = itemUnits.Select(e => e.UnitId).ToList();
            return repo.unitRepo.GetList(e => ids.Contains(e.Id), e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new UnitModelView(e)).ToList();
        }

        public long GetMaxCode(long type = 0)
        {
            return repo.productUnitRepo.GetMaXCode();
        }
        #endregion
    }
}