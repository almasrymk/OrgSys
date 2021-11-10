using Entity.Model;

namespace Entity.ModelView
{
    public class PropertyElementModelView : BaseModel
    {
        public PropertyElementModelView()
        {

        }
        public PropertyElementModelView(PropertyElement ob)
        {
            if (ob == null)
                ob = new PropertyElement();

            this.PropertyId = ob.PropertyId;

            this.PropertyName = ob.Property?.Name;

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

        public PropertyElement Model()
        {
            return new PropertyElement
            {
                PropertyId = this.PropertyId,
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

        public long PropertyId { get; set; }

        public string PropertyName { get; set; }

        public string Name { get; set; }
    }
}