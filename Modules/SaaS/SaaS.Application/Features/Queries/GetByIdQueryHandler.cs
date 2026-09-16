namespace SaaS.Application.Features.Queries
{
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record GetByIdFeatureQuery(long Id) : ICommand<FeatureDto>, IGetByIdQuery<Result<FeatureDto>>;

    public sealed class GetByIdQueryHandler(IRepository<Feature> _Repository, IMapper mapper) : GetCommandHandler<GetByIdFeatureQuery, Feature, FeatureDto>(_Repository, mapper)
    {
        public override Expression<Func<Feature, bool>> CreateFilter(GetByIdFeatureQuery request)
        {
            return e => e.Id == request.Id && e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}
