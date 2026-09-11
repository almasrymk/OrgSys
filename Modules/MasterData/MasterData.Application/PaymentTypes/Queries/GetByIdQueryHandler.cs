namespace MasterData.Application.PaymentTypes.Queries
{
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record GetByIdPaymentTypeQuery(long Id) : ICommand<PaymentTypeDto> , IGetByIdQuery<Result<PaymentTypeDto>>;

    public sealed class GetByIdQueryHandler(IRepository<MasterData.Domain.PaymentType> _Repository, IMapper mapper) : GetCommandHandler<GetByIdPaymentTypeQuery, MasterData.Domain.PaymentType, PaymentTypeDto>(_Repository, mapper)
    {
        public override Expression<Func<MasterData.Domain.PaymentType, bool>> CreateFilter(GetByIdPaymentTypeQuery request)
        {           
            return e => e.Id == request.Id && e.Status !=OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}
