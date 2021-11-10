using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Repository
{
    public class AdminContext : DbContext
    {
        public AdminContext(DbContextOptions<AdminContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            IConfigurationRoot config = builder.Build();
            optionsBuilder.UseSqlServer(config.GetConnectionString("OrgConnection"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
        }
       
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<ClientPlan> ClientPlans { get; set; }
        public virtual DbSet<Plan> Plans { get; set; }
        public virtual DbSet<PlanElement> PlanElements { get; set; }
        public virtual DbSet<PlanType> PlanTypes { get; set; }
        public virtual DbSet<Request> Requests { get; set; }
        public virtual DbSet<LoginUser> LoginUsers { get; set; }
        public virtual DbSet<Nationality> Nationalities { get; set; }
        public virtual DbSet<TypeActivity> TypeActivities { get; set; }
    }
}