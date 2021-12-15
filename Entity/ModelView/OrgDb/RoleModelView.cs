using Utility;
using Entity.Model;
using System.Collections.Generic;

namespace Entity.ModelView
{
    public class RoleModelView : BaseModel
    {
        public RoleModelView()
        {

        }

        public RoleModelView(Role ob)
        {
            if (ob == null)
                ob = new Role();

            this.Name = ob.Name;

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

        public Role Model()
        {
            return new Role
            {
                Name = this.Name,
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

        public string Name { get; set; }

        public List<RolePermission> Permissions { get; set; }

        public List<TreeView> PermissionsTree { get; set; }
    }
}