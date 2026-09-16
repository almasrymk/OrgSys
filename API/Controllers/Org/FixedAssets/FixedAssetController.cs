using FixedAssets.Contracts.Assets;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Org.FixedAssets;

[Authorize]
[ApiController]
[Route("[controller]")]
public class FixedAssetController(ISender sender) : ControllerBase
{
    [HttpPost("Category/Create")]
    public Task<Result<long>> CreateCategory([FromBody] CreateFixedAssetCategoryCommand command, CancellationToken cancellationToken) =>
        sender.Send(command, cancellationToken);

    [HttpPost("Create")]
    public Task<Result<long>> Create([FromBody] CreateFixedAssetCommand command, CancellationToken cancellationToken) =>
        sender.Send(command, cancellationToken);

    [HttpPost("Depreciation/Post")]
    public Task<Result<long>> PostDepreciation([FromBody] PostDepreciationCommand command, CancellationToken cancellationToken) =>
        sender.Send(command, cancellationToken);

    [HttpGet("Get")]
    public Task<Result<FixedAssetDto>> Get(long assetId, CancellationToken cancellationToken) =>
        sender.Send(new GetFixedAssetQuery(assetId), cancellationToken);
}
