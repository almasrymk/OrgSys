namespace Advances.Application.Custodies.Commands;

using Advances.Domain.Exceptions;
using System.Net;

public sealed record CreateCustodyCommand(
    long HolderId,
    string Purpose,
    long CurrencyId,
    decimal Rate,
    decimal IssuedAmount,
    DateTime? DueDate,
    long CreateUserId,
    DateTime CreateDate,
    long? BranchId,
    string? Notes,
    string? Code) : ICommand<long>;

public sealed class CreateCustodyCommandHandler(IRepository<Custody> repository, IUnitOfWork unitOfWork)
    : ICommandHandler<CreateCustodyCommand, long>
{
    public async Task<Result<long>> Handle(CreateCustodyCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var custody = Custody.Create(
                request.HolderId,
                request.Purpose,
                request.CurrencyId,
                request.Rate,
                request.IssuedAmount,
                request.DueDate,
                request.CreateUserId,
                request.CreateDate == default ? DateTime.Now : request.CreateDate,
                request.BranchId,
                request.Notes);

            custody.Code = request.Code;

            await repository.CreateAsync(custody);
            await unitOfWork.SaveChangeAsync(cancellationToken);
            return new Result<long>(HttpStatusCode.OK, custody.Id, null);
        }
        catch (CustodyDomainException ex)
        {
            return new Result<long>(HttpStatusCode.BadRequest, 0, [new Error(ex.Message)]);
        }
    }
}
