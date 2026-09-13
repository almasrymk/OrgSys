namespace Treasury.Application.FinancialAccounts.Commands
{
    using Accounting.Contracts.Accounts;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;

    public sealed class CreateFinancialAccountCommand : FinancialAccountDto, ICommand, ICreateCommand<Result>;

    public sealed class CreateCommandHandler(
        IUnitOfWork _UnitOfWork, IRepository<Treasury.Domain.FinancialAccount> _Repository, IMapper mapper,
        IReceivableAccountValidator accountValidator)
        : CreateCommandHandler<CreateFinancialAccountCommand, Treasury.Domain.FinancialAccount>(_UnitOfWork, _Repository, mapper)
    {
        // Same guards SaveFinancialAccountCommandHandler used to run before its dual-write — a blank
        // name or an AccountId pointing at a missing/inactive/group (non-postable) GL account would
        // otherwise sail through, since the generic base has no notion of either check.
        public override async Task<Result> Handle(CreateFinancialAccountCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                return new Result(HttpStatusCode.BadRequest, [new Error("Financial account name is required.")]);
            if (request.AccountId is > 0)
            {
                var (_, errors) = await accountValidator.ValidateAccountAsync(request.AccountId.Value, cancellationToken);
                if (errors.Count > 0)
                    return new Result(HttpStatusCode.BadRequest, errors);
            }
            return await base.Handle(request, cancellationToken);
        }
    }
}
