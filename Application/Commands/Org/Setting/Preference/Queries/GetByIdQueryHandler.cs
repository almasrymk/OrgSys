namespace Application.Commands.Org.Setting.Preference.Queries
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using Domain.Abstraction;
    using Application.DTOs;
    using System.Linq.Expressions;

    public sealed record GetByIdPreferenceQuery(long Id) : ICommand<PreferenceDto> , IGetByIdQuery<Result<PreferenceDto>>;

    public sealed class GetByIdQueryHandler(IRepository<Domain.Entities.Preference> _Repository, IMapper mapper) : GetCommandHandler<GetByIdPreferenceQuery, Domain.Entities.Preference, PreferenceDto>(_Repository, mapper)
    {
        public override Expression<Func<Domain.Entities.Preference, bool>> CreateFilter(GetByIdPreferenceQuery request)
        {           
            return e => e.Id == request.Id && e.Status !=OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}