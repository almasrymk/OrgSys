namespace Application.Commands.Org.Setting.PaymentType.Queries
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

    public sealed record GetByIdPaymentTypeQuery(long Id) : ICommand<PaymentTypeModelView> , IGetByIdQuery<Result<PaymentTypeModelView>>;

    public sealed class GetByIdQueryHandler(IRepository<Entity.Model.PaymentType> _Repository, IMapper mapper) : GetCommandHandler<GetByIdPaymentTypeQuery, Entity.Model.PaymentType, PaymentTypeModelView>(_Repository, mapper)
    {
        public override Expression<Func<Entity.Model.PaymentType, bool>> CreateFilter(GetByIdPaymentTypeQuery request)
        {           
            return e => e.Id == request.Id && e.Status !=Utility.Status.Deleted && e.Hide != true;
        }
    }
}