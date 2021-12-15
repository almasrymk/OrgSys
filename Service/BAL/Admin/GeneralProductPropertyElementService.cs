using Entity.Model;
using Entity.ModelView;

namespace Service
{
    public class GeneralProductPropertyElementService : BaseAdminService<GeneralProductPropertyElementModelView, GeneralProductPropertyElement>
    {
        public GeneralProductPropertyElementService() : base("TypeActivity,Nationality") { }
    }
}