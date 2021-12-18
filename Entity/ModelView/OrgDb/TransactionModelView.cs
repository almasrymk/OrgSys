using Entity.Model;
using System.Collections.Generic;

namespace Entity.ModelView
{
    public class TransactionModelView : Transaction
    {
        public string DealerName { get; set; }

        public string StoreName { get; set; }

        public string ToStoreName { get; set; }

        public string ParentCode { get; set; }

        public string BranchName { get; set; }

        public string CreateUserName { get; set; }

        public string ModifyUserName { get; set; }

        public string ShiftName { get; set; }

        public List<TransactionProductModelView> TransactionProductList { get; set; }
    }
}