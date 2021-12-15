using Utility;
using Entity.Model;

namespace Entity.ModelView
{
    public class ClassificationModelView : BaseModel
    {
        public ClassificationModelView()
        {

        }

        public ClassificationModelView(Classification ob)
        {
            if (ob == null)
                ob = new Classification();

            this.Name = ob.Name;

            this.BePurchased = ob.BePurchased;

            this.BeSold = ob.BeSold;

            this.BeManufactured = ob.BeManufactured;

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

        public Classification Model()
        {
            return new Classification
            {
                Name = this.Name,
                BePurchased = this.BePurchased,
                BeSold = this.BeSold,
                BeManufactured = this.BeManufactured,
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

        public bool BePurchased { get; set; }

        public bool BeSold { get; set; }

        public bool BeManufactured { get; set; }
    }
}