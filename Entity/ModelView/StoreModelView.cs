using Entity.Model;

namespace Entity.ModelView
{
    public class StoreModelView : BaseModel
    {
        public StoreModelView()
        {

        }

        public StoreModelView(Store ob)
        {
            if (ob == null)
                ob = new Store();
            
            this.Name = ob.Name;
            this.BranchId = ob.BranchId;

            this.BranchName = ob.Branch?.Name;

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

        public Store Model
        {
            get
            {
                return new Store
                {
                    Name = this.Name,
                    BranchId = this.BranchId,
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

        public string Name { get; set; }

        public long BranchId { get; set; }

        public string BranchName { get; set; }
    }
}