using Accounting.Application.Journals.Commands;
using Moq;
using System.Net;
using Xunit;

namespace Application.Tests;

public class JournalDeleteCommandHandlerTests
{
    private static (DeleteCommandHandler handler, Mock<IRepository<Journal>> repository, Mock<IUnitOfWork> unitOfWork) BuildHandler(Journal? existingJournal)
    {
        var repository = new Mock<IRepository<Journal>>();
        repository.Setup(r => r.GetByFilterAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Journal, bool>>>(), It.IsAny<string>()))
            .ReturnsAsync(existingJournal);
        repository.Setup(r => r.ShiftDeleteAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Journal, bool>>>())).ReturnsAsync(true);

        var unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(u => u.SaveChangeAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var handler = new DeleteCommandHandler(unitOfWork.Object, repository.Object, Mock.Of<IServiceProvider>());

        return (handler, repository, unitOfWork);
    }

    [Fact]
    public async Task Handle_PostedJournal_RejectsDeleteAndDoesNotShiftDelete()
    {
        var existing = new Journal { Id = 1, Posted = true, RefranceTable = null };
        var (handler, repository, unitOfWork) = BuildHandler(existing);

        var result = await handler.Handle(new DeleteJournalCommand(1), CancellationToken.None);

        Assert.Equal(HttpStatusCode.InternalServerError, result.StatusCode);
        Assert.Contains("posted journal entry cannot be deleted", result.Errors!.Select(e => e.MessageError).First(), StringComparison.OrdinalIgnoreCase);
        repository.Verify(r => r.ShiftDeleteAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Journal, bool>>>()), Times.Never);
    }

    [Fact]
    public async Task Handle_DraftJournal_DeletesSuccessfully()
    {
        var existing = new Journal { Id = 1, Posted = false, RefranceTable = null, JournalItems = [] };
        var (handler, repository, unitOfWork) = BuildHandler(existing);

        var result = await handler.Handle(new DeleteJournalCommand(1), CancellationToken.None);

        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        repository.Verify(r => r.ShiftDeleteAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Journal, bool>>>()), Times.Once);
    }
}
