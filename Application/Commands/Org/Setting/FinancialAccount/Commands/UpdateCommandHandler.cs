namespace Application.Commands.Org.Setting.FinancialAccount.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Common.Services;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Entities;
    using Domain.Enums;
    using Domain.Shared;
    using Application.DTOs;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;

    public sealed class UpdateFinancialAccountCommand : FinancialAccountDto, ICommand, IUpdateCommand<Result>;

    public sealed class UpdateCommandHandler(
        IUnitOfWork _UnitOfWork, IRepository<Domain.Entities.FinancialAccount> _Repository,
        IRepository<CashBox> _CashBoxRepository, IRepository<BankAccount> _BankAccountRepository,
        IMapper mapper, IServiceProvider _provider,
        IReceivableAccountValidator accountValidator)
        : UpdateCommandHandler<UpdateFinancialAccountCommand, Domain.Entities.FinancialAccount>(_UnitOfWork, _Repository, mapper, _provider)
    {
        // Same guards SaveFinancialAccountCommandHandler used to run before its dual-write.
        public override async Task<Result> Handle(UpdateFinancialAccountCommand request, CancellationToken cancellationToken)
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

        public override async Task<bool> SaveDetials(UpdateFinancialAccountCommand request)
        {
            if (request.FinancialAccountType == FinancialAccountType.CashBox)
            {
                var detail = await _CashBoxRepository.GetByFilterAsync(e => e.FinancialAccountId == request.Id, string.Empty)
                    ?? new CashBox { FinancialAccountId = request.Id };
                detail.Name = request.Name;
                detail.AccountId = request.AccountId;
                detail.BranchId = request.BranchId;
                detail.KeeperUserId = request.KeeperUserId;
                return await UpdateDetails<CashBox>([detail]);
            }
            else
            {
                var detail = await _BankAccountRepository.GetByFilterAsync(e => e.FinancialAccountId == request.Id, string.Empty)
                    ?? new BankAccount { FinancialAccountId = request.Id };
                detail.Name = request.Name;
                detail.AccountId = request.AccountId;
                detail.BankId = request.BankId ?? 0;
                detail.BankBranchd = request.BankBranchId;
                detail.AccountNumber = request.AccountNumber;
                detail.IBAN = request.IBAN;
                detail.SwiftCode = request.SwiftCode;
                detail.BranchId = request.BranchId;
                return await UpdateDetails<BankAccount>([detail]);
            }
        }
    }
}
