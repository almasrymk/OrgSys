using Entity.Model;
using Entity.ModelView;

namespace Service
{
    public class OutlayService : BaseOrgService<OutlayModelView, Outlay>
    {
        public OutlayService(string Schema) : base(Schema) { }
    }
}