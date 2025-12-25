namespace Application.Commands.Org.Setting.InvoiceType.Queries
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Common.Queries;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Entity.ModelView;
    using System.Linq.Expressions;

    public sealed record GetByIdInvoiceTypeQuery(long Id) : ICommand<InvoiceTypeModelView> , IGetByIdQuery<Result<InvoiceTypeModelView>>;

    public sealed class GetByIdQueryHandler(IRepository<Entity.Model.InvoiceType> _Repository, IMapper mapper) : GetCommandHandler<GetByIdInvoiceTypeQuery, Entity.Model.InvoiceType, InvoiceTypeModelView>(_Repository, mapper)
    {
        public override Expression<Func<Entity.Model.InvoiceType, bool>> CreateFilter(GetByIdInvoiceTypeQuery request)
        {           
            return e => e.Id == request.Id && e.Status !=Utility.Status.Deleted && e.Hide != true;
        }
    }
}