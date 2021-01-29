using Utility;
using Entity.Model;
using Utility.Resource;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Entity.ModelView
{
    public class StoreModelView : BaseModel
    {
        public StoreModelView()
        {

        }

        public StoreModelView(Store ob)
        {
            if (ob == null)
                ob = new Store();
            this.Id = ob.Id;
            this.Name = ob.Name;
            this.BranchId = ob.BranchId;
            this.BranchName = ob.Branch?.Name;
            this.Status = ob.Status;
            this.ParentId = ob.ParentId;
            this.TypeId = ob.TypeId;
            this.ImgPath = ob.ImgPath;
        }

        public Store Model
        {
            get
            {
                return new Store
                {
                    Id = this.Id,
                    Name = this.Name,
                    BranchId = this.BranchId,
                    Status = this.Status,
                    MaskText = this.MaskText,
                    ParentId = this.ParentId,
                    TypeId = this.TypeId,
                    ImgPath = this.ImgPath
                };
            }
        }

        [Display(Name = nameof(Title_Designer.Name), ResourceType = typeof(Title_Designer))]
        [StringLength(50, MinimumLength = 3, ErrorMessageResourceName = nameof(Message_Designer.NameRequired), ErrorMessageResourceType = typeof(Message_Designer))]
        [Required(ErrorMessageResourceName = nameof(Message_Designer.NameRequired), ErrorMessageResourceType = typeof(Message_Designer))]
        public string Name { get; set; }

        //[Display(Name = nameof(Title_Designer.Branch), ResourceType = typeof(Title_Designer))]
        //[Required(ErrorMessageResourceName = nameof(Message_Designer.BranchRequired), ErrorMessageResourceType = typeof(Message_Designer))]
        public long BranchId { get; set; }

        [Display(Name = nameof(Title_Designer.Branch), ResourceType = typeof(Title_Designer))]
        public string BranchName { get; set; }
    }
}