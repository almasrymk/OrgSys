namespace MasterData.Application.ReferenceTypes.Queries
{
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record GetByIdReferenceTypeQuery(long Id) : ICommand<ReferenceTypeDto> , IGetByIdQuery<Result<ReferenceTypeDto>>;

    public sealed class GetByIdQueryHandler(IRepository<MasterData.Domain.ReferenceType> _Repository, IMapper mapper) : GetCommandHandler<GetByIdReferenceTypeQuery, MasterData.Domain.ReferenceType, ReferenceTypeDto>(_Repository, mapper)
    {
        public override Expression<Func<MasterData.Domain.ReferenceType, bool>> CreateFilter(GetByIdReferenceTypeQuery request)
        {
            return e => e.Id == request.Id && e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}
