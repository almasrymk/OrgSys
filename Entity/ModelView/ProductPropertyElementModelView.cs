using Utility;
using Entity.Model;
using Utility.Resource;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Entity.ModelView
{
   public class ProductPropertyElementModelView :BaseModel
    {
        public ProductPropertyElementModelView()
        {

        }
        public ProductPropertyElementModelView(ProductPropertyElement ob)
        {
            if (ob == null)
                ob = new ProductPropertyElement();
            this.Id = ob.Id;
            this.ProductId = ob.ProductId;
            this.PropertyElementId = ob.PropertyElementId;
            this.PropertyId = ob.PropertyId;
            this.Status = ob.Status;
            this.ParentId = ob.ParentId;
            this.TypeId = ob.TypeId;
            this.ImgPath = ob.ImgPath;
        }

        public ProductPropertyElement Model
        {
            get
            {
                return new ProductPropertyElement
                {
                    Id = this.Id,
                    ProductId = this.ProductId,
                    PropertyId=this.PropertyId,
                    PropertyElementId = this.PropertyElementId,
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
        [Display(Name = nameof(Title_Designer.Properties), ResourceType = typeof(Title_Designer))]
        public long PropertyId { get; set; }
        [Display(Name = nameof(Title_Designer.PropertyElement), ResourceType = typeof(Title_Designer))]
        public long PropertyElementId { get; set; }
    }
}
