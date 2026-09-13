namespace Sales.Infrastructure.Seeding
{
    using Microsoft.EntityFrameworkCore;

    public interface ISalesDataSeeder
    {
        void Seed(DbContext dbContext);
    }

    /// <summary>
    /// Sales' slice of the legacy InitialData seed (OrderType) — relocated verbatim, split by
    /// module ownership. See Administration.Infrastructure.Seeding.AdministrationDataSeeder for
    /// the shared rationale.
    /// </summary>
    public sealed class SalesDataSeeder : ISalesDataSeeder
    {
        public void Seed(DbContext dbContext)
        {
            InitialOrderType(dbContext);
        }

        public void InitialOrderType(Microsoft.EntityFrameworkCore.DbContext orgContext)
        {
            List<OrderType> list = new List<OrderType> {
                   new OrderType { Id = 1, Name = "Internal", Hide = false, Icon = "iconsminds-right-1" },
                   new OrderType { Id = 2, Name = "External", Hide = false, Icon = "iconsminds-left-1" }
            };

            //foreach (var ob in list)
            //{
            //    if (!orgContext.OrderTypes.Any(e => e.Id == ob.Id))
            //        orgContext.Set<OrderType>().Add(ob);
            //    else
            //        orgContext.Entry<OrderType>(orgContext.Set<OrderType>().Find(ob.Id)).CurrentValues.SetValues(ob);
            //}
            orgContext.SaveChanges();
        }
    }
}
