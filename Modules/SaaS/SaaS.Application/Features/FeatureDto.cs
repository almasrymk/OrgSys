namespace SaaS.Application
{
    using System.ComponentModel.DataAnnotations;

    public class FeatureDto : BaseModel
    {
        [StringLength(100)]
        public string? Key { get; set; }

        [StringLength(150)]
        public string? Name { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }
    }
}
