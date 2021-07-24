using Entity.ModelView;
using X.PagedList;
using Repository;
using System.Collections.Generic;
using System.Linq;

namespace Service
{
    public class BranchService : BaseService<BranchModelView>
    {
        string Includes = "";
        UnitOfWork repo;
        public BranchService()
        {
            if (repo == null)
                repo = new UnitOfWork();
        }

        #region Save / Delete
        public BranchModelView Save(BranchModelView ob)
        {
            return new BranchModelView(repo.branchRepo.AddOrUpdate(ob.Model));
        }
      
        public bool Delete(long id)
        {
            return repo.branchRepo.Delete(id);
        }

        public bool Delete(List<long> ids)
        {
            return repo.branchRepo.Delete(ids);
        }
        #endregion

        #region Gets
        public BranchModelView Get(long Id)
        {
            return new BranchModelView(repo.branchRepo.Get(e => e.Id == Id , Includes));
        }

        public BranchModelView Get(string textSearch)
        {
            return new BranchModelView(repo.branchRepo.Get(e => e.Name.Contains("" + textSearch) , Includes));
        }

        public List<BranchModelView> GetAll(long parentId = 0, long TypeId = 0)
        {
            return repo.branchRepo.GetList(e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new BranchModelView(e)).ToList();
        }
      
        public List<BranchModelView> GetAll(string textSearch , long parentId = 0, long TypeId = 0)
        {
            return repo.branchRepo.GetList(e => e.Name.Contains("" + textSearch), e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new BranchModelView(e)).ToList();
        }
        
        public IPagedList<BranchModelView> GetAll(long parentId = 0, long TypeId = 0 ,int page = 1, int pageSize = 20)
        {
            return repo.branchRepo.GetList(e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new BranchModelView(e)).ToPagedList(page, pageSize);
        }
        
        public IPagedList<BranchModelView> GetAll(string textSearch , long parentId = 0, long TypeId = 0, int page = 1, int pageSize = 20)
        {
            return repo.branchRepo.GetList(e => "" + textSearch == "" || e.Name.Contains("" + textSearch), e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new BranchModelView(e)).ToPagedList(page, pageSize);
        }            

        public List<BranchModelView> GetAll(List<long> ids, long TypeId = 0)
        {
            return repo.branchRepo.GetList(e => ids.Contains(e.Id), e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new BranchModelView(e)).ToList();
        }

        public long GetMaxCode(long type = 0)
        {
            return repo.branchRepo.GetMaXCode(e => e.TypeId == type);
        }
        #endregion
    }
}