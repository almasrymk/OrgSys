using Entity.Model;
using System.Collections.Generic;

namespace Entity.ModelView
{
    public class RoleModelView : Role
    {       
        public List<RolePermission> PermissionList { get; set; }

        public List<TreeView> PermissionsTree { get; set; }
    }
}