namespace Organization.Application
{
    using System.ComponentModel.DataAnnotations;

    public class BranchDto :  BaseModel
    {
        [StringLength(50, MinimumLength = 3)]
        public string? Name { get; set; }

        public long CompanyId { get; set; }
    }
}