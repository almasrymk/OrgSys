using Entity.ModelView;
using X.PagedList;
using Repository;
using System.Collections.Generic;
using System.Linq;

namespace Service
{
    public class ClassificationService : BaseService<ClassificationModelView>
    {
        string Includes = "";
        UnitOfWork repo;
        public ClassificationService()
        {
            repo = new UnitOfWork();
        }

        #region Save / Delete
        public ClassificationModelView Save(ClassificationModelView ob)
        {
            return new ClassificationModelView(repo.classificationRepo.AddOrUpdate(ob.Model));
        }
 
        public bool Delete(long id)
        {
            return repo.classificationRepo.Delete(id);
        }

        public bool Delete(List<long> ids)
        {
            return repo.classificationRepo.Delete(ids);
        }
        #endregion

        #region Gets
        public ClassificationModelView Get(long Id)
        {
            return new ClassificationModelView(repo.classificationRepo.Get(e => e.Id == Id , Includes));
        }

        public ClassificationModelView Get(string textSearch)
        {
            return new ClassificationModelView(repo.classificationRepo.Get(e => e.Name.Contains("" + textSearch) , Includes));
        }

        public List<ClassificationModelView> GetAll(long parentId = 0, long TypeId = 0)
        {
            return repo.classificationRepo.GetList(e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new ClassificationModelView(e)).ToList();
        }
      
        public List<ClassificationModelView> GetAll(string textSearch , long parentId = 0, long TypeId = 0)
        {
            return repo.classificationRepo.GetList(e => e.Name.Contains("" + textSearch), e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new ClassificationModelView(e)).ToList();
        }
        
        public IPagedList<ClassificationModelView> GetAll(long parentId = 0, long TypeId = 0 ,int page = 1, int pageSize = 20)
        {
            return repo.classificationRepo.GetList(e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new ClassificationModelView(e)).ToPagedList(page, pageSize);
        }
        
        public IPagedList<ClassificationModelView> GetAll(string textSearch , long parentId = 0, long TypeId = 0, int page = 1, int pageSize = 20)
        {
            return repo.classificationRepo.GetList(e => "" + textSearch == "" || e.Name.Contains("" + textSearch), e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new ClassificationModelView(e)).ToPagedList(page, pageSize);
        }
               
        public List<ClassificationModelView> GetAll(List<long> ids, long TypeId = 0)
        {
            return repo.classificationRepo.GetList(e => ids.Contains(e.Id), e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new ClassificationModelView(e)).ToList();
        }

        public long GetMaxCode(long type = 0)
        {
            return repo.classificationRepo.GetMaXCode();
        }
        #endregion
    }
}