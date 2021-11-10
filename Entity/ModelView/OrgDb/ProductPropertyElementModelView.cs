using Entity.Model;

namespace Entity.ModelView
{
    public class ProductPropertyElementModelView : BaseModel
    {
        public ProductPropertyElementModelView()
        {

        }
        public ProductPropertyElementModelView(ProductPropertyElement ob)
        {
            if (ob == null)
                ob = new ProductPropertyElement();

            this.ProductId = ob.ProductId;

            this.PropertyElementId = ob.PropertyElementId;

            this.PropertyId = ob.PropertyId;
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

        public ProductPropertyElement Model()
        {
            return new ProductPropertyElement
            {
                ProductId = this.ProductId,
                PropertyId = this.PropertyId,
                PropertyElementId = this.PropertyElementId,
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

        public long ProductId { get; set; }

        public long PropertyId { get; set; }

        public long PropertyElementId { get; set; }
    }
}