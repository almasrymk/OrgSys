namespace FixedAssets.Application.Assets.Queries;

using FixedAssets.Contracts.Assets;
using System.Net;

public sealed class GetFixedAssetQueryHandler(IRepository<FixedAsset> repository)
    : IQueryHandler<GetFixedAssetQuery, FixedAssetDto>
{
    public async Task<Result<FixedAssetDto>> Handle(GetFixedAssetQuery request, CancellationToken cancellationToken)
    {
        var asset = await repository.GetByFilterAsync(e => e.Id == request.AssetId, string.Empty);
        if (asset is null || asset.Id == 0)
            return new Result<FixedAssetDto>(HttpStatusCode.NotFound, null, [new Error("Asset not found.")]);

        return new Result<FixedAssetDto>(
            HttpStatusCode.OK,
            new FixedAssetDto(
                asset.Id,
                asset.Name,
                asset.CategoryId,
                asset.AcquisitionDate,
                asset.AcquisitionCost,
                asset.ResidualValue,
                asset.UsefulLifeMonths,
                asset.AccumulatedDepreciation,
                asset.LifecycleStatus.ToString()),
            null);
    }
}
