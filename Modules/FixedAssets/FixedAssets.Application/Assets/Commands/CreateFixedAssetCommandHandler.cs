namespace FixedAssets.Application.Assets.Commands;

using FixedAssets.Contracts.Assets;
using FixedAssets.Domain.Exceptions;
using System.Net;

public sealed class CreateFixedAssetCommandHandler(
    IUnitOfWork unitOfWork,
    IRepository<FixedAsset> repository,
    IRepository<FixedAssetCategory> categoryRepository)
    : ICommandHandler<CreateFixedAssetCommand, long>
{
    public async Task<Result<long>> Handle(CreateFixedAssetCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var category = await categoryRepository.GetByFilterAsync(e => e.Id == request.CategoryId, string.Empty);
            if (category is null || category.Id == 0)
                return new Result<long>(HttpStatusCode.BadRequest, 0, [new Error("Category not found.")]);

            var residual = request.ResidualValue
                ?? Math.Round(request.AcquisitionCost * category.ResidualPercent / 100, 2, MidpointRounding.AwayFromZero);
            var life = request.UsefulLifeMonths ?? category.UsefulLifeMonths;

            var asset = FixedAsset.Create(
                request.Name,
                category.Id,
                request.AcquisitionDate,
                request.AcquisitionCost,
                residual,
                life,
                request.CurrencyId,
                request.BranchId);

            await repository.CreateAsync(asset);
            if (await unitOfWork.SaveChangeAsync(cancellationToken) <= 0)
                return new Result<long>(HttpStatusCode.InternalServerError, 0, [new Error("Error")]);

            return new Result<long>(HttpStatusCode.OK, asset.Id, null);
        }
        catch (FixedAssetDomainException ex)
        {
            return new Result<long>(HttpStatusCode.BadRequest, 0, [new Error(ex.Message)]);
        }
    }
}
