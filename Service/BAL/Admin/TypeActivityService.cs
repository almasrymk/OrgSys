using Entity.ModelView;
using X.PagedList;
using Repository;
using System.Collections.Generic;
using System.Linq;
using Entity.Model;
using Microsoft.Extensions.Localization;

namespace Service
{
    public class TypeActivityService : BaseService<TypeActivityModelView>
    {
        string Includes = "";
        UnitOfWorkAdmin repo;

        public TypeActivityService()
        {
            repo = new UnitOfWorkAdmin();
        } 

        #region Save / Delete
        public TypeActivityModelView Save(TypeActivityModelView ob)
        {
            return new TypeActivityModelView(repo.typeActivityRepo.AddOrUpdate(ob.Model()));
        }

        public bool Delete(long id)
        {
            return repo.typeActivityRepo.Delete(id);
        }

        public bool Delete(List<long> ids)
        {
            return repo.typeActivityRepo.Delete(ids);
        }
        #endregion

        #region Gets
        public TypeActivityModelView Get(long Id)
        {
            var ob = new TypeActivityModelView(repo.typeActivityRepo.Get(e => e.Id == Id, Includes));
            if (ob == null)
                ob = new TypeActivityModelView();         
            return ob;
        }

        public TypeActivityModelView Get(string textSearch)
        {
            var ob = new TypeActivityModelView(repo.typeActivityRepo.Get(e => e.Name.ToLower().Trim().Equals("" + textSearch.ToLower().Trim()), Includes));
            if (ob == null)
                ob = new TypeActivityModelView();           
            return ob;
        }

        public TypeActivityModelView GetTypeActivityName(string textSearch)
        {
            var ob = new TypeActivityModelView(repo.typeActivityRepo.Get(e => e.Name.ToLower().Trim().Equals("" + textSearch.ToLower().Trim()), Includes));
            if (ob == null)
                ob = new TypeActivityModelView();           
            return ob;
        }

        public List<TypeActivityModelView> GetAll(long parentId = 0, long TypeId = 0)
        {
            return repo.typeActivityRepo.GetList(e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new TypeActivityModelView(e)).ToList();
        }

        public List<TypeActivityModelView> GetAll(string textSearch, long parentId = 0, long TypeId = 0)
        {
            return repo.typeActivityRepo.GetList(e => e.Name.Contains("" + textSearch), e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new TypeActivityModelView(e)).ToList();
        }

        public IPagedList<TypeActivityModelView> GetAll(long parentId = 0, long TypeId = 0, int page = 1, int pageSize = 20)
        {
            return repo.typeActivityRepo.GetList(e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new TypeActivityModelView(e)).ToPagedList(page, pageSize);
        }

        public IPagedList<TypeActivityModelView> GetAll(string textSearch, long parentId = 0, long TypeId = 0, int page = 1, int pageSize = 20)
        {
            return repo.typeActivityRepo.GetList(e => "" + textSearch == "" || e.Name.Contains("" + textSearch), e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new TypeActivityModelView(e)).ToPagedList(page, pageSize);
        }

        public bool CheckDoublicat(string TypeActivityName, long id)
        {
            var ob = new TypeActivityModelView(repo.typeActivityRepo.Get(e => e.Name.Equals("" + TypeActivityName) && (id == 0 || e.Id != id)));
            return ob != null && ob.Id > 0;
        }

        public List<TypeActivityModelView> GetAll(List<long> ids, long TypeId = 0)
        {
            return repo.typeActivityRepo.GetList(e => ids.Contains(e.Id), e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new TypeActivityModelView(e)).ToList();
        }

        public long GetMaxCode(long type = 0)
        {
            return repo.typeActivityRepo.GetMaXCode();
        }       
        #endregion
    }
}