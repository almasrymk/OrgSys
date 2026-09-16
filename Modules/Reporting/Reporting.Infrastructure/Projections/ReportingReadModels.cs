namespace Reporting.Infrastructure.Projections;

using OrgSys.SharedKernel;
using System.ComponentModel.DataAnnotations.Schema;

[Table("CustomerAgingReadModel")]
public class CustomerAgingReadModel : BaseModel
{
    public virtual long CustomerId { get; set; }

    public virtual long InvoiceId { get; set; }

    public virtual DateTime InvoiceDate { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public virtual decimal OriginalAmount { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public virtual decimal OutstandingAmount { get; set; }
}

[Table("SalesSummaryReadModel")]
public class SalesSummaryReadModel : BaseModel
{
    public virtual DateTime SummaryDate { get; set; }

    public virtual long? BranchId { get; set; }

    public virtual int InvoiceCount { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public virtual decimal NetAmount { get; set; }
}
