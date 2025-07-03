namespace Domain.Abstraction
{
    using Domain.Entities;
    using Microsoft.EntityFrameworkCore;

    public interface IAdminContext : IDisposable 
    {
        DbSet<Client> Clients { get; set; }
        DbSet<ClientPlan> ClientPlans { get; set; }
        DbSet<Plan> Plans { get; set; }
        DbSet<PlanElement> PlanElements { get; set; }
        DbSet<PlanType> PlanTypes { get; set; }
        DbSet<Request> Requests { get; set; }
        DbSet<LoginUser> LoginUsers { get; set; }
        DbSet<Nationality> Nationalities { get; set; }
        DbSet<TypeActivity> TypeActivities { get; set; }
        DbSet<GeneralCity> GeneralCities { get; set; }
        DbSet<GeneralClassification> GeneralClassifications { get; set; }
        DbSet<GeneralCountry> GeneralCountries { get; set; }
        DbSet<GeneralDistrict> GeneralDistricts { get; set; }
        DbSet<GeneralProduct> GeneralProducts { get; set; }
        DbSet<GeneralProductPropertyElement> GeneralProductPropertyElements { get; set; }
        DbSet<GeneralProductRecipe> GeneralProductRecipes { get; set; }
        DbSet<GeneralProductUnit> GeneralProductUnits { get; set; }
        DbSet<GeneralProperty> GeneralProperties { get; set; }
        DbSet<GeneralPropertyElement> GeneralPropertyElements { get; set; }
        DbSet<GeneralUnit> GeneralUnits { get; set; }

        DbSet<TEntity> Set<TEntity>() where TEntity : class;

        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}