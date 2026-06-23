namespace Application.Commands.Org.Setting.BankBranch.Queries
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Common.Queries;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Application.DTOs;
    using System.Linq.Expressions;

    public sealed record GetByIdBankBranchQuery(long Id) : ICommand<BankBranchDto> , IGetByIdQuery<Result<BankBranchDto>>;

    public sealed class GetByIdQueryHandler(IRepository<Domain.Entities.BankBranch> _Repository, IMapper mapper) : GetCommandHandler<GetByIdBankBranchQuery, Domain.Entities.BankBranch, BankBranchDto>(_Repository, mapper)
    {
        public override Expression<Func<Domain.Entities.BankBranch, bool>> CreateFilter(GetByIdBankBranchQuery request)
        {           
            return e => e.Id == request.Id && e.Status !=Domain.Enums.Status.Deleted && e.Hide != true;
        }
    }
}