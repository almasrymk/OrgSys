using Entity.Model;

namespace Entity.ModelView
{
    public class BankBranchModelView : BankBranch
    {
        public string BankName { get; set; }

        public string CountryName { get; set; }

        public string CityName { get; set; }

        public string DistrictName { get; set; }
    }
}