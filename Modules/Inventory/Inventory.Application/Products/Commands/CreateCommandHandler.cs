namespace Inventory.Application.Products.Commands
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;

    public sealed class CreateProductCommand : ProductDto , ICommand , ICreateCommand<Result>;

    public sealed class CreateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Inventory.Domain.Product> _Repository , IMapper mapper) : CreateCommandHandler<CreateProductCommand, Inventory.Domain.Product>(_UnitOfWork, _Repository , mapper)
    {
       
    }
}