namespace SaaS.Infrastructure.Seeding
{
    using Microsoft.EntityFrameworkCore;
    using SaaS.Domain;

    public interface ISaaSDataSeeder
    {
        void Seed(DbContext dbContext);
    }

    /// <summary>
    /// Seeds the "Default Tenant" the multi-tenant retrofit's backfill step assigns every existing
    /// row to (docs/architecture/adr/tenant-vs-company.md), plus a starter Plan/Feature catalog
    /// matching brief §53/§54's examples. Follows the same "add if none exist" idempotency as
    /// Organization.Infrastructure.Seeding.OrganizationDataSeeder.InitialCompany.
    /// </summary>
    public sealed class SaaSDataSeeder : ISaaSDataSeeder
    {
        public void Seed(DbContext dbContext)
        {
            InitialTenant(dbContext);
            InitialFeatures(dbContext);
            InitialPlans(dbContext);
            InitialPlanFeatures(dbContext);
        }

        public void InitialTenant(DbContext dbContext)
        {
            if (!dbContext.Set<Tenant>().Any())
            {
                dbContext.Set<Tenant>().Add(new Tenant
                {
                    Code = "DEFAULT",
                    CodeNumber = 1,
                    Name = "Default Tenant",
                    TenantStatus = TenantLifecycleStatus.Active,
                    ActivatedAt = DateTime.UtcNow,
                    Hide = false
                });
            }
            dbContext.SaveChanges();
        }

        public void InitialFeatures(DbContext dbContext)
        {
            if (!dbContext.Set<Feature>().Any())
            {
                var features = new List<Feature>
                {
                    new() { Code = "GeneralLedger", CodeNumber = 1, Key = "GeneralLedger", Name = "General Ledger" },
                    new() { Code = "Sales", CodeNumber = 2, Key = "Sales", Name = "Sales" },
                    new() { Code = "Purchasing", CodeNumber = 3, Key = "Purchasing", Name = "Purchasing" },
                    new() { Code = "Inventory", CodeNumber = 4, Key = "Inventory", Name = "Inventory" },
                    new() { Code = "Budgeting", CodeNumber = 5, Key = "Budgeting", Name = "Budgeting & Cost Management" },
                    new() { Code = "AdvancedReporting", CodeNumber = 6, Key = "AdvancedReporting", Name = "Advanced Reporting" },
                    new() { Code = "MultiBranch", CodeNumber = 7, Key = "MultiBranch", Name = "Multi-Branch" },
                    new() { Code = "APIAccess", CodeNumber = 8, Key = "APIAccess", Name = "API Access" },
                };
                dbContext.Set<Feature>().AddRange(features);
            }
            dbContext.SaveChanges();
        }

        public void InitialPlans(DbContext dbContext)
        {
            if (!dbContext.Set<Plan>().Any())
            {
                var plans = new List<Plan>
                {
                    new() { Code = "STARTER", CodeNumber = 1, Name = "Starter", Price = 0m, BillingPeriod = BillingPeriod.Monthly, MaxUsers = 5, MaxCompanies = 1, MaxBranches = 1, MaxWarehouses = 1 },
                    new() { Code = "PROFESSIONAL", CodeNumber = 2, Name = "Professional", Price = 49m, BillingPeriod = BillingPeriod.Monthly, MaxUsers = 25, MaxCompanies = 3, MaxBranches = 10, MaxWarehouses = 5 },
                    new() { Code = "ENTERPRISE", CodeNumber = 3, Name = "Enterprise", Price = 199m, BillingPeriod = BillingPeriod.Monthly, MaxUsers = null, MaxCompanies = null, MaxBranches = null, MaxWarehouses = null },
                };
                dbContext.Set<Plan>().AddRange(plans);
            }
            dbContext.SaveChanges();
        }

        public void InitialPlanFeatures(DbContext dbContext)
        {
            if (dbContext.Set<PlanFeature>().Any())
            {
                dbContext.SaveChanges();
                return;
            }

            var plans = dbContext.Set<Plan>().OrderBy(e => e.Id).ToList();
            var features = dbContext.Set<Feature>().OrderBy(e => e.Id).ToList();
            var starter = plans.FirstOrDefault(p => p.Code == "STARTER");
            var professional = plans.FirstOrDefault(p => p.Code == "PROFESSIONAL");
            var enterprise = plans.FirstOrDefault(p => p.Code == "ENTERPRISE");

            var planFeatures = new List<PlanFeature>();

            // Starter: core modules only.
            foreach (var key in new[] { "GeneralLedger", "Sales", "Purchasing", "Inventory" })
                AddIfFound(starter, key);

            // Professional: core + Budgeting/MultiBranch/AdvancedReporting.
            foreach (var key in new[] { "GeneralLedger", "Sales", "Purchasing", "Inventory", "Budgeting", "MultiBranch", "AdvancedReporting" })
                AddIfFound(professional, key);

            // Enterprise: every feature.
            foreach (var feature in features)
                AddIfFound(enterprise, feature.Key);

            void AddIfFound(Plan? plan, string? key)
            {
                var feature = features.FirstOrDefault(f => f.Key == key);
                if (plan is null || feature is null)
                    return;
                planFeatures.Add(new PlanFeature { PlanId = plan.Id, FeatureId = feature.Id });
            }

            if (planFeatures.Count > 0)
                dbContext.Set<PlanFeature>().AddRange(planFeatures);
            dbContext.SaveChanges();
        }
    }
}
