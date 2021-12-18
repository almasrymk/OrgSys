using Entity.Model;
using Entity.ModelView;

namespace Service
{
    public class UnitService : BaseOrgService<UnitModelView, Unit>
    {
        public UnitService(string Schema) : base(Schema) { }
    }
}