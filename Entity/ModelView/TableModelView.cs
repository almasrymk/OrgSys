using Utility;
using Entity.Model;
using Utility.Resource;
using System.ComponentModel.DataAnnotations;

namespace Entity.ModelView
{
    public class TableModelView : BaseModel
    {
        public TableModelView()
        {

        }

        public TableModelView(Table ob)
        {
            if (ob == null)
                ob = new Table();
            this.Id = ob.Id;
            this.Name = ob.Name;
            this.Description = ob.Description;
            this.NumberOfPeople = ob.NumberOfPeople;
            this.Status = ob.Status;
            this.ParentId = ob.ParentId;
            this.TypeId = ob.TypeId;
            this.ImgPath = ob.ImgPath;
        }

        public Table Model
        {
            get
            {
                return new Table
                {
                    Id = this.Id,
                    Name = this.Name,
                    Description = this.Description,
                     NumberOfPeople = this.NumberOfPeople,
                    Status = this.Status,
                    MaskText = this.MaskText,
                    ParentId = this.ParentId,
                    TypeId = this.TypeId,
                    ImgPath = this.ImgPath
                };
            }
        }

        [Display(Name = nameof(Title_Designer.Name), ResourceType = typeof(Title_Designer))]
        [StringLength(50, MinimumLength = 3, ErrorMessageResourceName = nameof(Message_Designer.NameRequired), ErrorMessageResourceType = typeof(Message_Designer))]
        [Required(ErrorMessageResourceName = nameof(Message_Designer.NameRequired), ErrorMessageResourceType = typeof(Message_Designer))]
        public string Name { get; set; }
        public string Description { get; set; }
        public int NumberOfPeople { get; set; }
    }
}