using Entity.Model;
using Entity.ModelView;

namespace Service
{
    public class GeneralUnitService : BaseAdminService<GeneralUnitModelView, GeneralUnit>
    {
        public GeneralUnitService() : base("TypeActivity,Nationality") { }
    }
}