namespace Organization.Application
{
    using System.ComponentModel.DataAnnotations;

    public class DepartmentDto : BaseModel
    {
        [StringLength(150, MinimumLength = 3)]
        public string? Name { get; set; }

        public long CompanyId { get; set; }
    }
}
