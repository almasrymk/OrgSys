using Utility;
using Entity.Model;

namespace Entity.ModelView
{
    public class BranchModelView : BaseModel
    {
        public BranchModelView()
        {

        }

        public BranchModelView(Branch ob)
        {
            if (ob == null)
                ob = new Branch();

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

        public Branch Model()
        {
            return new Branch
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