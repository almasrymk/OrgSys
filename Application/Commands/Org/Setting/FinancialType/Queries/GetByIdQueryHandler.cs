namespace Application.Commands.Org.Setting.FinancialType.Queries
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

    public sealed record GetByIdFinancialTypeQuery(long Id) : ICommand<FinancialTypeDto> , IGetByIdQuery<Result<FinancialTypeDto>>;

    public sealed class GetByIdQueryHandler(IRepository<Domain.Entities.FinancialType> _Repository, IMapper mapper) : 
        GetCommandHandler<GetByIdFinancialTypeQuery, Domain.Entities.FinancialType, FinancialTypeDto>(_Repository, mapper)
    {
        public override string CreateInclude()
        {
            return "";
        }

        public override Expression<Func<Domain.Entities.FinancialType, bool>> CreateFilter(GetByIdFinancialTypeQuery request)
        {           
            return e => e.Id == request.Id && e.Status != Domain.Enums.Status.Deleted && e.Hide != true;
        }
    }
}