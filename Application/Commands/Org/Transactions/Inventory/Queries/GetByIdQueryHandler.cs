namespace Application.Commands.Org.Transactions.Inventory.Queries
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

    public sealed record GetByIdInventoryQuery(long Id) : ICommand<InventoryModelView> , IGetByIdQuery<Result<InventoryModelView>>;

    public sealed class GetByIdQueryHandler(IRepository<Domain.Entities.Inventory> _Repository, IMapper mapper) : GetCommandHandler<GetByIdInventoryQuery, Domain.Entities.Inventory, InventoryModelView>(_Repository, mapper)
    {
        public override string CreateInclude()
        {
            return "InventoryProducts,InventoryProducts.Unit,InventoryProducts.Product";
        }

        public override Expression<Func<Domain.Entities.Inventory, bool>> CreateFilter(GetByIdInventoryQuery request)
        {           
            return e => e.Id == request.Id && e.Status !=Domain.Enums.Status.Deleted && e.Hide != true;
        }
    }
}