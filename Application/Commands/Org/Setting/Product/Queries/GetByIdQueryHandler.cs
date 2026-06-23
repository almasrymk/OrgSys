namespace Application.Commands.Org.Setting.Product.Queries
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

    public sealed record GetByIdProductQuery(long Id) : ICommand<ProductDto> , IGetByIdQuery<Result<ProductDto>>;

    public sealed class GetByIdQueryHandler(IRepository<Domain.Entities.Product> _Repository, IMapper mapper) : GetCommandHandler<GetByIdProductQuery, Domain.Entities.Product, ProductDto>(_Repository, mapper)
    {
        public override Expression<Func<Domain.Entities.Product, bool>> CreateFilter(GetByIdProductQuery request)
        {           
            return e => e.Id == request.Id && e.Status !=Domain.Enums.Status.Deleted && e.Hide != true;
        }

        public override string CreateInclude()
        {
            return "Classification,Dealer,ProductUnits,ProductUnits.Unit,ProductRecipes,ProductPropertyElements";
        }
    }
}