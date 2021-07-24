using Entity.ModelView;
using X.PagedList;
using Repository;
using System.Collections.Generic;
using System.Linq;

namespace Service
{
    public class ShiftService : BaseService<ShiftModelView>
    {
        string Includes = "";
        UnitOfWork repo;
        public ShiftService()
        {
            repo = new UnitOfWork();
        }

        #region Save / Delete
        public ShiftModelView Save(ShiftModelView ob)
        {
            return new ShiftModelView(repo.shiftRepo.AddOrUpdate(ob.Model));
        }
         
        public bool Delete(long id)
        {
            return repo.shiftRepo.Delete(id);
        }

        public bool Delete(List<long> ids)
        {
            return repo.shiftRepo.Delete(ids);
        }
        #endregion

        #region Gets
        public ShiftModelView Get(long Id)
        {
            return new ShiftModelView(repo.shiftRepo.Get(e => e.Id == Id , Includes));
        }

        public ShiftModelView Get(string textSearch)
        {
            return new ShiftModelView(repo.shiftRepo.Get(e => e.Name.Contains("" + textSearch) , Includes));
        }

        public List<ShiftModelView> GetAll(long parentId = 0, long TypeId = 0)
        {
            return repo.shiftRepo.GetList(e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new ShiftModelView(e)).ToList();
        }
         
        public List<ShiftModelView> GetAll(string textSearch , long parentId = 0, long TypeId = 0)
        {
            return repo.shiftRepo.GetList(e => e.Name.Contains("" + textSearch), e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new ShiftModelView(e)).ToList();
        }
         
        public IPagedList<ShiftModelView> GetAll(long parentId = 0, long TypeId = 0 ,int page = 1, int pageSize = 20)
        {
            return repo.shiftRepo.GetList(e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new ShiftModelView(e)).ToPagedList(page, pageSize);
        }
         
        public IPagedList<ShiftModelView> GetAll(string textSearch , long parentId = 0, long TypeId = 0, int page = 1, int pageSize = 20)
        {
            return repo.shiftRepo.GetList(e => "" + textSearch == "" || e.Name.Contains("" + textSearch), e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new ShiftModelView(e)).ToPagedList(page, pageSize);
        }
        
        public List<ShiftModelView> GetAll(List<long> ids, long TypeId = 0)
        {
            return repo.shiftRepo.GetList(e => ids.Contains(e.Id), e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new ShiftModelView(e)).ToList();
        }

        public long GetMaxCode(long type = 0)
        {
            return repo.shiftRepo.GetMaXCode();
        }
        #endregion
    }
}