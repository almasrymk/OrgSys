using Entity.Model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.ModelView
{
    public class StockModelView : BaseModel
    {
        [StringLength(50, MinimumLength = 3)]
        public  string Name { get; set; }

        public  long BranchId { get; set; }

        public string BranchName { get; set; }

        public long? AccountId { get; set; }

        public string AccountName { get; set; }
    }
}
