namespace Inventory.Infrastructure.Seeding
{
    using Microsoft.EntityFrameworkCore;

    public interface IInventoryDataSeeder
    {
        void Seed(DbContext dbContext);
    }

    /// <summary>
    /// Inventory's slice of the legacy InitialData seed (TransactionType, default Stock) —
    /// relocated verbatim, split by module ownership. See
    /// Administration.Infrastructure.Seeding.AdministrationDataSeeder for the shared rationale.
    /// </summary>
    public sealed class InventoryDataSeeder : IInventoryDataSeeder
    {
        public void Seed(DbContext dbContext)
        {
            InitialTransactionType(dbContext);
            InitialStock(dbContext);
        }

        public void InitialTransactionType(Microsoft.EntityFrameworkCore.DbContext orgContext)
        {
            List<TransactionType> list = new List<TransactionType> {
                  new TransactionType { Id = 1, Name = "Addition", Hide = false, InOut = 1, Icon = "iconsminds-down-1" },
                  new TransactionType { Id = 2, Name = "Issue", Hide = false, InOut = -1, Icon = "iconsminds-up-1" },
                  new TransactionType { Id = 3, Name = "Transafer", Hide = false, InOut = -1, Icon = "iconsminds-shuffle-1" },
                  new TransactionType { Id = 4, Name = "Received", Hide = false, InOut = 1, Icon = "iconsminds-file-edit" },
                  new TransactionType { Id = 5, Name = "Adjustment In", Hide = false, InOut = 1, Icon = "" },
                  new TransactionType { Id = 6, Name = "Adjustment Out", Hide = false, InOut = -1, Icon = "" },
                  new TransactionType { Id = 7, Name = "Opening Balance", Hide = false, InOut = 1, Icon = "iconsminds-folder-open" },
                  new TransactionType { Id = 8, Name = "Damaged", Hide = false, InOut = -1, Icon = "iconsminds-bio-hazard" }
            };

            foreach (var ob in list)
            {
                if (!orgContext.Set<TransactionType>().Any(e => e.Id == ob.Id))
                    orgContext.Set<TransactionType>().Add(ob);
                else
                    orgContext.Entry<TransactionType>(orgContext.Set<TransactionType>().Find(ob.Id)).CurrentValues.SetValues(ob);
            }
            orgContext.SaveChanges();
        }

        public void InitialStock(Microsoft.EntityFrameworkCore.DbContext orgContext)
        {
            List<Stock> list = new List<Stock> {
                 new Stock {Name = "Main Stock", BranchId = 1, Hide = false }
            };

            if (!orgContext.Set<Stock>().Any())
                orgContext.Set<Stock>().AddRange(list);
            orgContext.SaveChanges();
        }
    }
}
