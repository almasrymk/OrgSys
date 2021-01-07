using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository
{
    public class OrgContext : DbContext
    {
        public OrgContext(DbContextOptions<OrgContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            IConfigurationRoot config = builder.Build();
            optionsBuilder.UseSqlServer(config.GetConnectionString("OrgConnection"));
        }

        public virtual DbSet<Unit> Units { get; set; }
        public virtual DbSet<Dealer> Dealers { get; set; }
    }
}