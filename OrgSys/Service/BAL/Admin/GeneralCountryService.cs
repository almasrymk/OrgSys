using Entity.Model;
using Entity.ModelView;

namespace Service
{
    public class GeneralCountryService : BaseAdminService<GeneralCountryModelView, GeneralCountry>
    {
        public GeneralCountryService() : base("TypeActivity,Nationality") { }
    }
}