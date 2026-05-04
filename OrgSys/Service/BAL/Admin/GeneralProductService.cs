using Entity.Model;
using Entity.ModelView;

namespace Service
{
    public class GeneralProductService : BaseAdminService<GeneralProductModelView, GeneralProduct>
    {
        public GeneralProductService() : base("GeneralClassification,GeneralProductUnits,GeneralProductRecipes,GeneralProductPropertyElements") { }
    }
}