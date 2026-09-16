namespace Parties.Application.Dealers.Commands
{
    using Accounting.Contracts.Accounts;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using MediatR;
    using System.Net;

    public sealed class CreateDealerCommand : DealerDto, ICommand , ICreateCommand<Result>;

    public sealed class CreateCommandHandler(
        IUnitOfWork _UnitOfWork,
        IRepository<Parties.Domain.Dealer> _Repository,
        IReceivableAccountValidator _Validator,
        ISender sender,
        IMapper mapper) : CreateCommandHandler<CreateDealerCommand, Parties.Domain.Dealer>(_UnitOfWork, _Repository , mapper)
    {
        public override async Task<Result> Handle(CreateDealerCommand request, CancellationToken cancellationToken)
        {
            var dealer = mapper.Map<Parties.Domain.Dealer>(request);

            var (existingAccountId, provisionParentAccountId, errors) = dealer.TypeId == (long)Parties.Domain.DealerType.Supplier
                ? await DealerPayableAccountProvisioning.ResolveAsync(
                    dealer, request.AccountId, request.AutoCreatePayableAccount,
                    _Validator, sender, cancellationToken)
                : await DealerReceivableAccountProvisioning.ResolveAsync(
                    dealer, request.AccountId, request.AutoCreateReceivableAccount,
                    _Validator, sender, cancellationToken);
            if (errors.Count > 0)
                return new Result(HttpStatusCode.BadRequest, errors);

            await _UnitOfWork.BeginTransactionAsync();
            try
            {
                if (provisionParentAccountId is > 0)
                {
                    var provisionResult = await sender.Send(new ProvisionSubAccountCommand(provisionParentAccountId.Value, dealer.Name), cancellationToken);
                    if (provisionResult.Response is null)
                    {
                        await _UnitOfWork.RollbackAsync();
                        return new Result(HttpStatusCode.BadRequest, provisionResult.Errors);
                    }

                    dealer.AccountId = provisionResult.Response.Id;
                }
                else
                {
                    dealer.AccountId = existingAccountId;
                }

                await _Repository.CreateAsync(dealer);
                if (await _UnitOfWork.SaveChangeAsync(cancellationToken) <= 0)
                {
                    await _UnitOfWork.RollbackAsync();
                    return new Result(HttpStatusCode.InternalServerError, [new Error("Error")]);
                }

                await _UnitOfWork.CommitAsync();
                return new Result(HttpStatusCode.OK, null);
            }
            catch (Exception ex)
            {
                await _UnitOfWork.RollbackAsync();
                return new Result(HttpStatusCode.InternalServerError, [new Error(ex.Message)]);
            }
        }
    }
}
