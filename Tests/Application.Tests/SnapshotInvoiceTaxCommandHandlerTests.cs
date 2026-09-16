using Moq;
using OrgSys.SharedKernel;
using System.Linq.Expressions;
using System.Net;
using Tax.Application.ElectronicInvoicing;
using Tax.Application.Snapshots.Commands;
using Tax.Contracts.Snapshots;
using Tax.Domain;
using Xunit;

namespace Application.Tests;

public class SnapshotInvoiceTaxCommandHandlerTests
{
    [Fact]
    public async Task Snapshot_PercentTax_PersistsCalculatedAmountAndSubmits()
    {
        var repository = new Mock<IRepository<InvoiceTaxSnapshot>>();
        repository.Setup(r => r.GetByFilterAsync(It.IsAny<Expression<Func<InvoiceTaxSnapshot, bool>>>(), It.IsAny<string>()))
            .ReturnsAsync((InvoiceTaxSnapshot?)null);
        repository.Setup(r => r.CreateAsync(It.IsAny<InvoiceTaxSnapshot>())).ReturnsAsync((InvoiceTaxSnapshot s) => s);
        var unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(u => u.SaveChangeAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        var submitter = new Mock<IElectronicInvoiceSubmitter>();
        submitter.Setup(s => s.SubmitAsync(It.IsAny<InvoiceTaxSnapshot>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ElectronicInvoiceSubmission(
                ElectronicInvoiceChannel.None,
                ElectronicInvoiceSubmissionStatus.Skipped,
                null,
                "disabled"));

        var handler = new SnapshotInvoiceTaxCommandHandler(unitOfWork.Object, repository.Object, submitter.Object);
        var result = await handler.Handle(
            new SnapshotInvoiceTaxCommand(
                42,
                "INV-1",
                1,
                TaxType: 2,
                TaxInput: 14,
                DiscountType: 1,
                Discount: 100,
                Total: 1100,
                Net: 1140,
                CurrencyId: 1,
                [new InvoiceTaxLineInput(7, 1, 1100, 0, 1000, 1100)],
                new DateTime(2026, 9, 17)),
            CancellationToken.None);

        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        repository.Verify(r => r.CreateAsync(It.Is<InvoiceTaxSnapshot>(s =>
            s.InvoiceId == 42 && s.TaxAmount == 140 && s.Lines.Count == 1)), Times.Once);
        submitter.Verify(s => s.SubmitAsync(It.IsAny<InvoiceTaxSnapshot>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Snapshot_ExistingInvoice_IsIdempotent()
    {
        var existing = InvoiceTaxSnapshot.Capture(42, "INV-1", 1, 1, 50, 0, 0, 500, 550, 1, DateTime.UtcNow);
        typeof(BaseModel).GetProperty(nameof(BaseModel.Id))!.SetValue(existing, 9);

        var repository = new Mock<IRepository<InvoiceTaxSnapshot>>();
        repository.Setup(r => r.GetByFilterAsync(It.IsAny<Expression<Func<InvoiceTaxSnapshot, bool>>>(), It.IsAny<string>()))
            .ReturnsAsync(existing);
        var unitOfWork = new Mock<IUnitOfWork>();
        var submitter = new Mock<IElectronicInvoiceSubmitter>();

        var handler = new SnapshotInvoiceTaxCommandHandler(unitOfWork.Object, repository.Object, submitter.Object);
        var result = await handler.Handle(
            new SnapshotInvoiceTaxCommand(42, "INV-1", 1, 1, 50, 0, 0, 500, 550, 1, [], DateTime.UtcNow),
            CancellationToken.None);

        Assert.Equal(9, result.Response);
        repository.Verify(r => r.CreateAsync(It.IsAny<InvoiceTaxSnapshot>()), Times.Never);
        submitter.Verify(s => s.SubmitAsync(It.IsAny<InvoiceTaxSnapshot>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
