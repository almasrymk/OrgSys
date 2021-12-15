using Entity.Model;
using Entity.ModelView;

namespace Service
{
    public class GeneralPropertyElementService : BaseAdminService<GeneralPropertyElementModelView, GeneralPropertyElement>
    {
        public GeneralPropertyElementService() : base("GeneralProperty") { }
    }
}