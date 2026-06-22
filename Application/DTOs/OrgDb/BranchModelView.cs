namespace Application.DTOs
{
    using Domain.Entities;
    using System.ComponentModel.DataAnnotations;

    public class BranchModelView :  BaseModel
    {
        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }
    }
}