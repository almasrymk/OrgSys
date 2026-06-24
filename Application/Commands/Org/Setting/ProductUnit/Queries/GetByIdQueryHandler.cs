namespace Application.Commands.Org.Setting.ProductUnit.Queries
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

    public sealed record GetByIdProductUnitQuery(long Id) : ICommand<ProductUnitDto> , IGetByIdQuery<Result<ProductUnitDto>>;

    public sealed class GetByIdQueryHandler(IRepository<Domain.Entities.ProductUnit> _Repository, IMapper mapper) : GetCommandHandler<GetByIdProductUnitQuery, Domain.Entities.ProductUnit, ProductUnitDto>(_Repository, mapper)
    {
        public override Expression<Func<Domain.Entities.ProductUnit, bool>> CreateFilter(GetByIdProductUnitQuery request)
        {           
            return e => e.Id == request.Id && e.Status !=Domain.Enums.Status.Deleted && e.Hide != true;
        }
    }
}