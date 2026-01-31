using Entity.Model;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Entity.ModelView
{
    public class RoleModelView : BaseModel
    {
        [StringLength(50, MinimumLength = 3)]
        public  string Name { get; set; }

        public List<RolePermission> PermissionList { get; set; }

        public List<TreeView> PermissionsTree { get; set; }
    }
}