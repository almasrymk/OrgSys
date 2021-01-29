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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Permission>().HasData(
                new Permission { Id = 1, Key = "AllPage", Value = "AllPage.All", ParentId = 0 },
                    new Permission { Id = 10, Key = "Setting", Value = "Setting.All", ParentId = 1 },

                        new Permission { Id = 101, Key = "Branchs", Value = "Branchs.All", ParentId = 10 },
                            new Permission { Id = 10101, Key = "View", Value = "Branchs.View", ParentId = 101 },
                            new Permission { Id = 10102, Key = "Add", Value = "Branchs.Add", ParentId = 101 },
                            new Permission { Id = 10103, Key = "Edit", Value = "Branchs.Edit", ParentId = 101 },
                            new Permission { Id = 10104, Key = "Delete", Value = "Branchs.Delete", ParentId = 101 },

                        new Permission { Id = 102, Key = "Roles", Value = "Roles.All", ParentId = 10 },
                            new Permission { Id = 10201, Key = "View", Value = "Roles.View", ParentId = 102 },
                            new Permission { Id = 10202, Key = "Add", Value = "Roles.Add", ParentId = 102 },
                            new Permission { Id = 10203, Key = "Edit", Value = "Roles.Edit", ParentId = 102 },
                            new Permission { Id = 10204, Key = "Delete", Value = "Roles.Delete", ParentId = 102 },

                        new Permission { Id = 103, Key = "Users", Value = "Users.All", ParentId = 10 },
                            new Permission { Id = 10301, Key = "View", Value = "Users.View", ParentId = 103 },
                            new Permission { Id = 10302, Key = "Add", Value = "Users.Add", ParentId = 103 },
                            new Permission { Id = 10303, Key = "Edit", Value = "Users.Edit", ParentId = 103 },
                            new Permission { Id = 10304, Key = "Delete", Value = "Users.Delete", ParentId = 103 },

                        new Permission { Id = 104, Key = "Shifts", Value = "Shifts.All", ParentId = 10 },
                            new Permission { Id = 10401, Key = "View", Value = "Shifts.View", ParentId = 104 },
                            new Permission { Id = 10402, Key = "Add", Value = "Shifts.Add", ParentId = 104 },
                            new Permission { Id = 10403, Key = "Edit", Value = "Shifts.Edit", ParentId = 104 },
                            new Permission { Id = 10404, Key = "Delete", Value = "Shifts.Delete", ParentId = 104 },

                     new Permission { Id = 20, Key = "Sales", Value = "Sales.All", ParentId = 1 },

                        new Permission { Id = 201, Key = "Products", Value = "Products.All", ParentId = 20 },
                            new Permission { Id = 20101, Key = "View", Value = "Products.View", ParentId = 201 },
                            new Permission { Id = 20102, Key = "Add", Value = "Products.Add", ParentId = 201 },
                            new Permission { Id = 20103, Key = "Edit", Value = "Products.Edit", ParentId = 201 },
                            new Permission { Id = 20104, Key = "Delete", Value = "Products.Delete", ParentId = 201 },

                        new Permission { Id = 202, Key = "Classifications", Value = "Classifications.All", ParentId = 20 },
                            new Permission { Id = 20201, Key = "View", Value = "Classifications.View", ParentId = 202 },
                            new Permission { Id = 20202, Key = "Add", Value = "Classifications.Add", ParentId = 202 },
                            new Permission { Id = 20203, Key = "Edit", Value = "Classifications.Edit", ParentId = 202 },
                            new Permission { Id = 20204, Key = "Delete", Value = "Classifications.Delete", ParentId = 202 },

                        new Permission { Id = 203, Key = "UnitsMeasure", Value = "UnitsMeasure.All", ParentId = 20 },
                            new Permission { Id = 20301, Key = "View", Value = "UnitsMeasure.View", ParentId = 203 },
                            new Permission { Id = 20302, Key = "Add", Value = "UnitsMeasure.Add", ParentId = 203 },
                            new Permission { Id = 20303, Key = "Edit", Value = "UnitsMeasure.Edit", ParentId = 203 },
                            new Permission { Id = 20304, Key = "Delete", Value = "UnitsMeasure.Delete", ParentId = 203 },

                        new Permission { Id = 204, Key = "Clients", Value = "Clients.All", ParentId = 20 },
                            new Permission { Id = 20401, Key = "View", Value = "Clients.View", ParentId = 204 },
                            new Permission { Id = 20402, Key = "Add", Value = "Clients.Add", ParentId = 204 },
                            new Permission { Id = 20403, Key = "Edit", Value = "Clients.Edit", ParentId = 204 },
                            new Permission { Id = 20404, Key = "Delete", Value = "Clients.Delete", ParentId = 204 }
                );
        }

        public virtual DbSet<Unit> Units { get; set; }
        public virtual DbSet<Dealer> Dealers { get; set; }
        public virtual DbSet<Classification> Classifications { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductUnit> ProductUnits { get; set; }
        public virtual DbSet<Branch> Branches { get; set; }
        public virtual DbSet<Store> Stores { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Shift> Shifts { get; set; }
        public virtual DbSet<User>  Users { get; set; }
        public virtual DbSet<Permission> Permissions { get; set; }        
    }
}