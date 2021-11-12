using Entity.ModelView;
using X.PagedList;
using Repository;
using System.Collections.Generic;
using System.Linq;
using Entity.Model;
using Microsoft.Extensions.Localization;

namespace Service
{
    public class NationalityService : BaseService<NationalityModelView>
    {
        string Includes = "";
        UnitOfWork repo;
        public NationalityService()
        {
            repo = new UnitOfWork();
        }
      
        #region Save / Delete
        public NationalityModelView Save(NationalityModelView ob)
        {
            return new NationalityModelView(repo.nationalityRepo.AddOrUpdate(ob.Model()));
        }

        public bool Delete(long id)
        {
            return repo.nationalityRepo.Delete(id);
        }

        public bool Delete(List<long> ids)
        {
            return repo.nationalityRepo.Delete(ids);
        }
        #endregion

        #region Gets
        public NationalityModelView Get(long Id)
        {
            var ob = new NationalityModelView(repo.nationalityRepo.Get(e => e.Id == Id, Includes));
            if (ob == null)
                ob = new NationalityModelView();         
            return ob;
        }

        public NationalityModelView Get(string textSearch)
        {
            var ob = new NationalityModelView(repo.nationalityRepo.Get(e => e.Name.ToLower().Trim().Equals("" + textSearch.ToLower().Trim()), Includes));
            if (ob == null)
                ob = new NationalityModelView();           
            return ob;
        }

        public NationalityModelView GetNationalityName(string textSearch)
        {
            var ob = new NationalityModelView(repo.nationalityRepo.Get(e => e.Name.ToLower().Trim().Equals("" + textSearch.ToLower().Trim()), Includes));
            if (ob == null)
                ob = new NationalityModelView();           
            return ob;
        }

        public List<NationalityModelView> GetAll(long parentId = 0, long TypeId = 0)
        {
            return repo.nationalityRepo.GetList(e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new NationalityModelView(e)).ToList();
        }

        public List<NationalityModelView> GetAll(string textSearch, long parentId = 0, long TypeId = 0)
        {
            return repo.nationalityRepo.GetList(e => e.Name.Contains("" + textSearch), e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new NationalityModelView(e)).ToList();
        }

        public IPagedList<NationalityModelView> GetAll(long parentId = 0, long TypeId = 0, int page = 1, int pageSize = 20)
        {
            return repo.nationalityRepo.GetList(e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new NationalityModelView(e)).ToPagedList(page, pageSize);
        }

        public IPagedList<NationalityModelView> GetAll(string textSearch, long parentId = 0, long TypeId = 0, int page = 1, int pageSize = 20)
        {
            return repo.nationalityRepo.GetList(e => "" + textSearch == "" || e.Name.Contains("" + textSearch), e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new NationalityModelView(e)).ToPagedList(page, pageSize);
        }

        public bool CheckDoublicat(string NationalityName, long id)
        {
            var ob = new NationalityModelView(repo.nationalityRepo.Get(e => e.Name.Equals("" + NationalityName) && (id == 0 || e.Id != id)));
            return ob != null && ob.Id > 0;
        }

        public List<NationalityModelView> GetAll(List<long> ids, long TypeId = 0)
        {
            return repo.nationalityRepo.GetList(e => ids.Contains(e.Id), e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new NationalityModelView(e)).ToList();
        }

        public long GetMaxCode(long type = 0)
        {
            return repo.nationalityRepo.GetMaXCode();
        }       
        #endregion
    }
}