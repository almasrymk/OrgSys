namespace Application.Commands.Org.Country.Queries
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

    public sealed record GetByIdCountryQuery(long Id) : ICommand<CountryModelView> , IGetByIdQuery<Result<CountryModelView>>;

    public sealed class GetByIdQueryHandler(IRepository<Entity.Model.Country> _Repository, IMapper mapper) : GetCommandHandler<GetByIdCountryQuery, Entity.Model.Country, CountryModelView>(_Repository, mapper)
    {
        public override Expression<Func<Entity.Model.Country, bool>> CreateFilter(GetByIdCountryQuery request)
        {           
            return e => e.Id == request.Id && e.Status !=Utility.Status.Deleted && e.Hide != true;
        }
    }
}