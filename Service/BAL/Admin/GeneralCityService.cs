using Entity.Model;
using Entity.ModelView;

namespace Service
{
    public class GeneralCityService : BaseAdminService<GeneralCityModelView, GeneralCity>
    {
        public GeneralCityService() : base("GeneralCountry") { }
    }
}