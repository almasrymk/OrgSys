namespace FixedAssets.Application.Assets.Commands;

using Accounting.Contracts.Postings;
using FixedAssets.Contracts.Assets;
using FixedAssets.Domain.Exceptions;
using MediatR;
using System.Net;

public sealed class PostDepreciationCommandHandler(
    IUnitOfWork unitOfWork,
    IRepository<FixedAsset> repository,
    IRepository<FixedAssetCategory> categoryRepository,
    ISender sender)
    : ICommandHandler<PostDepreciationCommand, long>
{
    public async Task<Result<long>> Handle(PostDepreciationCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var asset = await repository.GetByFilterAsync(e => e.Id == request.AssetId, "Entries");
            if (asset is null || asset.Id == 0)
                return new Result<long>(HttpStatusCode.NotFound, 0, [new Error("Asset not found.")]);

            var category = await categoryRepository.GetByFilterAsync(e => e.Id == asset.CategoryId, string.Empty);
            if (category is null || category.Id == 0)
                return new Result<long>(HttpStatusCode.BadRequest, 0, [new Error("Category not found.")]);

            await unitOfWork.BeginTransactionAsync();
            try
            {
                var entry = asset.PostStraightLinePeriod(request.PeriodDate.Year, request.PeriodDate.Month, DateTime.UtcNow);
                await repository.UpdateAsync(asset);
                await unitOfWork.SaveChangeAsync(cancellationToken);

                var note = $"Depreciation {asset.Name} {request.PeriodDate:yyyy-MM}";
                var post = await sender.Send(new PostAccountingEntryCommand(
                    ReferenceTable: "depreciationentry",
                    SourceDocumentId: entry.Id == 0 ? asset.Id : entry.Id,
                    SourceDocumentTypeId: 1,
                    SourceDocumentCode: asset.Code,
                    JournalTypeId: 2,
                    Date: request.PeriodDate,
                    CreateDate: DateTime.UtcNow,
                    CreateUserId: request.CreateUserId,
                    BranchId: asset.BranchId,
                    ShiftId: null,
                    CurrencyId: asset.CurrencyId,
                    Rate: 1,
                    Note: note,
                    Lines:
                    [
                        new AccountingPostingLine(category.DepreciationExpenseAccountId, entry.Amount, 0, note),
                        new AccountingPostingLine(category.AccumulatedDepreciationAccountId, 0, entry.Amount, note)
                    ]), cancellationToken);

                if (post.Response is null)
                {
                    await unitOfWork.RollbackAsync();
                    return new Result<long>(post.StatusCode, 0, post.Errors);
                }

                entry.AttachJournal(post.Response.JournalId);
                await repository.UpdateAsync(asset);
                await unitOfWork.SaveChangeAsync(cancellationToken);
                await unitOfWork.CommitAsync();
                return new Result<long>(HttpStatusCode.OK, post.Response.JournalId, null);
            }
            catch
            {
                await unitOfWork.RollbackAsync();
                throw;
            }
        }
        catch (FixedAssetDomainException ex)
        {
            return new Result<long>(HttpStatusCode.BadRequest, 0, [new Error(ex.Message)]);
        }
    }
}
