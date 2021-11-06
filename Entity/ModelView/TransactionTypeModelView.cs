using Entity.Model;

namespace Entity.ModelView
{
    public class TransactionTypeModelView : BaseModel
    {
        public TransactionTypeModelView()
        {

        }

        public TransactionTypeModelView(TransactionType ob)
        {
            if (ob == null)
                ob = new TransactionType();

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

        public TransactionType Model()
        {
            return new TransactionType
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