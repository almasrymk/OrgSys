namespace Administration.Application.Access.Queries;

using Administration.Contracts.Access;
using OrgSys.SharedKernel;
using System.Net;

public sealed class HasPermissionQueryHandler(
    IRepository<User> users,
    IRepository<RolePermission> rolePermissions,
    IRepository<Permission> permissions) : IQueryHandler<HasPermissionQuery, bool>
{
    public async Task<Result<bool>> Handle(HasPermissionQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.PermissionKey))
            return new Result<bool>(HttpStatusCode.OK, false, null);

        var keys = await LoadPermissionKeys(request.UserId, cancellationToken);
        var has = keys.Any(k => string.Equals(k, request.PermissionKey, StringComparison.OrdinalIgnoreCase));
        return new Result<bool>(HttpStatusCode.OK, has, null);
    }

    internal static async Task<IReadOnlyList<string>> LoadPermissionKeys(
        long userId,
        CancellationToken cancellationToken,
        IRepository<User> users,
        IRepository<RolePermission> rolePermissions,
        IRepository<Permission> permissions)
    {
        var user = await users.GetByFilterAsync(
            e => e.Id == userId && e.Status != Status.Deleted && e.Hide != true,
            string.Empty);
        if (user is null || user.Id == 0)
            return [];

        var rolePermissionRows = await rolePermissions.GetListByFilterAsync(e => e.RoleId == user.RoleId) ?? [];
        var permissionIds = rolePermissionRows.Select(e => e.PermissionId).Distinct().ToList();
        if (permissionIds.Count == 0)
            return [];

        var permissionRows = await permissions.GetListByFilterAsync(e => permissionIds.Contains(e.Id)) ?? [];
        return permissionRows
            .Select(e => e.Key)
            .Where(k => !string.IsNullOrWhiteSpace(k))
            .Cast<string>()
            .ToList();
    }

    private Task<IReadOnlyList<string>> LoadPermissionKeys(long userId, CancellationToken cancellationToken) =>
        LoadPermissionKeys(userId, cancellationToken, users, rolePermissions, permissions);
}

public sealed class GetCurrentUserAccessQueryHandler(
    IRepository<User> users,
    IRepository<RolePermission> rolePermissions,
    IRepository<Permission> permissions) : IQueryHandler<GetCurrentUserAccessQuery, UserAccessDto>
{
    public async Task<Result<UserAccessDto>> Handle(GetCurrentUserAccessQuery request, CancellationToken cancellationToken)
    {
        var user = await users.GetByFilterAsync(
            e => e.Id == request.UserId && e.Status != Status.Deleted && e.Hide != true,
            string.Empty);
        if (user is null || user.Id == 0)
            return new Result<UserAccessDto>(HttpStatusCode.NotFound, null, [new Error("User not found")]);

        var keys = await HasPermissionQueryHandler.LoadPermissionKeys(
            request.UserId, cancellationToken, users, rolePermissions, permissions);

        return new Result<UserAccessDto>(
            HttpStatusCode.OK,
            new UserAccessDto(user.Id, user.RoleId, user.BranchId, keys),
            null);
    }
}

public sealed class CanAccessBranchQueryHandler(IRepository<User> users) : IQueryHandler<CanAccessBranchQuery, bool>
{
    public async Task<Result<bool>> Handle(CanAccessBranchQuery request, CancellationToken cancellationToken)
    {
        var user = await users.GetByFilterAsync(
            e => e.Id == request.UserId && e.Status != Status.Deleted && e.Hide != true,
            string.Empty);
        if (user is null || user.Id == 0)
            return new Result<bool>(HttpStatusCode.OK, false, null);

        var allowed = user.BranchId is null or 0 || user.BranchId == request.BranchId;
        return new Result<bool>(HttpStatusCode.OK, allowed, null);
    }
}
