namespace Application.Commands.Org.Setting.InvoiceType.Queries
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

    public sealed record GetByIdInvoiceTypeQuery(long Id) : ICommand<InvoiceTypeModelView> , IGetByIdQuery<Result<InvoiceTypeModelView>>;

    public sealed class GetByIdQueryHandler(IRepository<Domain.Entities.InvoiceType> _Repository, IMapper mapper) : GetCommandHandler<GetByIdInvoiceTypeQuery, Domain.Entities.InvoiceType, InvoiceTypeModelView>(_Repository, mapper)
    {
        public override Expression<Func<Domain.Entities.InvoiceType, bool>> CreateFilter(GetByIdInvoiceTypeQuery request)
        {           
            return e => e.Id == request.Id && e.Status !=Domain.Enums.Status.Deleted && e.Hide != true;
        }
    }
}