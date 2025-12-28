namespace Application.Commands.Org.Setting.Property.Queries
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

    public sealed record GetByIdPropertyQuery(long Id) : ICommand<PropertyModelView> , IGetByIdQuery<Result<PropertyModelView>>;

    public sealed class GetByIdQueryHandler(IRepository<Entity.Model.Property> _Repository, IMapper mapper) : GetCommandHandler<GetByIdPropertyQuery, Entity.Model.Property, PropertyModelView>(_Repository, mapper)
    {
        public override Expression<Func<Entity.Model.Property, bool>> CreateFilter(GetByIdPropertyQuery request)
        {           
            return e => e.Id == request.Id && e.Status !=Utility.Status.Deleted && e.Hide != true;
        }
    }
}