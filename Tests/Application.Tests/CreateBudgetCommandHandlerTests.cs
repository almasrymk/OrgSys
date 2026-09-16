using Budgeting.Application.Budgets.Commands;
using Budgeting.Contracts.Budgets;
using Budgeting.Domain;
using Moq;
using OrgSys.SharedKernel;
using System.Net;
using Xunit;

namespace Application.Tests;

public class CreateBudgetCommandHandlerTests
{
    [Fact]
    public async Task Create_ValidBudget_PersistsWithLines()
    {
        var repository = new Mock<IRepository<Budget>>();
        repository.Setup(r => r.CreateAsync(It.IsAny<Budget>())).ReturnsAsync((Budget b) => b);
        var unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(u => u.SaveChangeAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var handler = new CreateBudgetCommandHandler(unitOfWork.Object, repository.Object);
        var result = await handler.Handle(
            new CreateBudgetCommand(
                "FY26 OPEX",
                1,
                new DateTime(2026, 1, 1),
                new DateTime(2026, 12, 31),
                3,
                [new BudgetLineInput(10, 5000)]),
            CancellationToken.None);

        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        repository.Verify(r => r.CreateAsync(It.Is<Budget>(b => b.Name == "FY26 OPEX" && b.Lines.Count == 1)), Times.Once);
    }
}
