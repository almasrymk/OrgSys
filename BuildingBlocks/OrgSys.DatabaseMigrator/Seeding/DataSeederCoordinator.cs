namespace OrgSys.DatabaseMigrator.Seeding
{
    using Microsoft.EntityFrameworkCore;
    using Administration.Infrastructure.Seeding;
    using Sales.Infrastructure.Seeding;
    using Accounting.Infrastructure.Seeding;
    using CommercialDocuments.Infrastructure.Seeding;
    using Inventory.Infrastructure.Seeding;
    using MasterData.Infrastructure.Seeding;
    using Organization.Infrastructure.Seeding;
    using Parties.Infrastructure.Seeding;
    using Treasury.Infrastructure.Seeding;
    using SaaS.Infrastructure.Seeding;

    public interface IDataSeederCoordinator
    {
        void Seed(DbContext dbContext);
    }

    /// <summary>
    /// Replaces the legacy monolithic InitialData.Seed() — this coordinator carries no seed data
    /// of its own (per the Infrastructure-elimination brief's "the coordinator must contain no
    /// seed data itself"); it only calls each module's own seeder, in the same order the legacy
    /// Seed() method did, with one deliberate reordering: Treasury's Bank/BankBranch seeding reads
    /// MasterData's Country/City/District rows, so MasterData now runs before Treasury (in the
    /// original file this worked only because both lived in the same class with Country/City/
    /// District seeded earlier in the same method — that ordering constraint is preserved here
    /// across module boundaries instead of within one method).
    /// </summary>
    public sealed class DataSeederCoordinator(
        IAdministrationDataSeeder administrationSeeder,
        ISalesDataSeeder salesSeeder,
        IAccountingDataSeeder accountingSeeder,
        ICommercialDocumentsDataSeeder commercialDocumentsSeeder,
        IInventoryDataSeeder inventorySeeder,
        IMasterDataDataSeeder masterDataSeeder,
        IOrganizationDataSeeder organizationSeeder,
        IPartiesDataSeeder partiesSeeder,
        ITreasuryDataSeeder treasurySeeder,
        ISaaSDataSeeder saaSSeeder) : IDataSeederCoordinator
    {
        public void Seed(DbContext dbContext)
        {
            // SaaS seeds the Default Tenant first — Organization's InitialCompany (and every future
            // module's backfill-to-default-tenant step) needs a TenantId to assign, per
            // docs/architecture/adr/tenant-vs-company.md.
            saaSSeeder.Seed(dbContext);
            administrationSeeder.Seed(dbContext);
            salesSeeder.Seed(dbContext);
            accountingSeeder.Seed(dbContext);
            commercialDocumentsSeeder.Seed(dbContext);
            inventorySeeder.Seed(dbContext);
            masterDataSeeder.Seed(dbContext);
            organizationSeeder.Seed(dbContext);
            partiesSeeder.Seed(dbContext);
            treasurySeeder.Seed(dbContext);
        }
    }
}
