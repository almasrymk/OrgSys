using Utility;
using Entity.Model;
using Utility.Resource;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Entity.ModelView
{
  public  class PropertyElementModelView : BaseModel
    {
        public PropertyElementModelView()
        {

        }
        public PropertyElementModelView(PropertyElement ob)
        {
            if (ob == null)
                ob = new PropertyElement();
            this.Id = ob.Id;
            this.PropertyId = ob.PropertyId;
            this.Name = ob.Name;
            this.Status = ob.Status;
            this.ParentId = ob.ParentId;
            this.TypeId = ob.TypeId;
            this.ImgPath = ob.ImgPath;
            this.PropertyName = ob.Property?.Name;
        }

        public PropertyElement Model
        {
            get
            {
                return new PropertyElement
                {
                    Id = this.Id,
                    PropertyId = this.PropertyId,
                    Name = this.Name,
                    Status = this.Status,
                    MaskText = this.MaskText,
                    ParentId = this.ParentId,
                    TypeId = this.TypeId,
                    ImgPath = this.ImgPath,
                };
            }
        }

        

        [Display(Name = nameof(Title_Designer.Properties), ResourceType = typeof(Title_Designer))]
        public long PropertyId { get; set; }

        [Display(Name = nameof(Title_Designer.Name), ResourceType = typeof(Title_Designer))]
        [StringLength(50, MinimumLength = 3, ErrorMessageResourceName = nameof(Message_Designer.NameRequired), ErrorMessageResourceType = typeof(Message_Designer))]
        [Required(ErrorMessageResourceName = nameof(Message_Designer.NameRequired), ErrorMessageResourceType = typeof(Message_Designer))]
        public string Name { get; set; }
        public string PropertyName { get; set; }


    }
}
