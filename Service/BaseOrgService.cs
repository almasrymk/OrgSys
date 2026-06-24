using Repository;
using X.PagedList;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System;
using System.Linq.Expressions;
using X.PagedList.Extensions;
using Domain.Entities;

namespace Service
{
    public class BaseOrgService<entityModelView, entity> : IBaseService<entityModelView>
        where entity : BaseModel
        where entityModelView : BaseModel
    {
        public virtual string Includes { get; set; } = "";
        public ICurd<entity> repo;
        public UnitOfWorkOrg repoAll;
        public string _Schema = "org";
        public BaseOrgService(string Schema, string IncludeTables = "")
        {
            Assembly assembly = Assembly.Load("Repository");
            var RepoName = "Repository." + typeof(entity).Name + "Repo";
            var type = assembly.GetType(RepoName);
            repo = (ICurd<entity>)Activator.CreateInstance(type, Schema);

            repoAll = new UnitOfWorkOrg(Schema);
            _Schema = Schema;
            Includes = IncludeTables;
        }

        #region Save / Delete
        public virtual entityModelView Save(entityModelView ob)
        {
            return repo.AddOrUpdate(ob.Map<entity>()).Map<entityModelView>();
        }

        public virtual bool Delete(long id)
        {
            return repo.Delete(id);
        }

        public virtual bool Delete(List<long> ids)
        {
            return repo.Delete(ids);
        }
        #endregion

        #region Gets
        public virtual entityModelView Get(long Id)
        {
            return repo.Get(e => e.Id == Id, Includes).Map<entityModelView>();
        }

        public virtual entityModelView Get(string textSearch)
        {
            return repo.Get(CreateFilter(textSearch), Includes).Map<entityModelView>();
        }

        public virtual List<entityModelView> GetAll(long parentId = 0, long TypeId = 0)
        {
            return repo.GetList(CreateFilter("" , parentId , TypeId), e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => e.Map<entityModelView>()).ToList();
        }

        public virtual List<entityModelView> GetAll(List<long> ids, long TypeId = 0)
        {
            return repo.GetList(CreateFilter(ids, 0, TypeId), e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => e.Map<entityModelView>()).ToList();
        }

        public virtual List<entityModelView> GetAll(string textSearch, long parentId = 0, long TypeId = 0)
        {
            return repo.GetList(CreateFilter(textSearch, parentId, TypeId), e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => e.Map<entityModelView>()).ToList();
        }

        public virtual IPagedList<entityModelView> GetAll(long parentId = 0, long TypeId = 0, int page = 1, int pageSize = 20)
        {
            return repo.GetList(CreateFilter("", parentId, TypeId), e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => e.Map<entityModelView>()).ToPagedList(page, pageSize);
        }

        public virtual IPagedList<entityModelView> GetAll(string textSearch, long parentId = 0, long TypeId = 0, int page = 1, int pageSize = 20)
        {
            return repo.GetList(CreateFilter(textSearch, parentId, TypeId), e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => e.Map<entityModelView>()).ToPagedList(page, pageSize);
        }

        public virtual long GetMaxCode(long type = 0)
        {
            return repo.GetMaXCode(e => e.TypeId == type || type == 0);
        }

        public virtual Func<entity, bool> CreateFilter(string textSearch)
        {
            return e => "" + textSearch == "" || e.Code.Contains("" + textSearch);
        }

        public virtual Expression<Func<entity, bool>> CreateFilter(string textSearch, long ParentId = 0, long TypeId = 0)
        {
            return e => ("" + textSearch == "" || e.Code.Contains("" + textSearch)) && (e.ParentId == ParentId || ParentId == 0) && (e.TypeId == TypeId || TypeId == 0);
        }

        public virtual Expression<Func<entity, bool>> CreateFilter(List<long> ids, long ParentId = 0, long TypeId = 0)
        {
            return e => ids.Contains(e.Id) && (e.ParentId == ParentId || ParentId == 0) && (e.TypeId == TypeId || TypeId == 0);
        }
        #endregion
    }
}