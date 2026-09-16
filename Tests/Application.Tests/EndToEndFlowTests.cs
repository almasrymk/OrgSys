using Accounting.Contracts.Postings;
using Budgeting.Application.Budgets.Commands;
using Budgeting.Application.Budgets.Queries;
using Budgeting.Contracts.Budgets;
using Budgeting.Domain;
using Catalog.Contracts.Products;
using CommercialDocuments.Contracts.Invoices;
using FixedAssets.Application.Assets.Commands;
using FixedAssets.Contracts.Assets;
using FixedAssets.Domain;
using Inventory.Application.InventoryReceipts.Commands;
using Inventory.Contracts.Availability;
using Inventory.Domain;
using Inventory.Domain.Events;
using MediatR;
using Moq;
using OrgSys.SharedKernel;
using Purchasing.Application.PurchaseOrders.Commands;
using Purchasing.Application.PurchaseRequisitions.Commands;
using Purchasing.Domain;
using Receivables.Application.Invoices.Integration;
using Receivables.Application.Payments.Integration;
using Receivables.Domain;
using Receivables.Domain.Repositories;
using Sales.Application.Quotations.Commands;
using Sales.Application.SalesOrders.Commands;
using Sales.Domain;
using System.Linq.Expressions;
using System.Net;
using Treasury.Contracts.IntegrationEvents;
using Workflow.Contracts.Approvals;
using Xunit;
using CommercialDocuments.Contracts.IntegrationEvents;

namespace Application.Tests;

public class EndToEndFlowTests
{
    private static readonly DateTime Day = new(2026, 9, 1);

    [Fact]
    public async Task FlowA_Quotation_To_ConfirmedSalesOrder()
    {
        Quotation? quotation = null;
        SalesOrder? order = null;
        var quotationRepository = new Mock<IRepository<Quotation>>();
        quotationRepository.Setup(r => r.CreateAsync(It.IsAny<Quotation>()))
            .Callback<Quotation>(q =>
            {
                typeof(BaseModel).GetProperty(nameof(BaseModel.Id))!.SetValue(q, 8);
                quotation = q;
            })
            .ReturnsAsync((Quotation q) => q);
        quotationRepository.Setup(r => r.GetByFilterAsync(It.IsAny<Expression<Func<Quotation, bool>>>(), It.IsAny<string>()))
            .ReturnsAsync(() => quotation);
        quotationRepository.Setup(r => r.UpdateAsync(It.IsAny<Quotation>())).ReturnsAsync(true);

        var orderRepository = new Mock<IRepository<SalesOrder>>();
        orderRepository.Setup(r => r.CreateAsync(It.IsAny<SalesOrder>()))
            .Callback<SalesOrder>(o =>
            {
                typeof(BaseModel).GetProperty(nameof(BaseModel.Id))!.SetValue(o, 21);
                order = o;
            })
            .ReturnsAsync((SalesOrder o) => o);
        orderRepository.Setup(r => r.GetByFilterAsync(It.IsAny<Expression<Func<SalesOrder, bool>>>(), It.IsAny<string>()))
            .ReturnsAsync(() => order);
        orderRepository.Setup(r => r.UpdateAsync(It.IsAny<SalesOrder>())).ReturnsAsync(true);

        var unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(u => u.SaveChangeAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var catalog = new Mock<ISender>();
        catalog.Setup(s => s.Send(It.IsAny<GetProductNamesQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Result<Dictionary<long, string?>>(HttpStatusCode.OK, new Dictionary<long, string?> { [10] = "Widget" }, null));
        catalog.Setup(s => s.Send(It.IsAny<ReserveInventoryCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Result<long>(HttpStatusCode.OK, 9, null));

        var created = await new CreateQuotationCommandHandler(quotationRepository.Object, unitOfWork.Object, catalog.Object)
            .Handle(new CreateQuotationCommand(1, 1, 1, Day, Day.AddDays(10), 1, Day, 1, null, "Q-1",
                [new QuotationLineInput(10, 1, 2, 5, 0, 0, null)]), CancellationToken.None);
        Assert.Equal(HttpStatusCode.OK, created.StatusCode);

        Assert.Equal(HttpStatusCode.OK, (await new SendQuotationCommandHandler(quotationRepository.Object, unitOfWork.Object)
            .Handle(new SendQuotationCommand(8), CancellationToken.None)).StatusCode);
        Assert.Equal(HttpStatusCode.OK, (await new AcceptQuotationCommandHandler(quotationRepository.Object, unitOfWork.Object)
            .Handle(new AcceptQuotationCommand(8), CancellationToken.None)).StatusCode);

        var converted = await new ConvertQuotationCommandHandler(quotationRepository.Object, orderRepository.Object, unitOfWork.Object)
            .Handle(new ConvertQuotationCommand(8, 1), CancellationToken.None);
        Assert.Equal(HttpStatusCode.OK, converted.StatusCode);
        Assert.Equal(QuotationStatus.Converted, quotation!.LifecycleStatus);

        var confirmed = await new ConfirmSalesOrderCommandHandler(orderRepository.Object, unitOfWork.Object, catalog.Object)
            .Handle(new ConfirmSalesOrderCommand(21, 3, null, 1), CancellationToken.None);
        Assert.Equal(HttpStatusCode.OK, confirmed.StatusCode);
        Assert.Equal(SalesOrderStatus.Confirmed, order!.LifecycleStatus);
    }

    [Fact]
    public async Task FlowB_PurchaseRequisition_Submit_Convert_LinkInvoice()
    {
        var requisition = PurchaseRequisition.Create(1, Day, 1, "need stock");
        requisition.AddLine(10, 1, 4);
        typeof(BaseModel).GetProperty(nameof(BaseModel.Id))!.SetValue(requisition, 15);

        PurchaseOrder? order = null;
        var requisitionRepository = new Mock<IRepository<PurchaseRequisition>>();
        requisitionRepository.Setup(r => r.GetByFilterAsync(It.IsAny<Expression<Func<PurchaseRequisition, bool>>>(), It.IsAny<string>()))
            .ReturnsAsync(requisition);
        requisitionRepository.Setup(r => r.UpdateAsync(It.IsAny<PurchaseRequisition>())).ReturnsAsync(true);

        var orderRepository = new Mock<IRepository<PurchaseOrder>>();
        orderRepository.Setup(r => r.CreateAsync(It.IsAny<PurchaseOrder>()))
            .Callback<PurchaseOrder>(o =>
            {
                typeof(BaseModel).GetProperty(nameof(BaseModel.Id))!.SetValue(o, 44);
                order = o;
            })
            .ReturnsAsync((PurchaseOrder o) => o);
        orderRepository.Setup(r => r.GetByFilterAsync(It.IsAny<Expression<Func<PurchaseOrder, bool>>>(), It.IsAny<string>()))
            .ReturnsAsync(() => order);
        orderRepository.Setup(r => r.UpdateAsync(It.IsAny<PurchaseOrder>())).ReturnsAsync(true);

        var unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(u => u.SaveChangeAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        unitOfWork.Setup(u => u.BeginTransactionAsync()).Returns(Task.CompletedTask);
        unitOfWork.Setup(u => u.CommitAsync()).Returns(Task.CompletedTask);

        var sender = new Mock<ISender>();
        sender.Setup(s => s.Send(It.IsAny<StartApprovalCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Result(HttpStatusCode.OK, null));
        sender.Setup(s => s.Send(It.IsAny<GetInvoiceReferenceQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Result<InvoiceReferenceDto?>(
                HttpStatusCode.OK,
                new InvoiceReferenceDto(100, "PINV-100", InvoiceTypeId.Purchase, 5, IsDeleted: false),
                null));

        var submitted = await new SubmitCommandHandler(unitOfWork.Object, requisitionRepository.Object, sender.Object)
            .Handle(new SubmitPurchaseRequisitionCommand(15), CancellationToken.None);
        Assert.Equal(HttpStatusCode.OK, submitted.StatusCode);
        Assert.Equal(Status.UnderReview, requisition.Status);
        sender.Verify(s => s.Send(It.Is<StartApprovalCommand>(c => c.DocumentId == 15), It.IsAny<CancellationToken>()), Times.Once);

        var converted = await new ConvertToPurchaseOrderCommandHandler(unitOfWork.Object, requisitionRepository.Object, orderRepository.Object)
            .Handle(new ConvertToPurchaseOrderCommand(15, 7), CancellationToken.None);
        Assert.Equal(HttpStatusCode.OK, converted.StatusCode);
        Assert.Equal(Status.Approved, requisition.Status);

        var linked = await new LinkInvoiceCommandHandler(unitOfWork.Object, orderRepository.Object, sender.Object)
            .Handle(new LinkInvoiceCommand(44, 100), CancellationToken.None);
        Assert.Equal(HttpStatusCode.OK, linked.StatusCode);
        Assert.Equal(100, order!.InvoiceId);
    }

    [Fact]
    public async Task FlowC_InventoryReceipt_CreateConfirmPost()
    {
        InventoryReceipt? receipt = null;
        var repository = new Mock<IRepository<InventoryReceipt>>();
        repository.Setup(r => r.CreateAsync(It.IsAny<InventoryReceipt>()))
            .Callback<InventoryReceipt>(r => receipt = r)
            .ReturnsAsync((InventoryReceipt r) => r);
        var unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(u => u.SaveChangeAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var created = await new CreateInventoryReceiptCommandHandler(repository.Object, unitOfWork.Object)
            .Handle(new CreateInventoryReceiptCommand(3, null, null, Day, 1, 1, null,
                [new InventoryReceiptLineInput(10, 1, 5, 2, null, null)]), CancellationToken.None);

        Assert.Equal(HttpStatusCode.OK, created.StatusCode);
        Assert.NotNull(receipt);
        receipt!.Confirm();
        receipt.Post(Day);
        Assert.Equal(Inventory.Domain.Enums.DocumentStatus.Posted, receipt.LifecycleStatus);
        Assert.Contains(receipt.DomainEvents, e => e is StockReceivedDomainEvent);
    }

    [Fact]
    public async Task FlowD_SalesInvoicePosted_Then_CustomerPaymentSettlesReceivable()
    {
        Receivable? receivable = null;
        var receivableRepository = new Mock<IReceivableRepository>();
        receivableRepository.Setup(r => r.ExistsForSourceDocumentAsync(Receivables.Domain.SourceDocumentType.SalesInvoice, 100, 1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        receivableRepository.Setup(r => r.AddAsync(It.IsAny<Receivable>(), It.IsAny<CancellationToken>()))
            .Callback<Receivable, CancellationToken>((r, _) =>
            {
                r.Id = 1;
                receivable = r;
            })
            .Returns(Task.CompletedTask);
        receivableRepository.Setup(r => r.GetOpenByCustomerAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(() => receivable is null ? [] : [receivable]);

        var paymentApplicationRepository = new Mock<IPaymentApplicationRepository>();
        paymentApplicationRepository.Setup(r => r.ExistsForSourceFinancialAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(u => u.SaveChangeAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        await new SalesInvoicePostedIntegrationEventHandler(receivableRepository.Object, unitOfWork.Object, InboxTestDoubles.AlwaysClaim())
            .Handle(new SalesInvoicePostedIntegrationEvent(
                100, "INV-100", 1, Day, Day, 1, 1, 1000, 1, Day, null), CancellationToken.None);

        Assert.NotNull(receivable);
        Assert.Equal(1000, receivable!.OutstandingAmount);

        await new CustomerPaymentPostedIntegrationEventHandler(
                receivableRepository.Object, paymentApplicationRepository.Object, unitOfWork.Object, InboxTestDoubles.AlwaysClaim())
            .Handle(new CustomerPaymentPostedIntegrationEvent(500, 1, 400, 1, 1, Day, 1, Day, null), CancellationToken.None);

        Assert.Equal(600, receivable.OutstandingAmount);
        paymentApplicationRepository.Verify(r => r.AddAsync(It.IsAny<PaymentApplication>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task FlowG_TenantIsolation_IsCoveredByDedicatedTests()
    {
        Assert.Contains(typeof(TenantIsolationTests).GetMethods(), m => m.Name.Contains("TenantB"));
    }

    [Fact]
    public async Task FlowE_FixedAsset_CreateAndPostDepreciationViaAccountingContracts()
    {
        var category = FixedAssetCategory.Create("Vehicles", 10, 11, 12, 60, 10);
        typeof(BaseModel).GetProperty(nameof(BaseModel.Id))!.SetValue(category, 3);
        var asset = FixedAsset.Create("Van 1", 3, new DateTime(2026, 1, 1), 10000, 1000, 60, 1, null);
        typeof(BaseModel).GetProperty(nameof(BaseModel.Id))!.SetValue(asset, 8);

        var repository = new Mock<IRepository<FixedAsset>>();
        repository.Setup(r => r.GetByFilterAsync(It.IsAny<Expression<Func<FixedAsset, bool>>>(), It.IsAny<string>()))
            .ReturnsAsync(asset);
        repository.Setup(r => r.UpdateAsync(It.IsAny<FixedAsset>())).ReturnsAsync(true);
        var categoryRepository = new Mock<IRepository<FixedAssetCategory>>();
        categoryRepository.Setup(r => r.GetByFilterAsync(It.IsAny<Expression<Func<FixedAssetCategory, bool>>>(), It.IsAny<string>()))
            .ReturnsAsync(category);
        var unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(u => u.SaveChangeAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        unitOfWork.Setup(u => u.BeginTransactionAsync()).Returns(Task.CompletedTask);
        unitOfWork.Setup(u => u.CommitAsync()).Returns(Task.CompletedTask);
        var sender = new Mock<ISender>();
        sender.Setup(s => s.Send(It.IsAny<PostAccountingEntryCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Result<PostAccountingEntryResult>(HttpStatusCode.OK, new PostAccountingEntryResult(77, "J-77"), null));

        var result = await new PostDepreciationCommandHandler(unitOfWork.Object, repository.Object, categoryRepository.Object, sender.Object)
            .Handle(new PostDepreciationCommand(8, new DateTime(2026, 2, 1), 1), CancellationToken.None);

        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        Assert.Equal(150, asset.AccumulatedDepreciation);
    }

    [Fact]
    public async Task FlowF_Budget_CreateThenVsActualFromAccountingContracts()
    {
        Budget? budget = null;
        var repository = new Mock<IRepository<Budget>>();
        repository.Setup(r => r.CreateAsync(It.IsAny<Budget>()))
            .Callback<Budget>(b =>
            {
                typeof(BaseModel).GetProperty(nameof(BaseModel.Id))!.SetValue(b, 9);
                budget = b;
            })
            .ReturnsAsync((Budget b) => b);
        repository.Setup(r => r.GetByFilterAsync(It.IsAny<Expression<Func<Budget, bool>>>(), It.IsAny<string>()))
            .ReturnsAsync(() => budget);
        var unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(u => u.SaveChangeAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        var sender = new Mock<ISender>();
        sender.Setup(s => s.Send(It.IsAny<GetAccountActivityQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Result<List<AccountActivityLineDto>>(
                HttpStatusCode.OK,
                [new AccountActivityLineDto(1, new DateTime(2026, 3, 1), 1200, 0)],
                null));

        var created = await new CreateBudgetCommandHandler(unitOfWork.Object, repository.Object)
            .Handle(new CreateBudgetCommand("FY26", 1, new DateTime(2026, 1, 1), new DateTime(2026, 12, 31), 3,
                [new BudgetLineInput(10, 5000)]), CancellationToken.None);
        Assert.Equal(HttpStatusCode.OK, created.StatusCode);

        var vsActual = await new GetBudgetVsActualQueryHandler(repository.Object, sender.Object)
            .Handle(new GetBudgetVsActualQuery(9), CancellationToken.None);
        Assert.Equal(HttpStatusCode.OK, vsActual.StatusCode);
        var line = Assert.Single(vsActual.Response!.Lines);
        Assert.Equal(5000, line.BudgetAmount);
        Assert.Equal(1200, line.ActualAmount);
        Assert.Equal(3800, line.Variance);
    }
}
