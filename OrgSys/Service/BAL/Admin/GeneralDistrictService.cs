using Entity.Model;
using Entity.ModelView;

namespace Service
{
    public class GeneralDistrictService : BaseAdminService<GeneralDistrictModelView, GeneralDistrict>
    {
        public GeneralDistrictService() : base("GeneralCountry,GeneralCity") { }
    }
}