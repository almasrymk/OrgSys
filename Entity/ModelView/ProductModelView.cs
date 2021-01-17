using Utility;
using Entity.Model;
using Utility.Resource;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;

namespace Entity.ModelView
{
    public class ProductModelView : BaseModel
    {
        public ProductModelView()
        {

        }

        public ProductModelView(Product ob)
        {
            if (ob == null)
                ob = new Product();
            this.Id = ob.Id;
            this.CodeNumber = ob.CodeNumber;
            this.Code = ob.Code;
            this.Name = ob.Name;
            this.Nickname = ob.Nickname;
            this.Barcode = ob.Barcode;
            this.Description = ob.Description;
            this.Price = ob.Price;
            this.Cost = ob.Cost;
            this.ClassificationId = ob.ClassificationId;
            this.ClassificationName = ob.Classification?.Name;
            this.DealerId = ob.DealerId;
            this.DealerName = ob.Dealer?.Name;
            this.Recipe = ob.Recipe;
            this.Status = ob.Status;
            this.ParentId = ob.ParentId;
            this.TypeId = ob.TypeId;
            this.ImgPath = ob.ImgPath;
            this.ProductUnits = ob.ProductUnits.Select(e => new ProductUnitModelView(e)).ToList();
        }

        public Product Model
        {
            get
            {
                return new Product
                {
                    Id = this.Id,
                    CodeNumber = this.CodeNumber,
                    Code = this.Code,
                    Name = this.Name,
                    Nickname = this.Nickname,
                    Barcode = this.Barcode,
                    Description = this.Description,
                    Price = this.Price,
                    Cost = this.Cost,
                    ClassificationId = this.ClassificationId,
                    DealerId = this.DealerId,
                    Status = this.Status,
                    MaskText = this.MaskText,
                    Recipe = this.Recipe,
                    ParentId = this.ParentId,
                    TypeId = this.TypeId,
                    ImgPath = this.ImgPath,
                    ProductUnits = null
                };
            }
        }

        public long CodeNumber { get; set; }

        [Display(Name = nameof(Title_Designer.Code), ResourceType = typeof(Title_Designer))]
        [StringLength(10, MinimumLength = 3, ErrorMessageResourceName = nameof(Message_Designer.CodeRange), ErrorMessageResourceType = typeof(Message_Designer))]
        [Required(ErrorMessageResourceName = nameof(Message_Designer.CodeRequired), ErrorMessageResourceType = typeof(Message_Designer))]
        public string Code { get; set; }

        [Display(Name = nameof(Title_Designer.Name), ResourceType = typeof(Title_Designer))]
        [StringLength(50, MinimumLength = 3, ErrorMessageResourceName = nameof(Message_Designer.NameRequired), ErrorMessageResourceType = typeof(Message_Designer))]
        [Required(ErrorMessageResourceName = nameof(Message_Designer.NameRequired), ErrorMessageResourceType = typeof(Message_Designer))]
        public string Name { get; set; }

        [Display(Name = nameof(Title_Designer.Nickname), ResourceType = typeof(Title_Designer))]
        [StringLength(15, MinimumLength = 3, ErrorMessageResourceName = nameof(Message_Designer.NicknameRange), ErrorMessageResourceType = typeof(Message_Designer))]
        public string Nickname { get; set; }

        [Display(Name = nameof(Title_Designer.Barcode), ResourceType = typeof(Title_Designer))]
        [StringLength(15, MinimumLength = 5, ErrorMessageResourceName = nameof(Message_Designer.BarcodeRange), ErrorMessageResourceType = typeof(Message_Designer))]
        [Required(ErrorMessageResourceName = nameof(Message_Designer.BarcodeRequired), ErrorMessageResourceType = typeof(Message_Designer))]
        public string Barcode { get; set; }

        [Display(Name = nameof(Title_Designer.Description), ResourceType = typeof(Title_Designer))]
        [StringLength(500, MinimumLength = 5, ErrorMessageResourceName = nameof(Message_Designer.DescriptionRange), ErrorMessageResourceType = typeof(Message_Designer))]
        public string Description { get; set; }

        [Display(Name = nameof(Title_Designer.Price), ResourceType = typeof(Title_Designer))]
        public decimal Price { get; set; }

        [Display(Name = nameof(Title_Designer.Cost), ResourceType = typeof(Title_Designer))]
        public decimal Cost { get; set; }

        [Display(Name = nameof(Title_Designer.Classification), ResourceType = typeof(Title_Designer))]
        [Required(ErrorMessageResourceName = nameof(Message_Designer.ClassificationRequired), ErrorMessageResourceType = typeof(Message_Designer))]
        public long ClassificationId { get; set; }

        [Display(Name = nameof(Title_Designer.Classification), ResourceType = typeof(Title_Designer))]
        public string ClassificationName { get; set; }

        [Display(Name = nameof(Title_Designer.Supplier), ResourceType = typeof(Title_Designer))]
        public long? DealerId { get; set; }

        [Display(Name = nameof(Title_Designer.Supplier), ResourceType = typeof(Title_Designer))]
        public string DealerName { get; set; }

        [Display(Name = nameof(Title_Designer.ProductRecipe), ResourceType = typeof(Title_Designer))]
        public string Recipe { get; set; }

        public List<ProductUnitModelView> ProductUnits { get; set; }
    }
}