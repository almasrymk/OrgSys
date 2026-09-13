using Accounting.Application.Journals.Commands;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Net;
using Xunit;

namespace Application.Tests;

public class JournalCancelCommandHandlerTests
{
    private static IMapper BuildMapper()
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddAutoMapper(cfg => { cfg.AddProfile<Accounting.Application.MappingProfile>(); });
        return services.BuildServiceProvider().GetRequiredService<IMapper>();
    }

    private static (CancelJournalCommandHandler handler, Mock<IRepository<Journal>> repository, Mock<IUnitOfWork> unitOfWork) BuildHandler(Journal? existing)
    {
        var repository = new Mock<IRepository<Journal>>();
        repository.Setup(r => r.GetByFilterAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Journal, bool>>>(), It.IsAny<string>()))
            .ReturnsAsync(existing);

        var unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(u => u.SaveChangeAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var handler = new CancelJournalCommandHandler(unitOfWork.Object, repository.Object, BuildMapper(), Mock.Of<IServiceProvider>());

        return (handler, repository, unitOfWork);
    }

    [Fact]
    public async Task Handle_DraftJournal_CancelsSuccessfully()
    {
        var journal = new Journal { Id = 1, Posted = false, Status = Status.New };
        var (handler, repository, unitOfWork) = BuildHandler(journal);

        var result = await handler.Handle(new CancelJournalCommand(1), CancellationToken.None);

        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        Assert.Equal(Status.Cancel, journal.Status);
    }

    [Fact]
    public async Task Handle_PostedJournal_RejectsCancel_MustUseReverseInstead()
    {
        var journal = new Journal { Id = 1, Posted = true, Status = Status.New };
        var (handler, repository, unitOfWork) = BuildHandler(journal);

        var result = await handler.Handle(new CancelJournalCommand(1), CancellationToken.None);

        Assert.Equal(HttpStatusCode.Forbidden, result.StatusCode);
        Assert.Contains("Reverse", result.Errors!.Select(e => e.MessageError).First());
        Assert.Equal(Status.New, journal.Status);
        unitOfWork.Verify(u => u.SaveChangeAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}
