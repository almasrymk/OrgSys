using Utility;
using Entity.Model;
using Utility.Resource;
using System.ComponentModel.DataAnnotations;

namespace Entity.ModelView
{
    public class RolePermissionModelView : BaseModel
    {
        public RolePermissionModelView()
        {

        }

        public RolePermissionModelView(RolePermission ob)
        {
            if (ob == null)
                ob = new RolePermission();
            this.Id = ob.Id;
            this.RoleId = ob.RoleId;
            this.RoleName = ob.Role?.Name;
            this.PermissionId = ob.PermissionId;
            this.PermissionName = ob.Permission?.Name;
            this.Status = ob.Status;
            this.ParentId = ob.ParentId;
            this.TypeId = ob.TypeId;
            this.ImgPath = ob.ImgPath;
        }

        public RolePermission Model
        {
            get
            {
                return new RolePermission
                {
                    Id = this.Id,
                    RoleId = this.RoleId,
                    PermissionId = this.PermissionId,
                    Status = this.Status,
                    MaskText = this.MaskText,
                    ParentId = this.ParentId,
                    TypeId = this.TypeId,
                    ImgPath = this.ImgPath
                };
            }
        }
        public long RoleId { get; set; }
        public string RoleName { get; set; }
        public long PermissionId { get; set; }
        public string PermissionName { get; set; }
    }
}