namespace SaaS.Application
{
    using System.ComponentModel.DataAnnotations;

    public class PlanDto : BaseModel
    {
        [StringLength(100, MinimumLength = 2)]
        public string? Name { get; set; }

        public decimal Price { get; set; }

        public BillingPeriod BillingPeriod { get; set; }

        public int? MaxUsers { get; set; }

        public int? MaxCompanies { get; set; }

        public int? MaxBranches { get; set; }

        public int? MaxWarehouses { get; set; }

        public int? MaxTransactionsPerMonth { get; set; }
    }
}
