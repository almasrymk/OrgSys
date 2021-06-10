using Utility;
using Entity.Model;
using Utility.Resource;
using System.ComponentModel.DataAnnotations;

namespace Entity.ModelView
{
    public class TransactionTypeModelView : BaseModel
    {
        public TransactionTypeModelView()
        {

        }

        public TransactionTypeModelView(TransactionType ob)
        {
            if (ob == null)
                ob = new TransactionType();
            this.Id = ob.Id;
            this.Name = ob.Name;
            this.Status = ob.Status;
            this.InOut = ob.InOut;
            this.Icon = ob.Icon;
            this.ParentId = ob.ParentId;
            this.TypeId = ob.TypeId;
            this.ImgPath = ob.ImgPath;
        }

        public TransactionType Model
        {
            get
            {
                return new TransactionType
                {
                    Id = this.Id,
                    Name = this.Name,
                    InOut = this.InOut,
                    Icon = this.Icon,
                    Status = this.Status,
                    MaskText = this.MaskText,
                    ParentId = this.ParentId,
                    TypeId = this.TypeId,
                    ImgPath = this.ImgPath
                };
            }
        }
      
        public string Name { get; set; }
        public int InOut { get; set; }
        public string Icon { get; set; }
    }
}