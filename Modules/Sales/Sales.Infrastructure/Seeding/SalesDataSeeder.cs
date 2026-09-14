namespace Sales.Infrastructure.Seeding
{
    using Microsoft.EntityFrameworkCore;

    public interface ISalesDataSeeder
    {
        void Seed(DbContext dbContext);
    }

    /// <summary>
    /// Sales has no seed data of its own yet — its only prior seed (OrderType) was removed along
    /// with the dead OrderType/Order/OrderProduct entities (a dormant POS table-ticket model with
    /// zero live references anywhere, replaced by the real Quotation/SalesOrder bounded context).
    /// Kept as a no-op so DataSeederCoordinator's per-module seeder list doesn't need touching
    /// every time a module gains/loses seed data.
    /// </summary>
    public sealed class SalesDataSeeder : ISalesDataSeeder
    {
        public void Seed(DbContext dbContext)
        {
        }
    }
}
