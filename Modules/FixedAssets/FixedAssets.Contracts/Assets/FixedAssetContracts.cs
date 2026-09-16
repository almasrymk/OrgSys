namespace FixedAssets.Contracts.Assets;

using OrgSys.SharedKernel;

public sealed record CreateFixedAssetCategoryCommand(
    string Name,
    long AssetAccountId,
    long AccumulatedDepreciationAccountId,
    long DepreciationExpenseAccountId,
    int UsefulLifeMonths,
    decimal ResidualPercent) : ICommand<long>;

public sealed record CreateFixedAssetCommand(
    string Name,
    long CategoryId,
    DateTime AcquisitionDate,
    decimal AcquisitionCost,
    decimal? ResidualValue,
    int? UsefulLifeMonths,
    long CurrencyId,
    long? BranchId) : ICommand<long>;

public sealed record PostDepreciationCommand(
    long AssetId,
    DateTime PeriodDate,
    long CreateUserId) : ICommand<long>;

public sealed record FixedAssetDto(
    long Id,
    string? Name,
    long CategoryId,
    DateTime AcquisitionDate,
    decimal AcquisitionCost,
    decimal ResidualValue,
    int UsefulLifeMonths,
    decimal AccumulatedDepreciation,
    string LifecycleStatus);

public sealed record GetFixedAssetQuery(long AssetId) : IQuery<FixedAssetDto>;
