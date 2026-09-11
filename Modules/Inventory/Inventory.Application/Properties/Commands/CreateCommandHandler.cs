namespace Inventory.Application.Properties.Commands
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;

    public sealed record CreatePropertyCommand(string Name) : ICommand , ICreateCommand<Result>;

    public sealed class CreateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Inventory.Domain.Property> _Repository , IMapper mapper) : CreateCommandHandler<CreatePropertyCommand, Inventory.Domain.Property>(_UnitOfWork, _Repository , mapper)
    {
       
    }
}