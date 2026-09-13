namespace CommercialDocuments.Infrastructure.Seeding
{
    using Microsoft.EntityFrameworkCore;

    public interface ICommercialDocumentsDataSeeder
    {
        void Seed(DbContext dbContext);
    }

    /// <summary>
    /// CommercialDocuments' slice of the legacy InitialData seed (InvoiceType — the Sales/
    /// Purchase/SalesReturn/PurchaseReturn discriminator) — relocated verbatim, split by module
    /// ownership. See Administration.Infrastructure.Seeding.AdministrationDataSeeder for the
    /// shared rationale.
    /// </summary>
    public sealed class CommercialDocumentsDataSeeder : ICommercialDocumentsDataSeeder
    {
        public void Seed(DbContext dbContext)
        {
            InitialInvoiceType(dbContext);
        }

        public void InitialInvoiceType(Microsoft.EntityFrameworkCore.DbContext orgContext)
        {
            List<InvoiceType> list = new List<InvoiceType> {
                    new InvoiceType { Id = 1, Group = "Sales", Name = "Invoice", Hide = false, InOut = 1, Icon = "simple-icon-basket-loaded" },
                    new InvoiceType { Id = 2, Group = "Purchases", Name = "Invoice", Hide = false, InOut = 1, Icon = "simple-icon-basket-loaded" },
                    new InvoiceType { Id = 3, Group = "Sales", Name = "Return", Hide = false, InOut = -1, Icon = "simple-icon-action-undo" },
                    new InvoiceType { Id = 4, Group = "Purchases", Name = "Return", Hide = false, InOut = -1, Icon = "simple-icon-action-undo" }
            };

            foreach (var ob in list)
            {
                if (!orgContext.Set<InvoiceType>().Any(e => e.Id == ob.Id))
                    orgContext.Set<InvoiceType>().Add(ob);
                else
                    orgContext.Entry<InvoiceType>(orgContext.Set<InvoiceType>().Find(ob.Id)).CurrentValues.SetValues(ob);
            }
            orgContext.SaveChanges();
        }
    }
}
