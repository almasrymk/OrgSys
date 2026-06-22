namespace Application.Commands.Org.Setting.Country.Queries
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

    public sealed record GetByIdCountryQuery(long Id) : ICommand<CountryModelView> , IGetByIdQuery<Result<CountryModelView>>;

    public sealed class GetByIdQueryHandler(IRepository<Domain.Entities.Country> _Repository, IMapper mapper) : GetCommandHandler<GetByIdCountryQuery, Domain.Entities.Country, CountryModelView>(_Repository, mapper)
    {
        public override Expression<Func<Domain.Entities.Country, bool>> CreateFilter(GetByIdCountryQuery request)
        {           
            return e => e.Id == request.Id && e.Status !=Domain.Enums.Status.Deleted && e.Hide != true;
        }
    }
}