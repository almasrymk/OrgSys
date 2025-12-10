namespace Application.Commands.Org.City.Queries
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Common.Queries;
    using AutoMapper;
    using Domain.Abstraction;
    using Entity.ModelView;
    using System.Linq.Expressions;
    using Utility;

    public sealed record GetByIdCommand(long Id) : ICommand<CityModelView>;

    public sealed class GetByIdCommandHandler(IRepository<Entity.Model.City> _Repository, IMapper mapper) : GetCommandHandler<GetByIdCommand, Entity.Model.City, CityModelView>(_Repository, mapper)
    {
        public override Expression<Func<Entity.Model.City, bool>> CreateFilter(GetByIdCommand request)
        {           
            return e => e.Id == request.Id && e.Status != Status.Deleted && e.Hide != true;
        }
    }
}