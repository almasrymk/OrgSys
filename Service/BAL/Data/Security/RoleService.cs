using Entity.ModelView;
using X.PagedList;
using Repository;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Localization;
using System.Reflection;
using System.Resources;
using Utility.Resource;
using Utility;

namespace Service
{
    public class RoleService : BaseService<RoleModelView>
    {
        string Includes = "";
        UnitOfWork repo;      
        public RoleService()
        {
            repo = new UnitOfWork();          
        }
      
        #region Save / Delete
        public RoleModelView Save(RoleModelView ob)
        {          
            var Nwob = repo.roleRepo.AddOrUpdate(ob.Model);

            var deleted = repo.rolePermissionRepo.GetList(e => e.RoleId == ob.Id, e => e.OrderBy(e => e.Id), "", Utility.Status.All).ToList();
            if (deleted != null && deleted.Count > 0)
                repo.rolePermissionRepo.ShiftDelete(deleted.Select(e => e.Id).ToList());

            foreach (var rolePermissions in ob.Permissions)
            {
                var model = rolePermissions;
                model.RoleId = Nwob.Id;
                repo.rolePermissionRepo.AddOrUpdate(model);
            }
            return new RoleModelView(Nwob);
        }
         
        public bool Delete(long id)
        {
            return repo.roleRepo.Delete(id);
        }

        public bool Delete(List<long> ids)
        {
            return repo.roleRepo.Delete(ids);
        }
        #endregion

        #region Gets
        public RoleModelView Get(long Id)
        {
            var ob = new RoleModelView(repo.roleRepo.Get(e => e.Id == Id , Includes));
            if (ob == null)
                ob = new RoleModelView();
            var permission = repo.rolePermissionRepo.GetList(e => e.RoleId == Id, null, "Role,Permission", Utility.Status.New);
            ob.PermissionsTree = repo.permissionRepo.GetList(e => e.OrderBy(e => e.Id), "").Select(e => new TreeView { Id = e.Id, Key = e.Key, Value = Translate.GetTranslate(e.Name), ParentId = e.ParentId }).ToList();
            foreach (var item in ob.PermissionsTree)
            {
                if (permission.Any(e => e.PermissionId == item.Id))
                    item.Select = true;
            }
            return ob;
        }

        public RoleModelView Get(string textSearch)
        {
            var ob = new RoleModelView(repo.roleRepo.Get(e => e.Name.Contains("" + textSearch) , Includes));
            if (ob == null)
                ob = new RoleModelView();
            var permission = repo.rolePermissionRepo.GetList(e => e.RoleId == ob.Id, null, "Role,Permission", Utility.Status.New);
            ob.PermissionsTree = repo.permissionRepo.GetList(e => e.OrderBy(e => e.Id), "").Select(e => new TreeView { Id = e.Id, Key = e.Key, Value = e.Name, ParentId = e.ParentId }).ToList();
            foreach (var item in ob.PermissionsTree)
            {
                if (permission.Any(e => e.PermissionId == item.Id))
                    item.Select = true;
            }
            return ob;
        }

        public List<RoleModelView> GetAll(long parentId = 0, long TypeId = 0)
        {
            return repo.roleRepo.GetList(e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new RoleModelView(e)).ToList();
        }
         
        public List<RoleModelView> GetAll(string textSearch , long parentId = 0, long TypeId = 0)
        {
            return repo.roleRepo.GetList(e => e.Name.Contains("" + textSearch), e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new RoleModelView(e)).ToList();
        }
         
        public IPagedList<RoleModelView> GetAll(long parentId = 0, long TypeId = 0 ,int page = 1, int pageSize = 20)
        {
            return repo.roleRepo.GetList(e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new RoleModelView(e)).ToPagedList(page, pageSize);
        }
         
        public IPagedList<RoleModelView> GetAll(string textSearch , long parentId = 0, long TypeId = 0, int page = 1, int pageSize = 20)
        {
            return repo.roleRepo.GetList(e => "" + textSearch == "" || e.Name.Contains("" + textSearch), e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new RoleModelView(e)).ToPagedList(page, pageSize);
        }
                
        public List<RoleModelView> GetAll(List<long> ids, long TypeId = 0)
        {
            return repo.roleRepo.GetList(e => ids.Contains(e.Id), e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => new RoleModelView(e)).ToList();
        }

        public long GetMaxCode(long type = 0)
        {
            return repo.roleRepo.GetMaXCode();
        }
        #endregion
    }
}