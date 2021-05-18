using Utility;
using Entity.Model;
using Utility.Resource;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;

namespace Entity.ModelView
{
   public class PropertyModelView :BaseModel
    {
        public PropertyModelView()
        {

        }
        public PropertyModelView(Property ob)
        {
            if (ob == null)
                ob = new Property();
            this.Id = ob.Id;
            this.Name = ob.Name;
            this.Status = ob.Status;
            this.ParentId = ob.ParentId;
            this.TypeId = ob.TypeId;
            this.ImgPath = ob.ImgPath;
            this.PropertyElements = ob.propertyElements.Select(e => new PropertyElementModelView(e)).ToList();

        }

        public Property Model
        {
            get
            {
                return new Property
                {
                    Id = this.Id,
                    Name = this.Name,
                    Status = this.Status,
                    MaskText = this.MaskText,
                    ParentId = this.ParentId,
                    TypeId = this.TypeId,
                    ImgPath = this.ImgPath,
                    propertyElements=null
                };
            }
        }

        [Display(Name = nameof(Title_Designer.Name), ResourceType = typeof(Title_Designer))]
        [StringLength(50, MinimumLength = 3, ErrorMessageResourceName = nameof(Message_Designer.NameRequired), ErrorMessageResourceType = typeof(Message_Designer))]
        [Required(ErrorMessageResourceName = nameof(Message_Designer.NameRequired), ErrorMessageResourceType = typeof(Message_Designer))]
        public string Name { get; set; }
        public List<PropertyElementModelView> PropertyElements { get; set; }

    }
}
