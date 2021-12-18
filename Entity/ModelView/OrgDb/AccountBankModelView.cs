using Entity.Model;

namespace Entity.ModelView
{
    public class AccountBankModelView : AccountBank
    {
        public string BankName { get; set; }
        public string BankBranchName { get; set; }
        public string AccountName { get; set; }
    }
}