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

        UnitOfWork repo;
        public RequestService()
        {
            repo = new UnitOfWork();
        }

        public bool Delete(long id)
        {
            throw new NotImplementedException();
        }

        public bool Delete(List<long> ids)
        {
            throw new NotImplementedException();
        }

        public RequestModelView Get(long id)
        {
            throw new NotImplementedException();
        }

        public RequestModelView Get(string textSearch)
        {
            throw new NotImplementedException();
        }

        public List<RequestModelView> GetAll(long parentId = 0, long TypeId = 0)
        {
            throw new NotImplementedException();
        }

        public List<RequestModelView> GetAll(List<long> ids, long TypeId = 0)
        {
            throw new NotImplementedException();
        }

        public List<RequestModelView> GetAll(string textSearch, long parentId = 0, long TypeId = 0)
        {
            throw new NotImplementedException();
        }

        public IPagedList<RequestModelView> GetAll(long parentId = 0, long TypeId = 0, int page = 1, int pageSize = 20)
        {
            throw new NotImplementedException();
        }

        public IPagedList<RequestModelView> GetAll(string textSearch, long parentId = 0, long TypeId = 0, int page = 1, int pageSize = 20)
        {
            throw new NotImplementedException();
        }

        public long GetMaxCode(long type = 0)
        {
            throw new NotImplementedException();
        }

        public RequestModelView Save(RequestModelView ob)
        {
            return new RequestModelView(repo.requestRepo.AddOrUpdate(ob.Model));
        }
    }
}
