using Entity.Model;

namespace Entity.ModelView
{
    public class ProductUnitModelView : BaseModel
    {
        public ProductUnitModelView()
        {

        }

        public ProductUnitModelView(ProductUnit ob)
        {
            if (ob == null)
                ob = new ProductUnit();

            this.ProductId = ob.ProductId;

            this.ProductName = ob.Product?.Name;

            this.UnitId = ob.UnitId;

            this.UnitName = ob.Unit?.Name;

            this.Rate = ob.Rate;

            this.DefaultUnit = ob.DefaultUnit;

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

        public ProductUnit Model
        {
            get
            {
                return new ProductUnit
                {
                    ProductId = this.ProductId,
                    UnitId = this.UnitId,
                    Rate = this.Rate,
                    DefaultUnit = this.DefaultUnit,
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
        }

        public long ProductId { get; set; }

        public string ProductName { get; set; }

        public long UnitId { get; set; }

        public string UnitName { get; set; }

        public decimal Rate { get; set; }

        public bool DefaultUnit { get; set; }    
    }
}