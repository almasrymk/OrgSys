namespace Application.Commands.Org.Setting.Property.Queries
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

    public sealed record GetByIdPropertyQuery(long Id) : ICommand<PropertyDto> , IGetByIdQuery<Result<PropertyDto>>;

    public sealed class GetByIdQueryHandler(IRepository<Domain.Entities.Property> _Repository, IMapper mapper) : GetCommandHandler<GetByIdPropertyQuery, Domain.Entities.Property, PropertyDto>(_Repository, mapper)
    {
        public override Expression<Func<Domain.Entities.Property, bool>> CreateFilter(GetByIdPropertyQuery request)
        {           
            return e => e.Id == request.Id && e.Status !=Domain.Enums.Status.Deleted && e.Hide != true;
        }
    }
}