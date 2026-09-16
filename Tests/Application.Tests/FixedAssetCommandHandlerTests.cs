using Accounting.Contracts.Postings;
using FixedAssets.Application.Assets.Commands;
using FixedAssets.Contracts.Assets;
using FixedAssets.Domain;
using Moq;
using OrgSys.SharedKernel;
using System.Linq.Expressions;
using System.Net;
using Xunit;

namespace Application.Tests;

public class FixedAssetCommandHandlerTests
{
    [Fact]
    public async Task CreateAsset_UsesCategoryLifeAndResidualPercent()
    {
        var category = FixedAssetCategory.Create("Vehicles", 10, 11, 12, 60, 10);
        typeof(BaseModel).GetProperty(nameof(BaseModel.Id))!.SetValue(category, 3);

        var categoryRepository = new Mock<IRepository<FixedAssetCategory>>();
        categoryRepository.Setup(r => r.GetByFilterAsync(It.IsAny<Expression<Func<FixedAssetCategory, bool>>>(), It.IsAny<string>()))
            .ReturnsAsync(category);
        var repository = new Mock<IRepository<FixedAsset>>();
        repository.Setup(r => r.CreateAsync(It.IsAny<FixedAsset>())).ReturnsAsync((FixedAsset a) => a);
        var unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(u => u.SaveChangeAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var handler = new CreateFixedAssetCommandHandler(unitOfWork.Object, repository.Object, categoryRepository.Object);
        var result = await handler.Handle(
            new CreateFixedAssetCommand("Van 1", 3, new DateTime(2026, 1, 1), 10000, null, null, 1, null),
            CancellationToken.None);

        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        repository.Verify(r => r.CreateAsync(It.Is<FixedAsset>(a =>
            a.UsefulLifeMonths == 60 && a.ResidualValue == 1000 && a.MonthlyDepreciationAmount() == 150)), Times.Once);
    }

    [Fact]
    public async Task PostDepreciation_PostsExpenseAndAccumulatedViaAccountingContracts()
    {
        var category = FixedAssetCategory.Create("Vehicles", 10, 11, 12, 60, 10);
        typeof(BaseModel).GetProperty(nameof(BaseModel.Id))!.SetValue(category, 3);
        var asset = FixedAsset.Create("Van 1", 3, new DateTime(2026, 1, 1), 10000, 1000, 60, 1, null);
        typeof(BaseModel).GetProperty(nameof(BaseModel.Id))!.SetValue(asset, 8);

        var repository = new Mock<IRepository<FixedAsset>>();
        repository.Setup(r => r.GetByFilterAsync(It.IsAny<Expression<Func<FixedAsset, bool>>>(), It.IsAny<string>()))
            .ReturnsAsync(asset);
        var categoryRepository = new Mock<IRepository<FixedAssetCategory>>();
        categoryRepository.Setup(r => r.GetByFilterAsync(It.IsAny<Expression<Func<FixedAssetCategory, bool>>>(), It.IsAny<string>()))
            .ReturnsAsync(category);
        var unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(u => u.SaveChangeAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        unitOfWork.Setup(u => u.BeginTransactionAsync()).Returns(Task.CompletedTask);
        unitOfWork.Setup(u => u.CommitAsync()).Returns(Task.CompletedTask);
        unitOfWork.Setup(u => u.RollbackAsync()).Returns(Task.CompletedTask);
        repository.Setup(r => r.UpdateAsync(It.IsAny<FixedAsset>())).ReturnsAsync(true);
        var sender = new Mock<MediatR.ISender>();
        sender.Setup(s => s.Send(It.IsAny<PostAccountingEntryCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Result<PostAccountingEntryResult>(HttpStatusCode.OK, new PostAccountingEntryResult(77, "J-77"), null));

        var handler = new PostDepreciationCommandHandler(unitOfWork.Object, repository.Object, categoryRepository.Object, sender.Object);
        var result = await handler.Handle(
            new PostDepreciationCommand(8, new DateTime(2026, 2, 1), 1),
            CancellationToken.None);

        Assert.Equal(77, result.Response);
        Assert.Equal(150, asset.AccumulatedDepreciation);
        sender.Verify(s => s.Send(It.Is<PostAccountingEntryCommand>(c =>
            c.Lines.Any(l => l.AccountId == 12 && l.Debit == 150) &&
            c.Lines.Any(l => l.AccountId == 11 && l.Credit == 150)), It.IsAny<CancellationToken>()), Times.Once);
    }
}
