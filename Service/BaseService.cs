using Entity;
using Repository;
using X.PagedList;
using System.Collections.Generic;
using System.Linq;

namespace Service
{
    public interface IBaseService<entityModelView> where entityModelView : BaseModel
    {
        entityModelView Save(entityModelView ob);

        bool Delete(long id);

        bool Delete(List<long> ids);

        entityModelView Get(long Id);

        entityModelView Get(string textSearch);

        List<entityModelView> GetAll(long parentId =0 , long TypeId = 0);

        List<entityModelView> GetAll(List<long> Ids, long TypeId =0);

        List<entityModelView> GetAll(string textSearch , long parentId = 0, long TypeId = 0);

        IPagedList<entityModelView> GetAll(long parentId = 0, long TypeId = 0 , int page = 1, int pageSize = 20);

        IPagedList<entityModelView> GetAll(string textSearch , long parentId = 0, long TypeId = 0, int page = 1, int pageSize = 20);

        long GetMaxCode(long type = 0);
    }

    public class BaseAdminService<entityModelView, entity> : IBaseService<entityModelView> 
        where  entity : BaseModel 
        where entityModelView : BaseModel
    {
        public virtual string Includes { get; set; } = "";
        public UnitOfWorkAdmin2<entity> repo;        
        public BaseAdminService(string IncludeTables = "")
        {
            repo = new UnitOfWorkAdmin2<entity>();
            Includes = IncludeTables;             
        }

        #region Save / Delete
        public virtual entityModelView Save(entityModelView ob)
        {
            return repo.Db.AddOrUpdate(ob.Map<entity>()).Map<entityModelView>();
        }

        public virtual bool Delete(long id)
        {
            return repo.Db.Delete(id);
        }

        public virtual bool Delete(List<long> ids)
        {
            return repo.Db.Delete(ids);
        }
        #endregion

        #region Gets
        public virtual entityModelView Get(long Id)
        {
            return repo.Db.Get(e => e.Id == Id, Includes).Map<entityModelView>();
        }

        public virtual entityModelView Get(string textSearch)
        {
            return repo.Db.Get(e=> textSearch == textSearch, Includes).Map<entityModelView>();
        }

        public virtual List<entityModelView> GetAll(long parentId = 0, long TypeId = 0)
        {
            return repo.Db.GetList(e => (e.ParentId == parentId || parentId == 0) && (e.TypeId == TypeId || TypeId == 0) , e=>e.OrderBy(e=>e.Id), Includes, Utility.Status.New).Select(e => e.Map<entityModelView>()).ToList();
        }

        public virtual List<entityModelView> GetAll(List<long> ids, long TypeId = 0)
        {
            return repo.Db.GetList(e => ids.Contains(e.Id) && (e.TypeId == TypeId || TypeId == 0), e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => e.Map<entityModelView>()).ToList();
        }

        public List<entityModelView> GetAll(string textSearch, long parentId = 0, long TypeId = 0)
        {
            return repo.Db.GetList(e => ("" + textSearch == "" || e.Code.Contains("" + textSearch)) && (e.ParentId == parentId || parentId == 0) && (e.TypeId == TypeId || TypeId == 0), e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => e.Map<entityModelView>()).ToList();           
        }

        public IPagedList<entityModelView> GetAll(long parentId = 0, long TypeId = 0, int page = 1, int pageSize = 20)
        {
            return repo.Db.GetList(e => (e.ParentId == parentId || parentId == 0) && (e.TypeId == TypeId || TypeId == 0) , e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => e.Map<entityModelView>()).ToPagedList(page, pageSize);
        }

        public IPagedList<entityModelView> GetAll(string textSearch, long parentId = 0, long TypeId = 0, int page = 1, int pageSize = 20)
        {
            return repo.Db.GetList(e => ("" + textSearch == "" || e.Code.Contains("" + textSearch)) && (e.ParentId == parentId || parentId == 0) && (e.TypeId == TypeId || TypeId == 0), e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => e.Map<entityModelView>()).ToPagedList(page, pageSize);
        }

        public long GetMaxCode(long type = 0)
        {
            return repo.Db.GetMaXCode(e=> e.TypeId == type || type == 0);
        }
        #endregion
    }

    //public class BaseService<entity> : IBaseService<entity> where entity : BaseModel
    //{
    //    string Includes = "TypeActivity,Nationality";
    //    UnitOfWorkAdmin repo;

    //    public BaseService()
    //    {
    //        repo = new UnitOfWorkAdmin();
    //    }

    //    public bool Delete(long id)
    //    {
    //        throw new System.NotImplementedException();
    //    }

    //    public bool Delete(List<long> ids)
    //    {
    //        throw new System.NotImplementedException();
    //    }

    //    public entity Get(long id)
    //    {
    //        throw new System.NotImplementedException();
    //    }

    //    public entity Get(string textSearch)
    //    {
    //        throw new System.NotImplementedException();
    //    }

    //    public List<entity> GetAll(long parentId = 0, long TypeId = 0)
    //    {
    //        throw new System.NotImplementedException();
    //    }

    //    public List<entity> GetAll(List<long> ids, long TypeId = 0)
    //    {
    //        throw new System.NotImplementedException();
    //    }

    //    public List<entity> GetAll(string textSearch, long parentId = 0, long TypeId = 0)
    //    {
    //        throw new System.NotImplementedException();
    //    }

    //    public IPagedList<entity> GetAll(long parentId = 0, long TypeId = 0, int page = 1, int pageSize = 20)
    //    {
    //        throw new System.NotImplementedException();
    //    }

    //    public IPagedList<entity> GetAll(string textSearch, long parentId = 0, long TypeId = 0, int page = 1, int pageSize = 20)
    //    {
    //        throw new System.NotImplementedException();
    //    }

    //    public long GetMaxCode(long type = 0)
    //    {
    //        throw new System.NotImplementedException();
    //    }

    //    public entity Save(entity ob)
    //    {
    //        throw new System.NotImplementedException();
    //    }
    //}
}