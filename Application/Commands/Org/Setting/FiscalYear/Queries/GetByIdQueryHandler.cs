namespace Application.Commands.Org.Setting.FiscalYear.Queries
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

    public sealed record GetByIdFiscalYearQuery(long Id) : ICommand<FiscalYearDto>, IGetByIdQuery<Result<FiscalYearDto>>;

    public sealed class GetByIdQueryHandler(IRepository<Domain.Entities.FiscalYear> _Repository, IMapper mapper) : GetCommandHandler<GetByIdFiscalYearQuery, Domain.Entities.FiscalYear, FiscalYearDto>(_Repository, mapper)
    {
        public override Expression<Func<Domain.Entities.FiscalYear, bool>> CreateFilter(GetByIdFiscalYearQuery request)
        {
            return e => e.Id == request.Id && e.Status != Domain.Enums.Status.Deleted && e.Hide != true;
        }

        public override string CreateInclude()
        {
            return "Periods";
        }
    }
}
