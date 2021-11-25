using Entity.Model;
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

            this.Id = ob.Id;

            this.CodeNumber = ob.CodeNumber;

            this.Code = ob.Code;

            this.MaskText = ob.MaskText;

            this.ParentId = ob.ParentId;

            this.TypeId = ob.TypeId;

            this.Hide = ob.Hide;

            this.ImgPath = ob.ImgPath;

            this.Status = ob.Status;

            if (ob.ProductUnits == null)
                ob.ProductUnits = new List<ProductUnit>();

            this.ProductUnits = ob.ProductUnits.Select(e => new ProductUnitModelView(e)).ToList();
        }

        public Product Model()
        {
            return new Product
            {
                Name = this.Name,
                Nickname = this.Nickname,
                Barcode = this.Barcode,
                Description = this.Description,
                Price = this.Price,
                Cost = this.Cost,
                ClassificationId = this.ClassificationId,
                DealerId = this.DealerId,
                Recipe = this.Recipe,
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

        public string Nickname { get; set; }

        public string Barcode { get; set; }

        public string Description { get; set; }

        public decimal Quantity { get; set; }

        public decimal Price { get; set; }

        public decimal Cost { get; set; }

        public long ClassificationId { get; set; }

        public string ClassificationName { get; set; }

        public long? DealerId { get; set; }

        public string DealerName { get; set; }

        public string Recipe { get; set; }

        public decimal Balance { get; set; }

        public List<ProductUnitModelView> ProductUnits { get; set; }

        public List<ProductRecipeModelView> ProductRecipes { get; set; }

        public List<ProductPropertyElementModelView> ProductPropertyElements { get; set; }

        public List<TreeView> ProductPropertyTree { get; set; }
    }
}