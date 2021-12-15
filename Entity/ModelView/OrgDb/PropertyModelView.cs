using Utility;
using System.Linq;
using Entity.Model;
using System.Collections.Generic;

namespace Entity.ModelView
{
    public class PropertyModelView : BaseModel
    {
        public PropertyModelView()
        {

        }
        public PropertyModelView(Property ob)
        {
            if (ob == null)
                ob = new Property();

            this.Name = ob.Name;

            this.Id = ob.Id;

            this.CodeNumber = ob.CodeNumber;

            this.Code = ob.Code;

            this.MaskText = ob.MaskText;

            this.ParentId = ob.ParentId;

            this.TypeId = ob.TypeId;

            this.Hide = ob.Hide;

            this.ImgPath = ob.ImgPath;

            this.PropertyElements = ob.propertyElements.Select(e => new PropertyElementModelView(e)).ToList();
        }

        public Property Model()
        {
            return new Property
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
                ImgPath = this.ImgPath,
                propertyElements = null
            };
        }

        public string Name { get; set; }

        public List<PropertyElementModelView> PropertyElements { get; set; }
    }
}