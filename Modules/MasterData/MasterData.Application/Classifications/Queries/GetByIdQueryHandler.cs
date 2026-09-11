namespace MasterData.Application.Classifications.Queries
{
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record GetByIdClassificationQuery(long Id) : ICommand<ClassificationDto> , IGetByIdQuery<Result<ClassificationDto>>;

    public sealed class GetByIdQueryHandler(IRepository<MasterData.Domain.Classification> _Repository, IMapper mapper) : GetCommandHandler<GetByIdClassificationQuery, MasterData.Domain.Classification, ClassificationDto>(_Repository, mapper)
    {
        public override Expression<Func<MasterData.Domain.Classification, bool>> CreateFilter(GetByIdClassificationQuery request)
        {           
            return e => e.Id == request.Id && e.Status !=OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}
