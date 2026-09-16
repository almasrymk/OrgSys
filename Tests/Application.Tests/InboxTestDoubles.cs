using Moq;
using OrgSys.SharedKernel;

namespace Application.Tests;

internal static class InboxTestDoubles
{
    public static IInboxStore AlwaysClaim()
    {
        var inbox = new Moq.Mock<IInboxStore>();
        inbox.Setup(i => i.TryClaimAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        return inbox.Object;
    }

    public static IInboxStore AlreadyClaimed()
    {
        var inbox = new Moq.Mock<IInboxStore>();
        inbox.Setup(i => i.TryClaimAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        return inbox.Object;
    }
}
