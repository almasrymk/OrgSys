using Entity.Model;

namespace Entity.ModelView
{
    public class InvoiceTypeModelView : BaseModel
    {
        public InvoiceTypeModelView()
        {

        }

        public InvoiceTypeModelView(InvoiceType ob)
        {
            if (ob == null)
                ob = new InvoiceType();

            this.Name = ob.Name;

            this.Group = ob.Group;

            this.InOut = ob.InOut;

            this.Icon = ob.Icon;

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

        public InvoiceType Model()
        {
            return new InvoiceType
            {
                Name = this.Name,
                Group = this.Group,
                InOut = this.InOut,
                Icon = this.Icon,
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

        public int InOut { get; set; }

        public string Icon { get; set; }

        public string Group { get; set; }
    }
}