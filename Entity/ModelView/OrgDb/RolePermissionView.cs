using Utility;
using Entity.Model;

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

            this.RoleId = ob.RoleId;

            this.RoleName = ob.Role?.Name;

            this.PermissionId = ob.PermissionId;

            this.PermissionName = ob.Permission?.Name;

            this.Id = ob.Id;

            this.CodeNumber = ob.CodeNumber;

            this.Code = ob.Code;

            this.MaskText = ob.MaskText;

            this.ParentId = ob.ParentId;

            this.TypeId = ob.TypeId;

            this.Hide = ob.Hide;

            this.ImgPath = ob.ImgPath;

            this.Status = ob.Status;
        }

        public RolePermission Model()
        {
            return new RolePermission
            {
                RoleId = this.RoleId,
                PermissionId = this.PermissionId,
                Id = this.Id,
                CodeNumber = this.CodeNumber,
                Code = this.Code,
                MaskText = this.MaskText,
                ParentId = this.ParentId,
                TypeId = this.TypeId,
                Hide = this.Hide,
                Status = this.Status,
                ImgPath = this.ImgPath
            };
        }

        public long RoleId { get; set; }

        public string RoleName { get; set; }

        public long PermissionId { get; set; }

        public string PermissionName { get; set; }
    }
}