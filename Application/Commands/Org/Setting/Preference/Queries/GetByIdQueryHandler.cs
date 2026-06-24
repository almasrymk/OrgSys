namespace Application.Commands.Org.Setting.Preference.Queries
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

    public sealed record GetByIdPreferenceQuery(long Id) : ICommand<PreferenceDto> , IGetByIdQuery<Result<PreferenceDto>>;

    public sealed class GetByIdQueryHandler(IRepository<Domain.Entities.Preference> _Repository, IMapper mapper) : GetCommandHandler<GetByIdPreferenceQuery, Domain.Entities.Preference, PreferenceDto>(_Repository, mapper)
    {
        public override Expression<Func<Domain.Entities.Preference, bool>> CreateFilter(GetByIdPreferenceQuery request)
        {           
            return e => e.Id == request.Id && e.Status !=Domain.Enums.Status.Deleted && e.Hide != true;
        }
    }
}