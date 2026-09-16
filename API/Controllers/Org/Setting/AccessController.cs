using Administration.Contracts.Access;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Org.Setting;

[Authorize]
[ApiController]
[Route("[controller]")]
public class AccessController(ISender sender) : ControllerBase
{
    [HttpGet("HasPermission")]
    public Task<Result<bool>> HasPermission(long userId, string permissionKey, CancellationToken cancellationToken) =>
        sender.Send(new HasPermissionQuery(userId, permissionKey), cancellationToken);

    [HttpGet("GetCurrentUserAccess")]
    public Task<Result<UserAccessDto>> GetCurrentUserAccess(long userId, CancellationToken cancellationToken) =>
        sender.Send(new GetCurrentUserAccessQuery(userId), cancellationToken);

    [HttpGet("CanAccessBranch")]
    public Task<Result<bool>> CanAccessBranch(long userId, long branchId, CancellationToken cancellationToken) =>
        sender.Send(new CanAccessBranchQuery(userId, branchId), cancellationToken);
}
