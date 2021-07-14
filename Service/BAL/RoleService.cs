using Entity.Model;
using Entity.ModelView;
using X.PagedList;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service.BAL
{
    public class RoleService : BaseService<RoleModelView>
    {
        UnitOfWork repo;
        public RoleService()
        {
            repo = new UnitOfWork();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ob"></param>
        /// <returns></returns>
        public RoleModelView Save(RoleModelView ob)
        {          
            var Nwob = repo.roleRepo.AddOrUpdate(ob.Model);

            var deleted = repo.rolePermissionRepo.GetList(e => e.RoleId == ob.Id, e => e.OrderBy(e => e.Id), "", Utility.Status.All).ToList();
            if (deleted != null && deleted.Count > 0)
                repo.rolePermissionRepo.ShiftDelete(deleted.Select(e => e.Id).ToList());

            foreach (var rolePermissions in ob.RolePermissions)
            {
                var model = rolePermissions;
                model.RoleId = Nwob.Id;
                repo.rolePermissionRepo.AddOrUpdate(model);
            }
            return new RoleModelView(Nwob);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(long id)
        {
            return repo.roleRepo.Delete(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<RoleModelView> GetAll(long parentId = 0, long TypeId = 0)
        {
            return repo.roleRepo.GetList(e => e.OrderBy(e => e.Id), "", Utility.Status.New).Select(e => new RoleModelView(e)).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textSearch"></param>
        /// <returns></returns>
        public List<RoleModelView> GetAll(string textSearch , long parentId = 0, long TypeId = 0)
        {
            return repo.roleRepo.GetList(e => e.Name.Contains("" + textSearch), e => e.OrderBy(e => e.Id), "", Utility.Status.New).Select(e => new RoleModelView(e)).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<RoleModelView> GetAll(long parentId = 0, long TypeId = 0 ,int page = 1, int pageSize = 20)
        {
            return repo.roleRepo.GetList(e => e.OrderBy(e => e.Id), "", Utility.Status.New).Select(e => new RoleModelView(e)).ToPagedList(page, pageSize);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textSearch"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<RoleModelView> GetAll(string textSearch , long parentId = 0, long TypeId = 0, int page = 1, int pageSize = 20)
        {
            return repo.roleRepo.GetList(e => "" + textSearch == "" || e.Name.Contains("" + textSearch), e => e.OrderBy(e => e.Id), "", Utility.Status.New).Select(e => new RoleModelView(e)).ToPagedList(page, pageSize);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public RoleModelView Get(long Id)
        {
            var ob = new RoleModelView(repo.roleRepo.Get(e => e.Id == Id));
            if (ob == null)
                ob = new RoleModelView();
            var permission = repo.rolePermissionRepo.GetList(e => e.RoleId == Id, null, "Role,Permission", Utility.Status.New);
            ob.Permissions = repo.permissionRepo.GetList(e => e.OrderBy(e => e.Id), "").Select(e => new TreeView { Id = e.Id, Key = e.Key, Value = e.Name, ParentId = e.ParentId }).ToList();
            foreach (var item in ob.Permissions)
            {
                if (permission.Any(e => e.PermissionId == item.Id))
                    item.Select = true;
            }
            return ob;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textSearch"></param>
        /// <returns></returns>
        public RoleModelView Get(string textSearch)
        {
            var ob = new RoleModelView(repo.roleRepo.Get(e => e.Name.Contains("" + textSearch)));
            if (ob == null)
                ob = new RoleModelView();
            var permission = repo.rolePermissionRepo.GetList(e => e.RoleId == ob.Id, null, "Role,Permission", Utility.Status.New);
            ob.Permissions = repo.permissionRepo.GetList(e => e.OrderBy(e => e.Id), "").Select(e => new TreeView { Id = e.Id, Key = e.Key, Value = e.Name, ParentId = e.ParentId }).ToList();
            foreach (var item in ob.Permissions)
            {
                if (permission.Any(e => e.PermissionId == item.Id))
                    item.Select = true;
            }
            return ob;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool Delete(List<long> ids)
        {
            return repo.roleRepo.Delete(ids);
        }

        public List<RoleModelView> GetAll(List<long> ids, long TypeId = 0)
        {
            return repo.roleRepo.GetList(e => ids.Contains(e.Id), e => e.OrderBy(e => e.Id), "", Utility.Status.New).Select(e => new RoleModelView(e)).ToList();
        }
    }
}