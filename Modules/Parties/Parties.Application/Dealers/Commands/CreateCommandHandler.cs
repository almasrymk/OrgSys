namespace Parties.Application.Dealers.Commands
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using Accounting.Application;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Net;

    public sealed class CreateDealerCommand : DealerDto, ICommand , ICreateCommand<Result>;

    public sealed class CreateCommandHandler(
        IUnitOfWork _UnitOfWork,
        IRepository<Parties.Domain.Dealer> _Repository,
        IRepository<Account> _AccountRepository,
        IRepository<Preference> _PreferenceRepository,
        IReceivableAccountValidator _Validator,
        IMapper mapper) : CreateCommandHandler<CreateDealerCommand, Parties.Domain.Dealer>(_UnitOfWork, _Repository , mapper)
    {
        public override async Task<Result> Handle(CreateDealerCommand request, CancellationToken cancellationToken)
        {
            var dealer = mapper.Map<Parties.Domain.Dealer>(request);

            var (existingAccountId, accountToCreate, errors) = dealer.TypeId == (long)Parties.Domain.DealerType.Supplier
                ? await DealerPayableAccountProvisioning.ResolveAsync(
                    dealer, request.AccountId, request.AutoCreatePayableAccount,
                    _Validator, _PreferenceRepository, _AccountRepository, cancellationToken)
                : await DealerReceivableAccountProvisioning.ResolveAsync(
                    dealer, request.AccountId, request.AutoCreateReceivableAccount,
                    _Validator, _PreferenceRepository, _AccountRepository, cancellationToken);
            if (errors.Count > 0)
                return new Result(HttpStatusCode.BadRequest, errors);

            await _UnitOfWork.BeginTransactionAsync();
            try
            {
                if (accountToCreate is not null)
                {
                    await _AccountRepository.CreateAsync(accountToCreate);
                    await _UnitOfWork.SaveChangeAsync(cancellationToken);
                    dealer.AccountId = accountToCreate.Id;
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
