using Utility;
using Entity.Model;

namespace Entity.ModelView
{
    public class CurrencyModelView : BaseModel
    {
        public CurrencyModelView()
        {

        }

        public CurrencyModelView(Currency ob)
        {
            if (ob == null)
                ob = new Currency();

            this.Name = ob.Name;

            this.Rate = ob.Rate;

            this.IsDefault = ob.IsDefault;

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

        public Currency Model()
        {
            return new Currency
            {
                Name = this.Name,
                Rate = this.Rate,
                IsDefault = this.IsDefault,
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

        public decimal Rate { get; set; }

        public bool IsDefault { get; set; }
    }
}