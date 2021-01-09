using Utility;
using Entity.Model;
using Utility.Resource;
using System.ComponentModel.DataAnnotations;

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
            this.Id = ob.Id;
            this.ProductId = ob.ProductId;
            this.ProductName = ob.Product?.Name;
            this.UnitId = ob.UnitId;
            this.UnitName = ob.Unit?.Name;
            this.Rate = ob.Rate;
            this.DefaultUnit = ob.DefaultUnit;
            this.Status = ob.Status;
            this.ParentId = ob.ParentId;
            this.TypeId = ob.TypeId;
            this.ImgPath = ob.ImgPath;
        }

        public ProductUnit Model
        {
            get
            {
                return new ProductUnit
                {
                    Id = this.Id,
                    ProductId = this.ProductId,
                    UnitId = this.UnitId,
                    Rate = this.Rate,
                    DefaultUnit = this.DefaultUnit,
                    Status = this.Status,
                    MaskText = this.MaskText,
                    ParentId = this.ParentId,
                    TypeId = this.TypeId,
                    ImgPath = this.ImgPath
                };
            }
        }

        [Display(Name = nameof(Title_Designer.Product), ResourceType = typeof(Title_Designer))]
        public long ProductId { get; set; }

        [Display(Name = nameof(Title_Designer.Product), ResourceType = typeof(Title_Designer))]
        public string ProductName { get; set; }

        [Display(Name = nameof(Title_Designer.UnitMeasure), ResourceType = typeof(Title_Designer))]
        public long UnitId { get; set; }

        [Display(Name = nameof(Title_Designer.UnitMeasure), ResourceType = typeof(Title_Designer))]
        public string UnitName { get; set; }

        [Display(Name = nameof(Title_Designer.Rate), ResourceType = typeof(Title_Designer))]
        [Required(ErrorMessageResourceName = nameof(Message_Designer.RateRequired), ErrorMessageResourceType = typeof(Message_Designer))]
        public decimal Rate { get; set; }

        [Display(Name = nameof(Title_Designer.DefaultUnit), ResourceType = typeof(Title_Designer))]
        public bool DefaultUnit { get; set; }
    }
}