using Utility;
using Entity.Model;

namespace Entity.ModelView
{
    public class FinancialTypeModelView : BaseModel
    {
        public FinancialTypeModelView()
        {

        }

        public FinancialTypeModelView(FinancialType ob)
        {
            if (ob == null)
                ob = new FinancialType();

            this.Name = ob.Name;

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

        public FinancialType Model()
        {
            return new FinancialType
            {
                Name = this.Name,
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
    }
}