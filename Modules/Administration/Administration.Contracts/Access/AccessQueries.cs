namespace Administration.Contracts.Access;

using OrgSys.SharedKernel;

public sealed record UserAccessDto(
    long UserId,
    long RoleId,
    long? BranchId,
    IReadOnlyList<string> PermissionKeys);

public sealed record HasPermissionQuery(long UserId, string PermissionKey) : IQuery<bool>;

public sealed record GetCurrentUserAccessQuery(long UserId) : IQuery<UserAccessDto>;

public sealed record CanAccessBranchQuery(long UserId, long BranchId) : IQuery<bool>;
