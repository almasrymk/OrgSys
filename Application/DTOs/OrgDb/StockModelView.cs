using Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs
{
    public class StockModelView : BaseModel
    {
        [StringLength(50, MinimumLength = 3)]
        public  string Name { get; set; }

        public  long BranchId { get; set; }

        public string BranchName { get; set; }
    }
}