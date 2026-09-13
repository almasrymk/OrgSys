namespace Organization.Infrastructure.Seeding
{
    using Microsoft.EntityFrameworkCore;

    public interface IOrganizationDataSeeder
    {
        void Seed(DbContext dbContext);
    }

    /// <summary>
    /// Organization's slice of the legacy InitialData seed (default Branch; CompanyProfile/Shift
    /// seeding bodies were already fully commented out in the legacy file — no active OrgContext
    /// DbSet/table backs them today — moved as-is, not revived) — relocated verbatim, split by
    /// module ownership. See Administration.Infrastructure.Seeding.AdministrationDataSeeder for
    /// the shared rationale. InitialShift is preserved but was never called from the legacy
    /// Seed() orchestrator either — kept unreachable here too, matching prior behavior exactly.
    /// </summary>
    public sealed class OrganizationDataSeeder : IOrganizationDataSeeder
    {
        public void Seed(DbContext dbContext)
        {
            InitialCompanyProfile(dbContext);
            InitialBranch(dbContext);
        }

        public void InitialCompanyProfile(Microsoft.EntityFrameworkCore.DbContext orgContext)
        {
            List<CompanyProfile> list = new List<CompanyProfile> {
                 new CompanyProfile {ClientId = 1, Name = "Owner", Code = "1", CodeNumber = 1, Email1 = "info@org.com", Mobile1 = "0201111105784", Phone1 = "0201111105784", NationalityId = 68, SizeOfCompany = 1, Hide = true }
            };

            //foreach (var ob in list)
            //{
            //    ob.Id = orgContext.CompanyProfiles.FirstOrDefault(e => e.Name == ob.Name)?.Id ?? 0;
            //    if (ob.Id == 0)
            //        orgContext.Set<CompanyProfile>().AddRange(ob);
            //    else
            //        orgContext.Entry<CompanyProfile>(orgContext.Set<CompanyProfile>().Find(ob.Id)).CurrentValues.SetValues(ob);
            //}
            orgContext.SaveChanges();
        }

        public void InitialBranch(Microsoft.EntityFrameworkCore.DbContext orgContext)
        {
            List<Branch> list = new List<Branch> {
                 new Branch {Name = "Main Branch", Hide = false }
            };

            if (!orgContext.Set<Branch>().Any())
                orgContext.Set<Branch>().AddRange(list);
            orgContext.SaveChanges();
        }

        public void InitialShift(Microsoft.EntityFrameworkCore.DbContext orgContext)
        {
            List<Shift> list = new List<Shift> {
                 new Shift { Code = "1" , CodeNumber = 1 , Name = "Full Time" , Start = new TimeSpan(9,0,0) , End = new TimeSpan(17,0,0), Hide = false },
                 new Shift { Code = "2" , CodeNumber = 2 , Name = "Over Time" , Start = new TimeSpan(18,0,0) , End = new TimeSpan(8,0,0), Hide = false }
            };

            if (!orgContext.Set<Shift>().Any())
                orgContext.Set<Shift>().AddRange(list);
            orgContext.SaveChanges();
        }
    }
}
