using Entity.Model;

namespace Entity.ModelView
{
    public class PaymentTypeModelView : BaseModel
    {
        public PaymentTypeModelView()
        {

        }

        public PaymentTypeModelView(PaymentType ob)
        {
            if (ob == null)
                ob = new PaymentType();

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

        public PaymentType Model()
        {
            return new PaymentType
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
    }
}