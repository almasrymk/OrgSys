using Domain.Entities;
using System.Linq;

namespace Repository
{
    public class CompanyProfileRepo : CurdOrg<CompanyProfile>
    {
        public CompanyProfileRepo(string Schema) : base(Schema) { }

        public CompanyProfile GetMyCompanyProfile()
        {
            return db.CompanyProfiles.FirstOrDefault();
        }
    }
}