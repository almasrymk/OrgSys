using Entity.ModelView;
using Repository;
using System;
using System.Collections.Generic;
using System.Text;
using X.PagedList;

namespace Service.BAL.Data.Security
{
   public class RequestService : BaseService<RequestModelView>
   {
        string Includes = "";
        UnitOfWorkAdmin repo;

        public RequestService( )
        {
            repo = new UnitOfWorkAdmin();
        }

        #region Save / Delete
        public RequestModelView Save(RequestModelView ob)
        {
            return new RequestModelView(repo.requestRepo.AddOrUpdate(ob.Model()));
        }

        public bool Delete(long id)
        {
            return repo.requestRepo.Delete(id);
        }

        public bool Delete(List<long> ids)
        {
            return repo.requestRepo.Delete(ids);
        }
        #endregion

        #region Gets
        public RequestModelView Get(long Id)
        {
            return new RequestModelView(repo.requestRepo.Get(e => e.Id == Id, Includes));
        }

        public RequestModelView Get(string textSearch)
        {
            return new RequestModelView(repo.requestRepo.Get(e => e.Name.Contains("" + textSearch), Includes));
        }

        public RequestModelView GetEmail(string Email)
        {
            return new RequestModelView(repo.requestRepo.Get(e => e.Email.Contains("" + Email), Includes));

        }
        public RequestModelView GetByKey(string Key)
        {
            return new RequestModelView(repo.requestRepo.Get(e => e.Key == Key, Includes));
        }

        public List<RequestModelView> GetAll(long parentId = 0, long TypeId = 0)
        {
            return null;
        }

        public List<RequestModelView> GetAll(string textSearch, long parentId = 0, long TypeId = 0)
        {
            return null;
        }

        public IPagedList<RequestModelView> GetAll(long parentId = 0, long TypeId = 0, int page = 1, int pageSize = 20)
        {
            return null;
        }

        public IPagedList<RequestModelView> GetAll(string textSearch, long parentId = 0, long TypeId = 0, int page = 1, int pageSize = 20)
        {
            return null;
        }

        public List<RequestModelView> GetAll(List<long> ids, long TypeId = 0)
        {
            return null;
        }

        public long GetMaxCode(long type = 0)
        {
            return repo.requestRepo.GetMaXCode();
        }
        #endregion
    }
}
