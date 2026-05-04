using Entity.Model;
using Entity.ModelView;

namespace Service
{
    public class GeneralPropertyService : BaseAdminService<GeneralPropertyModelView, GeneralProperty>
    {
        public GeneralPropertyService() : base("GeneralPropertyElements") { }
    }
}