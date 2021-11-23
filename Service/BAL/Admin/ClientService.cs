using Entity.ModelView;
using X.PagedList;
using Repository;
using System.Collections.Generic;
using System.Linq;
using Entity.Model;
using Microsoft.Extensions.Localization;

namespace Service
{
    public class ClientService : BaseService<ClientModelView>
    {
        string Includes = "TypeActivity,Nationality";
        UnitOfWorkAdmin repo;

        public ClientService()
        {
            repo = new UnitOfWorkAdmin();
        }

        #region Save / Delete
        public ClientModelView Save(ClientModelView ob)
        {
            return new ClientModelView(repo.clientRepo.AddOrUpdate(ob.Model()));
        }

        public bool Delete(long id)
        {
            return repo.clientRepo.Delete(id);
        }

        public bool Delete(List<long> ids)
        {
            return repo.clientRepo.Delete(ids);
        }
        #endregion

        #region Gets
        public ClientModelView Get(long Id)
        {
            var ob = new ClientModelView(repo.clientRepo.Get(e => e.Id == Id, Includes));
            if (ob == null)
                ob = new ClientModelView();         
            return ob;
        }

        public ClientModelView GetRequstId(long RequestId)
        {
            var ob = new ClientModelView(repo.clientRepo.Get(e => e.RequestId == RequestId, Includes));
            if (ob == null)
                ob = new ClientModelView();
            return ob;
        }

        public ClientModelView Get(string textSearch)
        {
            var ob = new ClientModelView(repo.clientRepo.Get(e => e.Name.ToLower().Trim().Equals("" + textSearch.ToLower().Trim()), Includes));
            if (ob == null)
                ob = new ClientModelView();           
            return ob;
        }

        public ClientModelView GetClientName(string textSearch)
        {
            var ob = new ClientModelView(repo.clientRepo.Get(e => e.Name.ToLower().Trim().Equals("" + textSearch.ToLower().Trim()), Includes));
            if (ob == null)
                ob = new ClientModelView();           
            return ob;
        }

        public List<ClientModelView> GetAll(long parentId = 0, long TypeId = 0)
        {
            return repo.clientRepo.GetList(e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new ClientModelView(e)).ToList();
        }

        public List<ClientModelView> GetAll(string textSearch, long parentId = 0, long TypeId = 0)
        {
            return repo.clientRepo.GetList(e => e.Name.Contains("" + textSearch), e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new ClientModelView(e)).ToList();
        }

        public IPagedList<ClientModelView> GetAll(long parentId = 0, long TypeId = 0, int page = 1, int pageSize = 20)
        {
            return repo.clientRepo.GetList(e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new ClientModelView(e)).ToPagedList(page, pageSize);
        }

        public IPagedList<ClientModelView> GetAll(string textSearch, long parentId = 0, long TypeId = 0, int page = 1, int pageSize = 20)
        {
            return repo.clientRepo.GetList(e => "" + textSearch == "" || e.Name.Contains("" + textSearch), e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new ClientModelView(e)).ToPagedList(page, pageSize);
        }

        public bool CheckDoublicat(string ClientName, long id)
        {
            var ob = new ClientModelView(repo.clientRepo.Get(e => e.Name.Equals("" + ClientName) && (id == 0 || e.Id != id)));
            return ob != null && ob.Id > 0;
        }

        public List<ClientModelView> GetAll(List<long> ids, long TypeId = 0)
        {
            return repo.clientRepo.GetList(e => ids.Contains(e.Id), e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new ClientModelView(e)).ToList();
        }

        public long GetMaxCode(long type = 0)
        {
            return repo.clientRepo.GetMaXCode();
        }

        public bool CheckEmailToClient(string Email) => repo.clientRepo.Any(e => e.Email.ToLower().Trim() == Email.ToLower().Trim());

        public bool CheckPhoneToClient(string Mobile) => repo.clientRepo.Any(e => e.Mobile.Trim() == Mobile.Trim());

        #endregion
    }
}