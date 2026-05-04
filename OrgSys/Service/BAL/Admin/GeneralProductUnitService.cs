using Entity.Model;
using Entity.ModelView;

namespace Service
{
    public class GeneralProductUnitService : BaseAdminService<GeneralProductUnitModelView, GeneralProductUnit>
    {
        public GeneralProductUnitService() : base("GeneralProduct,GeneralUnit") { }
    }
}