using Entity.Model;
using Entity.ModelView;

namespace Service
{
    public class CompanyProfileService : BaseOrgService<CompanyProfileModelView, CompanyProfile>
    {
        public CompanyProfileService(string Schema) : base(Schema) { }
    }
}