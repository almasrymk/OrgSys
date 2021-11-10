using Entity.Model;

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

            this.Name = ob.Name;

            this.Description = ob.Description;

            this.NumberOfPeople = ob.NumberOfPeople;

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

        public Table Model()
        {
            return new Table
            {
                Name = this.Name,
                Description = this.Description,
                NumberOfPeople = this.NumberOfPeople,
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

        public string Description { get; set; }

        public int NumberOfPeople { get; set; }
    }
}