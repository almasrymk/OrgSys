namespace Administration.Application.Preferences.Queries
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    
    
    using System.Linq.Expressions;

    public sealed record GetByIdPreferenceQuery(long Id) : ICommand<PreferenceDto> , IGetByIdQuery<Result<PreferenceDto>>;

    public sealed class GetByIdQueryHandler(IRepository<Administration.Domain.Preference> _Repository, IMapper mapper) : GetCommandHandler<GetByIdPreferenceQuery, Administration.Domain.Preference, PreferenceDto>(_Repository, mapper)
    {
        public override Expression<Func<Administration.Domain.Preference, bool>> CreateFilter(GetByIdPreferenceQuery request)
        {           
            return e => e.Id == request.Id && e.Status !=OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}