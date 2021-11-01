using Entity.ModelView;
using X.PagedList;
using Repository;
using System.Collections.Generic;
using System.Linq;

namespace Service
{
    public class PreferenceService : BaseService<PreferenceModelView>
    {
        string Includes = "";
        UnitOfWork repo;
        public PreferenceService()
        {
            repo = new UnitOfWork();
        }

        #region Save / Delete
        public PreferenceModelView Save(PreferenceModelView ob)
        {
            return new PreferenceModelView(repo.preferenceRepo.AddOrUpdate(ob.Model()));
        }
      
        public bool Delete(long id)
        {
            return repo.preferenceRepo.Delete(id);
        }

        public bool Delete(List<long> ids)
        {
            return repo.preferenceRepo.Delete(ids);
        }
        #endregion

        #region Gets
        public PreferenceModelView Get(long Id)
        {
            return new PreferenceModelView(repo.preferenceRepo.Get(e => e.Id == Id , Includes));
        }

        public PreferenceModelView Get(string textSearch)
        {
            return new PreferenceModelView(repo.preferenceRepo.Get(e => e.Key.Contains("" + textSearch) , Includes));
        }

        public List<PreferenceModelView> GetAll(long parentId = 0, long TypeId = 0)
        {
            return repo.preferenceRepo.GetList(e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new PreferenceModelView(e)).ToList();
        }
               
        public List<PreferenceModelView> GetAll(string textSearch , long parentId = 0, long TypeId = 0)
        {
            return repo.preferenceRepo.GetList(e=> (e.Reference == "" + textSearch && e.TypeId == TypeId), e => e.OrderBy(e => e.Id), Includes, Utility.Status.All).Select(e => new PreferenceModelView(e)).ToList();
        }
       
        public IPagedList<PreferenceModelView> GetAll(long parentId = 0, long TypeId = 0 ,int page = 1, int pageSize = 20)
        {
            return repo.preferenceRepo.GetList(e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new PreferenceModelView(e)).ToPagedList(page, pageSize);
        }
      
        public IPagedList<PreferenceModelView> GetAll(string textSearch , long parentId = 0, long TypeId = 0, int page = 1, int pageSize = 20)
        {
            return repo.preferenceRepo.GetList(e => "" + textSearch == "" || e.Key.Contains("" + textSearch), e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new PreferenceModelView(e)).ToPagedList(page, pageSize);
        }            

        public List<PreferenceModelView> GetAll(List<long> ids, long TypeId = 0)
        {
            return repo.preferenceRepo.GetList(e => ids.Contains(e.Id), e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new PreferenceModelView(e)).ToList();
        }

        public PreferenceModelView GetByKey(string textSearch , string reference , long type , int userId)
        {
            return new PreferenceModelView(repo.preferenceRepo.Get(e => e.Key =="" + textSearch && (e.TypeId == type || type == 0) && (userId == 0 || e.UserId == userId) && (e.Reference == reference || "" + reference == "") ));
        }

        public long GetMaxCode(long type = 0)
        {
            return repo.preferenceRepo.GetMaXCode();
        }
        #endregion
    }
}