namespace FixedAssets.Application.Assets.Commands;

using Accounting.Contracts.Accounts;
using FixedAssets.Contracts.Assets;
using FixedAssets.Domain.Exceptions;
using MediatR;
using System.Net;

public sealed class CreateFixedAssetCategoryCommandHandler(
    IUnitOfWork unitOfWork,
    IRepository<FixedAssetCategory> repository,
    ISender sender)
    : ICommandHandler<CreateFixedAssetCategoryCommand, long>
{
    public async Task<Result<long>> Handle(CreateFixedAssetCategoryCommand request, CancellationToken cancellationToken)
    {
        try
        {
            foreach (var accountId in new[]
            {
                request.AssetAccountId,
                request.AccumulatedDepreciationAccountId,
                request.DepreciationExpenseAccountId
            })
            {
                var account = (await sender.Send(new GetAccountQuery(accountId), cancellationToken)).Response;
                if (account is null || !account.IsPostable)
                    return new Result<long>(HttpStatusCode.BadRequest, 0, [new Error("Category accounts must be existing postable GL accounts.")]);
            }

            var category = FixedAssetCategory.Create(
                request.Name,
                request.AssetAccountId,
                request.AccumulatedDepreciationAccountId,
                request.DepreciationExpenseAccountId,
                request.UsefulLifeMonths,
                request.ResidualPercent);

            await repository.CreateAsync(category);
            if (await unitOfWork.SaveChangeAsync(cancellationToken) <= 0)
                return new Result<long>(HttpStatusCode.InternalServerError, 0, [new Error("Error")]);

            return new Result<long>(HttpStatusCode.OK, category.Id, null);
        }
        catch (FixedAssetDomainException ex)
        {
            return new Result<long>(HttpStatusCode.BadRequest, 0, [new Error(ex.Message)]);
        }
    }
}
