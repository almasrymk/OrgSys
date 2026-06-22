namespace Application.Commands.Org.Financials.Financial.Queries
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

    public sealed record GetByIdFinancialQuery(long Id) : ICommand<FinancialModelView> , IGetByIdQuery<Result<FinancialModelView>>;

    public sealed class GetByIdQueryHandler(IRepository<Domain.Entities.Financial> _Repository, IMapper mapper) :
        GetCommandHandler<GetByIdFinancialQuery, Domain.Entities.Financial, FinancialModelView>(_Repository, mapper)
    {
        public override string CreateInclude()
        {
            return "FinancialInvoices,FinancialInvoices.Invoice";
        }

        public override Expression<Func<Domain.Entities.Financial, bool>> CreateFilter(GetByIdFinancialQuery request)
        {           
            return e => e.Id == request.Id && e.Status !=Domain.Enums.Status.Deleted && e.Hide != true;
        }
    }
}