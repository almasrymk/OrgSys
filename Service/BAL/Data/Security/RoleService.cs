using Entity;
using Entity.Model;
using Entity.ModelView;
using System.Collections.Generic;
using System.Linq;
using Utility;

namespace Service
{
    public class RoleService : BaseOrgService<RoleModelView, Role>
    {
        public RoleService(string Schema) : base(Schema) { }

        #region Save / Delete
        public override RoleModelView Save(RoleModelView ob)
        {
            var Nwob = repoAll.roleRepo.AddOrUpdate(ob.Map<Role>());

            var deleted = repoAll.rolePermissionRepo.GetList(e => e.RoleId == ob.Id, e => e.OrderBy(e => e.Id), "", Utility.Status.All).ToList();
            if (deleted != null && deleted.Count > 0)
                repoAll.rolePermissionRepo.ShiftDelete(deleted.Select(e => e.Id).ToList());

            foreach (var rolePermissions in ob.PermissionList)
            {
                var model = rolePermissions;
                model.RoleId = Nwob.Id;
                repoAll.rolePermissionRepo.AddOrUpdate(model);
            }
            return Nwob.Map<RoleModelView>();
        }
        #endregion

        #region Get
        public override RoleModelView Get(long Id)
        {
            var ob = repo.Get(e => e.Id == Id, Includes).Map<RoleModelView>();
            if (ob == null)
                ob = new RoleModelView();
            var permission = repoAll.rolePermissionRepo.GetList(e => e.RoleId == Id, null, "Role,Permission", Utility.Status.New);
            ob.PermissionsTree = repoAll.permissionRepo.GetList(e => e.OrderBy(e => e.Id), Includes).Select(e => new TreeView { Id = e.Id, Key = e.Key, Value = Translate.GetTranslate(e.Name), ParentId = e.ParentId }).ToList();
            foreach (var item in ob.PermissionsTree)
            {
                if (permission.Any(e => e.PermissionId == item.Id))
                    item.Select = true;
            }
            return ob;
        }

        public override RoleModelView Get(string textSearch)
        {
            var ob = repo.Get(e => e.Name.Contains("" + textSearch) , Includes).Map<RoleModelView>();
            if (ob == null)
                ob = new RoleModelView();
            var permission = repoAll.rolePermissionRepo.GetList(e => e.RoleId == ob.Id, null, "Role,Permission", Utility.Status.New);
            ob.PermissionsTree = repoAll.permissionRepo.GetList(e => e.OrderBy(e => e.Id), Includes).Select(e => new TreeView { Id = e.Id, Key = e.Key, Value = e.Name, ParentId = e.ParentId }).ToList();
            foreach (var item in ob.PermissionsTree)
            {
                if (permission.Any(e => e.PermissionId == item.Id))
                    item.Select = true;
            }
            return ob;
        }
        #endregion
    }
}