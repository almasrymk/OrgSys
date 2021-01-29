using Utility;
using Entity.Model;
using Utility.Resource;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Entity.ModelView
{
    public class PermissionModelView : BaseModel
    {
        public PermissionModelView()
        {

        }

        public PermissionModelView(Permission ob)
        {
            if (ob == null)
                ob = new Permission();
            this.Id = ob.Id;
            this.Key = ob.Key;
            this.Value = ob.Value;
            this.Status = ob.Status;
            this.ParentId = ob.ParentId;
            this.TypeId = ob.TypeId;
            this.ImgPath = ob.ImgPath;
        }

        public Permission Model
        {
            get
            {
                return new Permission
                {
                    Id = this.Id,
                    Key = this.Key,
                    Value = this.Value,
                    Status = this.Status,
                    MaskText = this.MaskText,
                    ParentId = this.ParentId,
                    TypeId = this.TypeId,
                    ImgPath = this.ImgPath
                };
            }
        }

        public string Key { get; set; }
        public string Value { get; set; }
    }
}