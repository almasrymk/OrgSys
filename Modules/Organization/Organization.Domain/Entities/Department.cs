namespace Organization.Domain
{
    [Table("Department")]
    public class Department : BaseModel
    {
        [StringLength(150, MinimumLength = 3)]
        public string? Name { get; set; }

        [ForeignKey("Company")]
        public virtual long CompanyId { get; set; }

        public virtual Company? Company { get; set; }
    }
}
