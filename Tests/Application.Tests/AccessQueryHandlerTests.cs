using Administration.Contracts.Access;
using Administration.Domain;
using Moq;
using OrgSys.SharedKernel;
using System.Linq.Expressions;
using System.Net;
using Xunit;

namespace Application.Tests;

public class AccessQueryHandlerTests
{
    [Fact]
    public async Task HasPermission_MatchingKey_ReturnsTrue()
    {
        var users = MockUsers(new User { Id = 1, RoleId = 9, Name = "A", UserName = "a" });
        var rolePermissions = MockList(new RolePermission { RoleId = 9, PermissionId = 4 });
        var permissions = MockList(new Permission { Id = 4, Key = "Users.View", Name = "View" });

        var handler = new Administration.Application.Access.Queries.HasPermissionQueryHandler(
            users.Object, rolePermissions.Object, permissions.Object);

        var result = await handler.Handle(new HasPermissionQuery(1, "Users.View"), CancellationToken.None);

        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        Assert.True(result.Response);
    }

    [Fact]
    public async Task HasPermission_UnknownKey_ReturnsFalse()
    {
        var users = MockUsers(new User { Id = 1, RoleId = 9, Name = "A", UserName = "a" });
        var rolePermissions = MockList(new RolePermission { RoleId = 9, PermissionId = 4 });
        var permissions = MockList(new Permission { Id = 4, Key = "Users.View", Name = "View" });

        var handler = new Administration.Application.Access.Queries.HasPermissionQueryHandler(
            users.Object, rolePermissions.Object, permissions.Object);

        var result = await handler.Handle(new HasPermissionQuery(1, "Users.Delete"), CancellationToken.None);

        Assert.False(result.Response);
    }

    [Fact]
    public async Task CanAccessBranch_UnscopedUser_AllowsAnyBranch()
    {
        var users = MockUsers(new User { Id = 1, RoleId = 9, Name = "A", UserName = "a", BranchId = null });
        var handler = new Administration.Application.Access.Queries.CanAccessBranchQueryHandler(users.Object);

        var result = await handler.Handle(new CanAccessBranchQuery(1, 22), CancellationToken.None);

        Assert.True(result.Response);
    }

    [Fact]
    public async Task CanAccessBranch_ScopedUser_RejectsOtherBranch()
    {
        var users = MockUsers(new User { Id = 1, RoleId = 9, Name = "A", UserName = "a", BranchId = 5 });
        var handler = new Administration.Application.Access.Queries.CanAccessBranchQueryHandler(users.Object);

        var result = await handler.Handle(new CanAccessBranchQuery(1, 22), CancellationToken.None);

        Assert.False(result.Response);
    }

    [Fact]
    public async Task GetCurrentUserAccess_KnownUser_ReturnsKeysAndBranch()
    {
        var users = MockUsers(new User { Id = 7, RoleId = 3, BranchId = 11, Name = "A", UserName = "a" });
        var rolePermissions = MockList(new RolePermission { RoleId = 3, PermissionId = 1 });
        var permissions = MockList(new Permission { Id = 1, Key = "Roles.View", Name = "View" });

        var handler = new Administration.Application.Access.Queries.GetCurrentUserAccessQueryHandler(
            users.Object, rolePermissions.Object, permissions.Object);

        var result = await handler.Handle(new GetCurrentUserAccessQuery(7), CancellationToken.None);

        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        Assert.Equal(7, result.Response!.UserId);
        Assert.Equal(3, result.Response.RoleId);
        Assert.Equal(11, result.Response.BranchId);
        Assert.Contains("Roles.View", result.Response.PermissionKeys);
    }

    private static Mock<IRepository<User>> MockUsers(User user)
    {
        var mock = new Mock<IRepository<User>>();
        mock.Setup(r => r.GetByFilterAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<string>()))
            .ReturnsAsync(user);
        return mock;
    }

    private static Mock<IRepository<T>> MockList<T>(params T[] items) where T : BaseModel
    {
        var mock = new Mock<IRepository<T>>();
        mock.Setup(r => r.GetListByFilterAsync(It.IsAny<Expression<Func<T, bool>>>()))
            .ReturnsAsync(items);
        return mock;
    }
}
