namespace Parties.Infrastructure.Seeding
{
    using Microsoft.EntityFrameworkCore;

    public interface IPartiesDataSeeder
    {
        void Seed(DbContext dbContext);
    }

    /// <summary>
    /// Parties' slice of the legacy InitialData seed (default DealerGroup, default Dealer) —
    /// relocated verbatim, split by module ownership. See
    /// Administration.Infrastructure.Seeding.AdministrationDataSeeder for the shared rationale.
    /// </summary>
    public sealed class PartiesDataSeeder : IPartiesDataSeeder
    {
        public void Seed(DbContext dbContext)
        {
            InitialDealerGroup(dbContext);
            InitialDealer(dbContext);
        }

        public void InitialDealerGroup(Microsoft.EntityFrameworkCore.DbContext orgContext)
        {
            List<DealerGroup> list = new List<DealerGroup> {
                 new DealerGroup {CodeNumber = 1, Name = "Group 1", TypeId = 1, Hide = false },
                 new DealerGroup {CodeNumber = 1, Name = "Group 1", TypeId = 1, Hide = false }
            };

            if (!orgContext.Set<DealerGroup>().Any())
                orgContext.Set<DealerGroup>().AddRange(list);
            orgContext.SaveChanges();
        }

        public void InitialDealer(Microsoft.EntityFrameworkCore.DbContext orgContext)
        {
            List<Dealer> list = new List<Dealer> {
                 new Dealer { Code = "1", CodeNumber = 1, Name = "...", TypeId = 0 , Hide = false }
            };

            if (!orgContext.Set<Dealer>().Any())
                orgContext.Set<Dealer>().AddRange(list);
            orgContext.SaveChanges();
        }
    }
}
