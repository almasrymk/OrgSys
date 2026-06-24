namespace Domain.Entities
{
    public class BaseModel
    {
        public virtual long Id { get; set; }

        public virtual long CodeNumber { get; set; }

        public virtual string? Code { get; set; }

        public string? MaskText { get; set; }

        public long ParentId { get; set; }

        public long TypeId { get; set; }

        public bool Hide { get; set; }

        public string? ImgPath { get; set; }

        public Status Status { get; set; }

        [NotMapped]
        public List<string> CssFiles { get; set; }
    }
}